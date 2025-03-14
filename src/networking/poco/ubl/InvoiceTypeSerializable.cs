using System.Xml.Serialization;
using UblSharp;
using UblSharp.CommonAggregateComponents;
using UblSharp.UnqualifiedDataTypes;

[Serializable]
public class InvoiceTypeSerializable {

    public IdentifierType? ID;
    public DateType? issueDate;
    public DateType? dueDate;
    public MonetaryTotalType? legalMonetaryTotal;
    public CustomerPartyType? accountingCustomerParty;
    public SupplierPartyType? accountingSupplierParty;
    
    [XmlIgnore]
    public List<InvoiceLineType> invoiceLine{
        get{
            List<InvoiceLineType> res = new List<InvoiceLineType>();
            foreach(var item in invoiceLineTypeSerializables??new List<InvoiceLineTypeSerializable>()){
                res.Add(item.toInvoiceLineType());
            }
            return res;
        }
    }

    public List<InvoiceLineTypeSerializable>? invoiceLineTypeSerializables;

    public InvoiceTypeSerializable(){

    }

    public InvoiceTypeSerializable(InvoiceType invoiceType){
        ID = invoiceType.ID;
        issueDate = invoiceType.IssueDate;
        dueDate = invoiceType.DueDate;
        legalMonetaryTotal = invoiceType.LegalMonetaryTotal;
        accountingCustomerParty = invoiceType.AccountingCustomerParty;
        accountingSupplierParty = invoiceType.AccountingSupplierParty;
        foreach(var item in invoiceType.InvoiceLine){
            if(invoiceLineTypeSerializables != null){
                invoiceLineTypeSerializables.Add(new InvoiceLineTypeSerializable(item));
            }
            else{
                invoiceLineTypeSerializables = new List<InvoiceLineTypeSerializable>(){new InvoiceLineTypeSerializable(item)};
            }
        }
    }


    public InvoiceType toInvoiceType(){
        InvoiceType invoiceType = new InvoiceType();
        invoiceType.ID = ID;
        invoiceType.IssueDate = issueDate;
        invoiceType.DueDate = dueDate;
        invoiceType.LegalMonetaryTotal = legalMonetaryTotal;
        invoiceType.AccountingCustomerParty = accountingCustomerParty;
        invoiceType.AccountingSupplierParty = accountingSupplierParty;
        invoiceType.InvoiceLine = invoiceLine;
        return invoiceType;
    }
}