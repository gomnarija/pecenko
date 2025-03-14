namespace pečenko;
using System.Xml.Serialization;
using UblSharp.CommonAggregateComponents;

[Serializable]
public class RequestEnvelope
{
    public string? userName;
    public string? action;

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

    public List<string>? ids;


    public RequestEnvelope() { 
    }

    public RequestEnvelope(string name, string action){
        this.userName = name;
        this.action = action;
    }

    public RequestEnvelope(string name, string action, List<InvoiceLineType> inserted){
        this.userName = name;
        this.action = action;
        this.inserted = inserted;
    }

    public RequestEnvelope(string name, string action, InvoiceLineType inserted){
        this.userName = name;
        this.action = action;
        this.inserted = new List<InvoiceLineType>(){inserted};
    }

public RequestEnvelope(string name, string action, List<string> ids){
        this.userName = name;
        this.action = action;
        this.ids = ids;
    }

    public static RequestEnvelope Fail(){
        RequestEnvelope requestEnvelope= new RequestEnvelope();
        return requestEnvelope;
    }
}