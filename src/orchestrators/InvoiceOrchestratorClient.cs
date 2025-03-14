namespace pečenko;

using Microsoft.Xml.Serialization.GeneratedAssembly;
using UblSharp;
using UblSharp.CommonAggregateComponents;

class invoiceOrchestratorClient : IInvoiceOrchestrator{
    private InvoiceType? invoice = null;
    private List<InvoiceLineType>? insertedInvoiceLines = null;
    private Dictionary<String, String> unitOfMeasurementCodeLookup = new Dictionary<string, string>{
        {"H87", "kom."}};
    private Fantom fantom;
    public event EventHandler? InsertedUpdated;

    public InvoiceType getInvoice(){
        if(this.invoice != null){
            return invoice;
        }else{
            fetchInvoice();
            return invoice??new InvoiceType();
        }
    }

    public void fetchInvoice(){
        lock(this.fantom){
            ResponseEnvelope? response;        
            if(!fantom.isConnected){
                MessageBox.Show("server nije dostupan.");
                return;
            }
            RequestEnvelope requestEnvelop = new RequestEnvelope(
                Program.getUserName(), NetworkingConstants.ACTION_GET_INVOICE);
            response = fantom.sendRequestAsync(requestEnvelop).GetAwaiter().GetResult();
            if(response.code == NetworkingConstants.MESSAGE_CODE_OK){
                invoice = response.invoice;
            }
        }
    }


    public List<InvoiceLineType> getInserted(){
        if(this.insertedInvoiceLines != null){
            return insertedInvoiceLines;
        }else{
            fetchInserted();
            return insertedInvoiceLines??new List<InvoiceLineType>();
        }
    }

    public void fetchInserted(){
        lock(this.fantom){
            ResponseEnvelope? response;        
            if(!fantom.isConnected){
                MessageBox.Show("server nije dostupan.");
                return;
            }
            RequestEnvelope requestEnvelop = new RequestEnvelope(
                Program.getUserName(), NetworkingConstants.ACTION_GET_INSERTED);
            response = fantom.sendRequestAsync(requestEnvelop).GetAwaiter().GetResult();
            if(response.code == NetworkingConstants.MESSAGE_CODE_OK){
                insertedInvoiceLines = response.inserted;
            }
        }

    }

    public void setInvoice(InvoiceType invoice){
        return;
    }

    public void setInserted(List<InvoiceLineType> inserted){
        return;
    }

    public invoiceOrchestratorClient(Fantom fantom){
        this.fantom = fantom;
    }

    public String getUnitOfMeasurementFromCode(String code){
        if(this.unitOfMeasurementCodeLookup.ContainsKey(code)){
            return this.unitOfMeasurementCodeLookup[code];
        }else{
            return code;
        }
    }
    public void insertInvoiceLine(InvoiceLineType invoiceLine){
        lock(this.fantom){
            if(!fantom.isConnected){
                MessageBox.Show("bezuspešno slanje unetog artikla, server nije dostupan.");
                return;
            }
            RequestEnvelope requestEnvelop = new RequestEnvelope(
                Program.getUserName(), NetworkingConstants.ACTION_INSERT, invoiceLine);
            _ = fantom.sendRequestAsync(requestEnvelop);
            InsertedUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
    public void editInsertedInvoiceLine(InvoiceLineType invoiceLine){
        lock(this.fantom){
            if(!fantom.isConnected){
                MessageBox.Show("bezuspešno slanje izmenjenog artikla, server nije dostupan.");
                return;
            }
            RequestEnvelope requestEnvelop = new RequestEnvelope(
                Program.getUserName(), NetworkingConstants.ACTION_EDIT, invoiceLine);
            _ = fantom.sendRequestAsync(requestEnvelop).GetAwaiter().GetResult();
            InsertedUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
    public void removeInsertedInvoiceLine(String id){
        lock(this.fantom){
            if(!fantom.isConnected){
                MessageBox.Show("bezuspešno slanje uklonjenih artikla, server nije dostupan.");
                return;
            }
            RequestEnvelope requestEnvelop = new RequestEnvelope(
                Program.getUserName(), NetworkingConstants.ACTION_REMOVE, new List<string>{id});
            _ = fantom.sendRequestAsync(requestEnvelop).GetAwaiter().GetResult();
            InsertedUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
    public void updateRemainingInvoiceLines(){

    }
    
    public decimal getInsertedQuantityForCode(String code){
        ResponseEnvelope? response;
        lock(this.fantom){
            if(!fantom.isConnected){
                MessageBox.Show("server nije dostupan.");
                return 0;
            }
            RequestEnvelope requestEnvelop = new RequestEnvelope(
                Program.getUserName(), NetworkingConstants.ACTION_GET_QUANTITIES, new List<string>{code});
            response = fantom.sendRequestAsync(requestEnvelop).GetAwaiter().GetResult();
        }

        return response?.quantities?.Where(q => q.code == code)?.First()?.insertedQuantity ?? 0;
    }
    public decimal getOriginalQuantityForCode(String code){
        ResponseEnvelope? response;
        lock(this.fantom){
            if(!fantom.isConnected){
                MessageBox.Show("server nije dostupan.");
                return 0;
            }
            RequestEnvelope requestEnvelop = new RequestEnvelope(
                Program.getUserName(), NetworkingConstants.ACTION_GET_QUANTITIES, new List<string>{code});
            response = fantom.sendRequestAsync(requestEnvelop).GetAwaiter().GetResult();
        }

        return response?.quantities?.Where(q => q.code == code)?.First()?.originalQuantity ?? 0;
    }
    public decimal getRemainingQuantityForCode(String code){
        ResponseEnvelope? response;
        lock(this.fantom){
            if(!fantom.isConnected){
                MessageBox.Show("server nije dostupan.");
                return 0;
            }
            RequestEnvelope requestEnvelop = new RequestEnvelope(
                Program.getUserName(), NetworkingConstants.ACTION_GET_QUANTITIES, new List<string>{code});
            response = fantom.sendRequestAsync(requestEnvelop).GetAwaiter().GetResult();
        }

        return (response?.quantities?.Where(q => q.code == code)?.First()?.originalQuantity
         - response?.quantities?.Where(q => q.code == code)?.First()?.insertedQuantity )?? 0;
    }

    public List<decimal> getRemainingQuantityForCodes(List<String> codes){
        ResponseEnvelope? response;
        lock(this.fantom){
            if(!fantom.isConnected){
                MessageBox.Show("server nije dostupan.");
                return new List<decimal>{};
            }
            RequestEnvelope requestEnvelop = new RequestEnvelope(
                Program.getUserName(), NetworkingConstants.ACTION_GET_QUANTITIES, codes);
            response = fantom.sendRequestAsync(requestEnvelop).GetAwaiter().GetResult();
        }
        List<decimal> quantities = new List<decimal>();
        QuantityTypeSerializer defaultQuantityType = new QuantityTypeSerializer();
        foreach(String code in codes){
            decimal quantity = response?.quantities?.Where(q => q.code == code)?.FirstOrDefault()?.originalQuantity
                    - response?.quantities?.Where(q => q.code == code)?.FirstOrDefault()?.insertedQuantity ?? 0;
            quantity *= -1;
            quantities.Add(quantity);
        }
        return quantities;
    }
}