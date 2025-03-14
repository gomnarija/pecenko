using System.Xml.Serialization;
using UblSharp.CommonAggregateComponents;
using UblSharp.UnqualifiedDataTypes;

[Serializable]
public class InvoiceLineTypeSerializable {

    public IdentifierType? ID;
    
    [XmlIgnore]
    public ItemType item{
        get{
            return this.itemTypeSerializable?.toTimeType()??new ItemType();
        }
        set{
            this.itemTypeSerializable = new ItemTypeSerializable(value);
        }
    }
    public ItemTypeSerializable? itemTypeSerializable;
    public QuantityType? invoicedQuantity;
    public PriceType? price;
    public InvoiceLineTypeSerializable(){

    }

    public InvoiceLineTypeSerializable(InvoiceLineType invoiceLineType){
        ID = invoiceLineType.ID;
        item = invoiceLineType.Item;
        invoicedQuantity = invoiceLineType.InvoicedQuantity;
        price = invoiceLineType.Price;
    }


    public InvoiceLineType toInvoiceLineType(){
        InvoiceLineType invoiceLineType = new InvoiceLineType();
        invoiceLineType.ID = ID;
        invoiceLineType.Item = item;
        invoiceLineType.InvoicedQuantity = invoicedQuantity;
        invoiceLineType.Price = price;
        return invoiceLineType;
    }
}