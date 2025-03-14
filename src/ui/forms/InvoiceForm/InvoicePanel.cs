using pečenko;
using System.Drawing;
using UblSharp;


public class InvoicePanel : Panel{
    public InvoicePanel(){
        _ = InitializeAsync();
    }

    public void Reload(){
        _ = InitializeAsync();
    }

    Font font = new Font("Courier New", 12, FontStyle.Regular);

    private async Task InitializeAsync(){
        this.Controls.Clear();
        this.ForeColor = ColorTranslator.FromHtml("#effbff");

        InvoiceType invoice = await Task.Run(()=> Program.invoiceOrchestrator?.getInvoice()??new InvoiceType());
        int itemLength = 35;
        int itemX = 0;
        row = 3;
        addItem(nameValueConcat("id", invoice?.ID?.Value??"N/A", itemLength), itemX);
        addItem(nameValueConcat("datum prometa", invoice?.IssueDate?.ValueString??"N/A", itemLength), itemX);
        addItem(nameValueConcat("datum dospeća", invoice?.DueDate?.ValueString??"N/A", itemLength), itemX);

        itemX = 400;row=3;
        addItem(nameValueConcat("za plaćanje", invoice?.LegalMonetaryTotal?.TaxInclusiveAmount?.Value.ToString()??"N/A", itemLength), itemX);
        addItem(nameValueConcat("bez PDVa", invoice?.LegalMonetaryTotal?.TaxExclusiveAmount?.Value.ToString()??"N/A", itemLength), itemX);
        addItem(nameValueConcat("valuta", invoice?.LegalMonetaryTotal?.TaxInclusiveAmount?.currencyID??"N/A", itemLength), itemX);
    
        itemX = 0;row+=2;itemLength=75;
        if(invoice?.AccountingCustomerParty?.Party?.PartyName?.Count > 0)
            addItem(nameValueConcat("kupac", invoice?.AccountingCustomerParty?.Party?.PartyName[0]?.Name??"N/A", itemLength), itemX);

        String kupacAddress = invoice?.AccountingCustomerParty?.Party?.PostalAddress?.StreetName??"N/A" + ", " + invoice?.AccountingCustomerParty?.Party?.PostalAddress?.CityName??"N/A";
        addItem(nameValueConcat("   adresa",kupacAddress, itemLength), itemX);
        if(invoice?.AccountingCustomerParty?.Party?.PartyTaxScheme?.Count > 0)
            addItem(nameValueConcat("   PIB",invoice?.AccountingCustomerParty?.Party?.PartyTaxScheme[0]?.CompanyID??"N/A", itemLength), itemX);
        if(invoice?.AccountingCustomerParty?.Party?.PartyLegalEntity?.Count > 0)
            addItem(nameValueConcat("   matični broj",invoice?.AccountingCustomerParty?.Party?.PartyLegalEntity[0]?.CompanyID??"N/A", itemLength), itemX);

        row++;

        if(invoice?.AccountingSupplierParty?.Party?.PartyName?.Count > 0)
            addItem(nameValueConcat("prodavac", invoice?.AccountingSupplierParty?.Party?.PartyName[0]?.Name??"N/A", itemLength), itemX);
        String prodavacAddress = invoice?.AccountingSupplierParty?.Party.PostalAddress?.StreetName??"N/A" + ", " + invoice?.AccountingCustomerParty?.Party?.PostalAddress?.CityName??"N/A";
        addItem(nameValueConcat("   adresa",prodavacAddress, itemLength), itemX);
        if(invoice?.AccountingSupplierParty?.Party.PartyTaxScheme?.Count > 0)
            addItem(nameValueConcat("   PIB",invoice?.AccountingSupplierParty?.Party.PartyTaxScheme[0]?.CompanyID??"N/A", itemLength), itemX);
        if(invoice?.AccountingSupplierParty?.Party.PartyLegalEntity?.Count > 0)
            addItem(nameValueConcat("   matični broj",invoice?.AccountingSupplierParty?.Party.PartyLegalEntity[0]?.CompanyID??"N/A", itemLength), itemX);

    }

    private async void Async_Load(object sender, EventArgs e){
        await InitializeAsync();
    }

    int widthMargin = 10;
    int heightMargin = 10;
    int spacing = 2;
    int row=0;
    private void addItem(string text, int x){
        Label label = new Label();
        label.Text = text;
        label.AutoSize = true;
        label.Font = font;
        label.Location = new System.Drawing.Point(widthMargin + x, heightMargin + ((font.Height + spacing) * row));
        this.Controls.Add(label);
        row++;
    }





    private string nameValueConcat(string name, string value, int length){
        if(name == null || value == null){
            return "N/A";
        }
        if(name.Length+value.Length > length-5)
            value = value.Substring(0, value.Length-5);
        String dots = name.Length+value.Length<length ? 
                        new string('.', length - (name.Length+value.Length)) : new string('.', 5);
        return (name+dots+value).Substring(0, length);
    }
}