using UblSharp.CommonAggregateComponents;
using System.Windows.Forms;

namespace pečenko;

public partial class Insert : Form
{
    public Insert(bool oneTime=false){
        this.oneTime=oneTime;
        InitializeComponent();
        this.kodTextBox.Select();
    }

    public Insert(InvoiceLineType invoiceLine, bool oneTime=false){
        this.oneTime=oneTime;
        InitializeComponent();
        fillIn(invoiceLine);
        this.kolicinaTextBox.Select();
    }

    public Insert(String text, bool oneTime=false){
        this.oneTime=oneTime;
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


    private void insertInvoiceLine(object sender, EventArgs e){
        String name = nazivTextBox.Text;
        String code = kodTextBox.Text;
        String unitCode = meraTextBox.Text;
        String quantity = kolicinaTextBox.Text;
        String price = cenaTextBox.Text;
        String currency = valutaTextBox.Text;

        if(name == null || code == null || name == "" || code == ""){
            MessageBox.Show("naziv i kod su obavezni.");
            return;
        }

        InvoiceLineType invoiceLineType = new InvoiceLineType();
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
        }else if (dq > 10000){
            MessageBox.Show("količina mora biti manja od 10000");
        }


        Program.invoiceOrchestrator?.insertInvoiceLine(invoiceLineType);


        if(!oneTime){
            clearOut();
            this.kodTextBox.Select();
        }else{
            this.Close();
        }
    }


    private List<String> collectInvoiceLineNames(){
        List<String> names = new List<String>();

        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInvoice()?.InvoiceLine ?? new List<InvoiceLineType>()){
            names.Add(invoiceLine.Item?.Name?.Value??"");
        }


        return names.Distinct().ToList();
    }

    private List<String> collectInvoiceCodes(){
        List<String> names = new List<String>();

        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInvoice()?.InvoiceLine ?? new List<InvoiceLineType>()){
            names.Add(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"");
        }


        return names.Distinct().ToList();
    }


    private void fillIn(InvoiceLineType invoiceLine){
        //name
        this.nazivTextBox.Text = invoiceLine.Item?.Name?.Value??"N/A";
        //kod
        this.kodTextBox.Text = invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"N/A";
        //mera
        this.meraTextBox.Text = Program.invoiceOrchestrator?.getUnitOfMeasurementFromCode(invoiceLine.InvoicedQuantity?.unitCode??"N/A");
        //količina
        this.kolicinaTextBox.Text = invoiceLine.InvoicedQuantity?.Value.ToString("G29")??"N/A";
        //cena
        this.cenaTextBox.Text = invoiceLine.Price?.PriceAmount?.Value.ToString("G29")??"N/A";
         //valuta
        this.valutaTextBox.Text = invoiceLine.Price?.PriceAmount?.currencyID??"N/A";

    }

    private void clearOut(){
        //name
        this.nazivTextBox.Clear();
        //kod
        this.kodTextBox.Clear();
        //mera
        this.meraTextBox.Clear();
        //količina
        this.kolicinaTextBox.Clear();
        //cena
        this.cenaTextBox.Clear();
         //valuta
        this.valutaTextBox.Clear();
    }


    private void fillInFromName(object sender, EventArgs e){
        if(!nazivTextBox.Focused)
            return;

        String name = nazivTextBox.Text;
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInvoice()?.InvoiceLine ?? new List<InvoiceLineType>()){
            if(name.Equals(invoiceLine.Item?.Name?.Value??"") && name != ""){
                fillIn(invoiceLine);
                this.kolicinaTextBox.Focus();
                return;
            }
        }
    }

    private void fillInFromCode(object sender, EventArgs e){
        if(!kodTextBox.Focused)
            return;
            
        String code = kodTextBox.Text;
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInvoice()?.InvoiceLine ?? new List<InvoiceLineType>()){
            if(code.Equals(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"") && code != ""){
                fillIn(invoiceLine);
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



    private void Insert_KeyDown(object sender, KeyEventArgs e){
        if (e.KeyCode == Keys.Escape){
            this.Close();
        }else if(e.KeyCode == Keys.Enter){
            this.SelectNextControl(this.ActiveControl, true, true, true, true);
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
