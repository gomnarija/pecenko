using System.Xml.Serialization;
using UblSharp;
using UblSharp.CommonAggregateComponents;

[Serializable]
public class ResponseEnvelope{
    public string code;
    
    [XmlIgnore]
    public InvoiceType invoice{
        get{ return invoiceSerializable?.toInvoiceType()??new InvoiceType(); }
        set{ 
            invoiceSerializable = new InvoiceTypeSerializable(value); 
        }
    }

    public InvoiceTypeSerializable? invoiceSerializable;

    [XmlIgnore]
    public List<InvoiceLineType> inserted{
        get{
            List<InvoiceLineType> _inserted = new List<InvoiceLineType>();
            foreach (InvoiceLineTypeSerializable line in insertedSerializable??new List<InvoiceLineTypeSerializable>()){
                _inserted.Add(line.toInvoiceLineType());
            }
            return _inserted;
            }
        set{
            insertedSerializable = new List<InvoiceLineTypeSerializable>();
            foreach(InvoiceLineType line in value){
                insertedSerializable.Add(new InvoiceLineTypeSerializable(line));
            }
        }
    }

    public List<InvoiceLineTypeSerializable>? insertedSerializable;

    public List<QuantitiesTypeSerializable>? quantities;

    public ResponseEnvelope(){
        code = NetworkingConstants.MESSAGE_CODE_OK;
    }

    public static ResponseEnvelope Fail(){
        return new ResponseEnvelope(){
            code = NetworkingConstants.MESSAGE_CODE_FAIL
        };
    }

    public static ResponseEnvelope OK(){
        return new ResponseEnvelope(){
            code = NetworkingConstants.MESSAGE_CODE_OK
        };
    }
}