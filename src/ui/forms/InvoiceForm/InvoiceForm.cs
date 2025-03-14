using UblSharp.CommonAggregateComponents;

namespace pečenko;

public partial class InvoiceForm : Form
{
    public InvoiceForm(){
        InitializeComponent();
        Program.checkForAutoSave();
    }

    public InvoiceForm(InvoiceLineType invoiceLine){
        InitializeComponent();
    }

    public void Reload(){
        Program.checkForAutoSave();
        invoicePanel.Reload();
        fakturaButton.PerformClick();
    }



    private void ButtonSelection(object sender){
        foreach(Button button in new List<Button>{fakturaButton, artikliButton, prosloButton, presekButton}){
            if(button == sender){
                button.BackColor = ColorTranslator.FromHtml("#a0aebe");
                button.FlatAppearance.BorderSize = 0;
            }else{
                button.BackColor = ColorTranslator.FromHtml("#d2d7d3");
                button.FlatAppearance.BorderSize = 1;
            }
        }
    }


    private void FakturaButton_Click(object sender, EventArgs e){
        this.invoicePanel.Visible = true;
        this.invoiceLinesPanel.Visible = false;
        this.invoicePanel.Enabled = true;
        this.invoiceLinesPanel.Enabled = false;
        ButtonSelection(this.fakturaButton);
    }

    private async void ArtikliButton_Click(object sender, EventArgs e){
        this.invoiceLinesPanel.Visible = true;
        this.invoicePanel.Visible = false;
        this.invoiceLinesPanel.Enabled = true;
        this.invoicePanel.Enabled = false;
        this.invoiceLinesPanel.currentView = InvoiceLinesPanel.CurrentView.Artikli;
        ButtonSelection(this.artikliButton);
        await this.invoiceLinesPanel.FillGridInvoiceAsync();
    }

    private async void ProsloButton_Click(object sender, EventArgs e){
        this.invoiceLinesPanel.Visible = true;
        this.invoicePanel.Visible = false;
        this.invoiceLinesPanel.Enabled = true;
        this.invoicePanel.Enabled = false;
        this.invoiceLinesPanel.currentView = InvoiceLinesPanel.CurrentView.Proslo;
        ButtonSelection(this.prosloButton);
        await this.invoiceLinesPanel.FillGridInsertedInvoiceLinesAsync();
    }

    private async void PresekButton_Click(object sender, EventArgs e){
        this.invoiceLinesPanel.Visible = true;
        this.invoicePanel.Visible = false;
        this.invoiceLinesPanel.Enabled = true;
        this.invoicePanel.Enabled = false;
        this.invoiceLinesPanel.currentView = InvoiceLinesPanel.CurrentView.Presek;
        ButtonSelection(this.presekButton);
        await this.invoiceLinesPanel.FillGridInvoiceRemainingAsync();
    }


    private void Close(object sender, EventArgs e){
        this.Close();
    }

    private void Minimize(object sender, EventArgs e){
        this.WindowState = FormWindowState.Minimized;
    }

    private void LoadInserted(object sender, EventArgs e){
        Program.loadInsertedForm();
        if(this.invoiceLinesPanel.currentView == InvoiceLinesPanel.CurrentView.Proslo)
            _ = this.invoiceLinesPanel.FillGridInsertedInvoiceLinesAsync();

    }
    private void SaveInserted(object sender, EventArgs e){
        Program.saveInsertedForm();
    }

    private void LoadNewInvoice(object sender, EventArgs e){
        Program.loadInvoiceForm();
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



    private async void Sync(object sender, EventArgs e){
        await Task.Run(() => {Program.clientSync();});
        invoiceLinesPanel.InsertedUpdated(sender, e);
    }
}
