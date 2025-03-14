using UblSharp;
using UblSharp.CommonAggregateComponents;
using System.Resources;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.CodeDom;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;

namespace pečenko;

static class Program
{
    public static InvoiceForm? invoiceForm;
    private static MainForm? mainForm;
    
    public enum ProgramMode{
        None,
        Server,
        Client
    };

    private static ProgramMode programMode = ProgramMode.None;
    private static Mandrak? mandrak;
    private static Fantom? fantom;
    private static string userName = ProgramConstans.PROGRAM_NAME;
    private static InvoiceFilesOrchestrator invoiceFilesOrchestrator = new InvoiceFilesOrchestrator();
    public static IInvoiceOrchestrator? invoiceOrchestrator;
    private static string serverIp = ProgramConstans.IP;


    public static string getUserName(){
        return userName;
    }
    public static void setUserName(string name){
        if(userName == ProgramConstans.PROGRAM_NAME && name.Length > 0){
            userName = name.Trim();
        }
    }

    public static string getServerIp(){
        return serverIp;
    }
    public static void setServerIp(string ip){
        if(serverIp == ProgramConstans.IP && ip.Length > 0){
            serverIp = ip;
        }
    }

    public static ProgramMode getProgramMode(){
        return programMode;
    }

/******************************************************************************
    Forms
******************************************************************************/

    public static void loadInvoiceForm(){
        InvoiceType invoice = new InvoiceType();
        FileForm.OnButtonClick loadInvoice = (string path) => {
            invoice = invoiceFilesOrchestrator.loadInvoiceFromFile(path);
        };
        FileForm fileForm = new FileForm("putanja do fakture: ", "otvori", loadInvoice);
        fileForm.StartPosition = FormStartPosition.CenterScreen;
        fileForm.ShowDialog();
        if((invoice.InvoiceLine??new List<InvoiceLineType>()).Count > 0){
            mandrak?.invoiceOrchestrator.setInvoice(invoice);
            if(invoiceForm!=null){
                invoiceForm.Reload();
            }else{
                invoiceForm = new InvoiceForm();
                invoiceForm.StartPosition = FormStartPosition.CenterScreen;
                Application.Run(invoiceForm);
            }
        }
    }

    public static void saveInsertedForm(){
        FileForm.OnButtonClick saveInserted = (string path) => {
            invoiceFilesOrchestrator.saveInsertedToFile(path, invoiceOrchestrator?.getInserted() ?? new List<InvoiceLineType>());
        };
        FileForm main = new FileForm("putanja za čuvanje: ", "sačuvaj", saveInserted, true);
        main.StartPosition = FormStartPosition.CenterScreen;
        main.ShowDialog();
        main.Close();
    }

    public static void loadInsertedForm(){
        FileForm.OnButtonClick loadInserted = (string path) => {
            List<InvoiceLineType> inserted = new List<InvoiceLineType>();
            if(programMode == ProgramMode.Server){
                invoiceFilesOrchestrator.loadInsertedFromFile(path, ref inserted);
                mandrak?.invoiceOrchestrator?.setInserted(inserted);
            }
        };
        FileForm main = new FileForm("putanja do unetih artikala: ", "otvori", loadInserted);
        main.StartPosition = FormStartPosition.CenterScreen;
        main.ShowDialog();
        main.Close();
    }

    public static void connectClientForm(){
        ConnectForm connectForm = new ConnectForm();
        connectForm.StartPosition = FormStartPosition.CenterScreen;
        connectForm.ShowDialog();
        fantom = new Fantom(serverIp);
        if(fantom.isConnected){
            invoiceOrchestrator = fantom.invoiceOrchestratorClient;
            invoiceForm = new InvoiceForm();
            invoiceForm.StartPosition = FormStartPosition.CenterScreen;
            Application.Run(invoiceForm);
        }
    }

/******************************************************************************
    AutoSave
******************************************************************************/

    public static void saveInserted(){
        if(invoiceFilesOrchestrator.savePath == null){
            saveInsertedForm();
        }else{
            invoiceFilesOrchestrator.saveInsertedToFile(invoiceFilesOrchestrator.savePath, invoiceOrchestrator?.getInserted() ?? new List<InvoiceLineType>());
        }
    }

    public static void autoSaveInserted(Object? sender, EventArgs eventArgs) {
        if((invoiceOrchestrator?.getInvoice()?.ID.Value??"N/a") == "N/a" ||
            programMode != ProgramMode.Server){
            return;
        }
        String fileName = mandrak?.invoiceOrchestrator?.getAutoSaveFileName()??"default_autosave.xml";
        invoiceFilesOrchestrator.saveInsertedToFile(fileName, invoiceOrchestrator?.getInserted() ?? new List<InvoiceLineType>());
    }

    public static void checkForAutoSave(){
        if((invoiceOrchestrator?.getInvoice()?.ID.Value??"N/a") == "N/a" ||
            programMode != ProgramMode.Server){
            return;
        }
        String fileName = mandrak?.invoiceOrchestrator?.getAutoSaveFileName()??"default_autosave.xml";
        String invoiceName = mandrak?.invoiceOrchestrator?.getInvoice()?.ID??"invoice_name";
        if(File.Exists(fileName)){
            DialogResult result = 
                MessageBox.Show("pronađen samosačuvani fajl " + invoiceName + " da li želiš da ga učitaš ?", "pečenko", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes){
                List<InvoiceLineType> inserted = new List<InvoiceLineType>();
                invoiceFilesOrchestrator.loadInsertedFromFile(fileName, ref inserted);
                mandrak?.invoiceOrchestrator?.setInserted(inserted);
            }
        }
    }


/******************************************************************************
    MAIN
******************************************************************************/
    public static void serverModeStart(){
        if(programMode != ProgramMode.None){
            return;
        }
        programMode = ProgramMode.Server;
        //server
        mandrak = new Mandrak();
        mandrak.Start();
    }

    public static void clientModeStart(){
        if(programMode != ProgramMode.None){
            return;
        }
        programMode = ProgramMode.Client;
    }

    public static void clientSync(){
        if(programMode == ProgramMode.Client && (fantom?.isConnected??false)){
            fantom.invoiceOrchestratorClient.fetchInvoice();
            fantom.invoiceOrchestratorClient.fetchInserted();
        }
    }


    [STAThread]
    static void Main(){
        if (!Directory.Exists(ProgramConstans.APP_DATA_DIR_PATH)){
            Directory.CreateDirectory(ProgramConstans.APP_DATA_DIR_PATH);
        }

        ApplicationConfiguration.Initialize();

        // Main Form, selects program mode
        mainForm = new MainForm();
        mainForm.StartPosition = FormStartPosition.CenterScreen;
        mainForm.ShowDialog();

        if(programMode == ProgramMode.Server){
            if(mandrak != null){
                mandrak.invoiceOrchestrator.Updated += autoSaveInserted;
                invoiceOrchestrator = mandrak?.invoiceOrchestrator;
                loadInvoiceForm();
            }
        }else if(programMode == ProgramMode.Client){
            connectClientForm();
            fantom?.disconnect();
        }

    }
}