using UblSharp.CommonAggregateComponents;
using System.Windows.Forms;

namespace pečenko;

public partial class Result : Form
{
    public Result()
    {
        InitializeComponent();
        kodTextBox.Select();
    }

    public Result(InvoiceLineType invoiceLine){
        InitializeComponent();
        fillIn(invoiceLine);
        this.okButton.Select();
    }

    public Result(String text){
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

    private void fine(object sender, EventArgs e){
        this.Close();
    }

    private void edit(object sender, EventArgs e){
        String code = kodTextBox.Text;
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInserted()?? new List<InvoiceLineType>()){
            if(code.Equals(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"") && code != ""){
                EditInserted editInserted = new EditInserted(invoiceLine);
                editInserted.StartPosition = FormStartPosition.CenterScreen;
                editInserted.ShowDialog();
                fillInFromCode(code);
                closeIfZero();
                return;
            }
        }
        //inserted not found, open insert form
        insert(sender, e);
    }

    private void insert(object sender, EventArgs e){
        String code = kodTextBox.Text;
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInvoice()?.InvoiceLine ?? new List<InvoiceLineType>()){
            if(code.Equals(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"") && code != ""){
                Insert insert = new Insert(invoiceLine, true);
                insert.StartPosition = FormStartPosition.CenterScreen;
                insert.ShowDialog();
                fillIn(invoiceLine);
                closeIfZero();
                return;
            }
        }
        MessageBox.Show("artikal nije pronadjen");
    }

    private void closeIfZero(){
        decimal remainingQuantity = Program.invoiceOrchestrator?.getRemainingQuantityForCode(this.kodTextBox.Text) ?? 0;
        if(remainingQuantity==0)
            Close();
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
        //presek
        decimal remainingQuantity = Program.invoiceOrchestrator?.getRemainingQuantityForCode(this.kodTextBox.Text)??0;
        this.presekTextBox.Text = remainingQuantity.ToString("G29");
        //originalna kolicina
        decimal originalQuantity = invoiceLine.InvoicedQuantity?.Value??0;
        this.originalnaKolicinaTextBox.Text = originalQuantity.ToString("G29");
        //uneto
        decimal insertedQuantity = Program.invoiceOrchestrator?.getInsertedQuantityForCode((invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"N/A"))??0;
        this.unetoTextBox.Text = insertedQuantity.ToString("G29");
        //cena
        this.cenaTextBox.Text = invoiceLine.Price?.PriceAmount?.Value.ToString("G29")??"N/A";
         //valuta
        this.valutaTextBox.Text = invoiceLine.Price?.PriceAmount?.currencyID??"N/A";
    }


    private void fillInFromName(object sender, EventArgs e){
        if(!nazivTextBox.Focused)
            return;
        String name = nazivTextBox.Text;
        foreach(InvoiceLineType invoiceLine in Program.invoiceOrchestrator?.getInvoice()?.InvoiceLine ?? new List<InvoiceLineType>()){
            if(name.Equals(invoiceLine.Item?.Name?.Value??"") && name != ""){
                fillIn(invoiceLine);
                this.presekTextBox.Focus();
                return;
            }
        }
    }

    private void fillInFromCode(object sender, EventArgs e){
        if(!kodTextBox.Focused)
            return;
        String code = kodTextBox.Text;
        fillInFromCode(code);
    }
    private void fillInFromCode(String code){
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



    private void Result_KeyDown(object sender, KeyEventArgs e){
        if (e.KeyCode == Keys.Escape){
            this.Close();
        }else if(e.KeyCode == Keys.Enter){
            this.SelectNextControl(this.ActiveControl, true, true, true, true);
        }else if(e.KeyCode == Keys.I && !nazivTextBox.Focused && !kodTextBox.Focused){
            edit(sender, e);
        }else if(e.KeyCode == Keys.U && !nazivTextBox.Focused && !kodTextBox.Focused){
            insert(sender, e);
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
