using UblSharp.CommonAggregateComponents;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;

namespace pečenko;

public partial class EditInserted : Form
{
    public EditInserted()
    {
        InitializeComponent();
        this.kodTextBox.Select();
    }

    public EditInserted(InvoiceLineType eInvoiceLine){
        InitializeComponent();
        String code = eInvoiceLine.Item?.SellersItemIdentification?.ID?.Value??"";
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInserted()?? new List<InvoiceLineType>()){
            if(code.Equals(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"") && code != ""){
                fillIn(invoiceLine);
                this.unetoTextBox.Select();
                return;
            }
        }
        MessageBox.Show("uneti artikal nije pronadjen.");
    }

    public EditInserted(String text){
        InitializeComponent();
        if(text == "")
            return;

        //name
        if(char.IsLetter(text[0])){
            nazivTextBox.Select();
            SendKeys.Send(text);
        }else if(char.IsNumber(text[0])){
            kodTextBox.Select();
            SendKeys.Send(text);
        }
        
    }


    private void editInsertedInvoiceLine(object sender, EventArgs e){
        String name = nazivTextBox.Text;
        String code = kodTextBox.Text;
        String unitCode = meraTextBox.Text;
        String quantity = unetoTextBox.Text;
        String price = cenaTextBox.Text;
        String currency = valutaTextBox.Text;
        String id = idComboBox.Text;

        if(name == null || code == null || name == "" || code == ""){
            MessageBox.Show("naziv i kod su obavezni.");
            return;
        }

        InvoiceLineType invoiceLineType = new InvoiceLineType();
        invoiceLineType.ID = id;
        invoiceLineType.Item.Name = name;
        invoiceLineType.Item.SellersItemIdentification.ID = code;
        invoiceLineType.InvoicedQuantity.unitCode = unitCode;
        decimal.TryParse(quantity, out decimal dq);
        invoiceLineType.InvoicedQuantity.Value = dq;
        decimal.TryParse(price, out decimal pq);
        invoiceLineType.Price.PriceAmount.Value = pq;
        invoiceLineType.Price.PriceAmount.currencyID = currency;

        if(dq <= 0){
            MessageBox.Show("količina mora biti veća od 0");
            return;
        }

        Program.invoiceOrchestrator?.editInsertedInvoiceLine(invoiceLineType);

        this.Close();
    }

    private void deleteInsertedInvoiceLine(object sender, EventArgs e){
        String code = kodTextBox.Text;
        String id = "";
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInserted()?? new List<InvoiceLineType>()){
            if(code.Equals(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"") && code != ""){
                id = invoiceLine?.ID?.Value??"";
                break;
            }
        }
        if(id!=""){
            Program.invoiceOrchestrator?.removeInsertedInvoiceLine(id);
            this.Close();
        }else{
            MessageBox.Show("uneti artikal nije pronadjen.");
        }
    }


    private List<String> collectInsertedInvoiceLineNames(){
        List<String> names = new List<String>();

        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInserted()?? new List<InvoiceLineType>()){
            names.Add(invoiceLine.Item?.Name?.Value??"");
        }


        return names.Distinct().ToList();
    }

    private List<String> collectInsertedInvoiceIds(String code){
        List<String> ids = new List<String>();
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInserted()?? new List<InvoiceLineType>()){
            if(invoiceLine?.Item?.SellersItemIdentification?.ID?.Value.Equals(code)??false)
                ids.Add(invoiceLine.ID??"");
        }
        return ids.Distinct().ToList();
    }

    private List<String> collectInsertedInvoiceCodes(){
        List<String> names = new List<String>();
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInserted()?? new List<InvoiceLineType>()){
            names.Add(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"");
        }
        return names.Distinct().ToList();
    }

    private decimal getOriginalQuantity(String code){
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInvoice()?.InvoiceLine ?? new List<InvoiceLineType>()){
            if(code.Equals(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"") && code != ""){
                return invoiceLine.InvoicedQuantity?.Value ?? decimal.Zero;
            }
        }
        return decimal.Zero;
    }


    private void fillIn(InvoiceLineType invoiceLine, bool idFill=false){
        //name
        this.nazivTextBox.Text = invoiceLine.Item?.Name?.Value??"N/A";
        //kod
        this.kodTextBox.Text = invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"N/A";
        //mera
        this.meraTextBox.Text = Program.invoiceOrchestrator?.getUnitOfMeasurementFromCode(invoiceLine.InvoicedQuantity?.unitCode??"N/A");
        //količina
        this.unetoTextBox.Text = invoiceLine.InvoicedQuantity?.Value.ToString("G29")??"N/A";
        //cena
        this.cenaTextBox.Text = invoiceLine.Price?.PriceAmount?.Value.ToString("G29")??"N/A";
        //valuta
        this.valutaTextBox.Text = invoiceLine.Price?.PriceAmount?.currencyID??"N/A";
        //originalna kolicina
        decimal originalQuantity = getOriginalQuantity(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"");
        this.originalnaKolicinaTextBox.Text = originalQuantity.ToString("G29");
        //id
        if(!idFill){
            idComboBox.Items.Clear();
            idComboBox.Enabled = true;
            idComboBox.Items.AddRange(collectInsertedInvoiceIds(kodTextBox.Text)?.ToArray()??new List<String>().ToArray());
            //get index ( inside comboBox ) of currently selected id
            String currentId = invoiceLine.ID??"";
            int currentSelectedIndex = 0;
            for(int i = 0 ; i < idComboBox.Items.Count ; i++){
                if(idComboBox.Items[i]?.Equals(currentId)??false){
                    currentSelectedIndex = i;
                    break;
                }
            }
            //set to currently selected
            if(idComboBox.Items.Count > currentSelectedIndex){
                idComboBox.SelectedIndex = currentSelectedIndex;
            }else if(idComboBox.Items.Count > 0){
                idComboBox.SelectedIndex = 0;
            }
            //if there are no multiple options, disable it
            if(idComboBox.Items.Count==1){
                idComboBox.Enabled = false;
            }
        }
    }


    private void fillInFromName(object sender, EventArgs e){
        if(!nazivTextBox.Focused)
            return;

        String name = nazivTextBox.Text;
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInserted() ?? new List<InvoiceLineType>()){
            if(name.Equals(invoiceLine.Item?.Name?.Value??"") && name != ""){
                fillIn(invoiceLine);
                this.unetoTextBox.Focus();
                return;
            }
        }
    }

    private void fillInFromCode(object sender, EventArgs e){
        if(!kodTextBox.Focused)
            return;

        String code = kodTextBox.Text;
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInserted() ?? new List<InvoiceLineType>()){
            if(code.Equals(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"") && code != ""){
                fillIn(invoiceLine);
                return;
            }
        }
    }

    private void fillInFromId(object sender, EventArgs e){
        if(!idComboBox.Focused)
            return;

        String id = idComboBox.Text;
        //find code from insertedLines
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInserted() ?? new List<InvoiceLineType>()){
            if(id != null && id.Equals(invoiceLine.ID??"") && id != ""){
                fillIn(invoiceLine, true);
                return;
            }
        }
    }




    private void Close(object sender, EventArgs e){
        this.Close();
    }

    private void Minimize(object sender, EventArgs e){
        this.WindowState = FormWindowState.Minimized;
    }


    private Point startDragPoint;
    private void MenuStrip_MouseDown(object sender, MouseEventArgs e){
        if (e.Button == MouseButtons.Left)
        {
            startDragPoint = new Point(e.X, e.Y);
        }
    }
    private void MenuStrip_MouseMove(object sender, MouseEventArgs e){
        if (e.Button == MouseButtons.Left)
        {
            int deltaX = e.X - startDragPoint.X;
            int deltaY = e.Y - startDragPoint.Y;
            this.Location = new Point(this.Left + deltaX, this.Top + deltaY);
        }
    }



    private void EditInserted_KeyDown(object sender, KeyEventArgs e){
        if (e.KeyCode == Keys.Escape){
            this.Close();
        }else if(e.KeyCode == Keys.Enter){
            this.SelectNextControl(this.ActiveControl, true, true, true, true);
        }else if(e.KeyCode == Keys.U || e.KeyCode == Keys.Delete && !nazivTextBox.Focused && !kodTextBox.Focused){
            deleteInsertedInvoiceLine(sender, e);
        }
    }

    //this is just to handle enter, stops it from beeping
    private void Enter_NextControl(object sender, KeyPressEventArgs  e){
        if (e.KeyChar == (char)Keys.Enter){
            e.Handled = true;
        }
    }

    private void TextBox_Enter(object sender, EventArgs e){
            TextBox? textBox = sender as TextBox;
            if (textBox != null){
                textBox.BackColor = TextBoxSelectedBackColor;
            }
    }

    private void TextBox_Leave(object sender, EventArgs e){
            TextBox? textBox = sender as TextBox;
            if (textBox != null){
                textBox.BackColor = backUIColor;
            }
    }


    private void Button_Enter(object sender, EventArgs e){
            Button? button = sender as Button;
            if (button != null){
                button.FlatAppearance.BorderColor = TextBoxSelectedBackColor;
            }
    }

    private void Button_Leave(object sender, EventArgs e){
            Button? button = sender as Button;
            if (button != null){
                button.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#eaf2d7");
            }
    }
}
