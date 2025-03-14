namespace pečenko;
using UblSharp;
using UblSharp.CommonAggregateComponents;

class InvoiceOrchestratorImpl : IInvoiceOrchestrator {
    
    private InvoiceType invoice = new InvoiceType();
    private List<InvoiceLineType> insertedInvoiceLines = new List<InvoiceLineType>();
    private Dictionary<String, decimal> remainingQuantities = new Dictionary<string, decimal>();
    private Dictionary<String, String> unitOfMeasurementCodeLookup = new Dictionary<string, string>{
        {"H87", "kom."}};
    private InvoiceFilesOrchestrator invoiceFilesOrchestrator= new InvoiceFilesOrchestrator();
    public event EventHandler? Updated;

    public event EventHandler? InsertedUpdated;
    public InvoiceType getInvoice(){
        return invoice;
    }
    public List<InvoiceLineType> getInserted(){
        return insertedInvoiceLines;
    }

    public void setInvoice(InvoiceType invoice){
        this.invoice = invoice;
        updateRemainingInvoiceLines();
    }

    public void setInserted(List<InvoiceLineType> inserted){
        this.insertedInvoiceLines = inserted;
        updateRemainingInvoiceLines();
        InsertedUpdated?.Invoke(this, EventArgs.Empty);
    }

    public String getUnitOfMeasurementFromCode(String code){
        if(this.unitOfMeasurementCodeLookup.ContainsKey(code)){
            return this.unitOfMeasurementCodeLookup[code];
        }else{
            return code;
        }
    }

    public String getAutoSaveFileName(){
        if((this?.invoice?.ID?.Value??"") == ""){
            return "default.xml";
        }
        //clear id
        String id = this.invoice.ID.Value;
        id = id.Replace('/', '_')
                .Replace('\\', '_')
                .Replace(' ', '_');
        return  Path.Combine(ProgramConstans.APP_DATA_DIR_PATH, id + ".xml");
    }



    private String findInsertedInvoiceLinesAvaliableID(){
        int ID = 1;
        foreach(InvoiceLineType invoiceLine in insertedInvoiceLines){
            if(int.Parse(invoiceLine.ID) >= ID){
                ID = int.Parse(invoiceLine.ID)+1;
            }
        }

        return ID.ToString();
    }

    public void insertInvoiceLine(InvoiceLineType invoiceLine){
        if(!long.TryParse(invoiceLine?.Item?.SellersItemIdentification?.ID.Value, out _)){
            MessageBox.Show("ne-numerički barkod nije podržan");
            return;
        } // numeric id is required

        //if it already exists, add to the total sum
        foreach(InvoiceLineType inserted in this.insertedInvoiceLines){
            String code = invoiceLine.Item.SellersItemIdentification.ID.Value;
            if(code.Equals(inserted.Item?.SellersItemIdentification?.ID?.Value??"N/a")){
                if(inserted.InvoicedQuantity != null && invoiceLine.InvoicedQuantity != null)
                    inserted.InvoicedQuantity.Value += invoiceLine.InvoicedQuantity.Value;
                this.updateRemainingInvoiceLines();
                Updated?.Invoke(this, EventArgs.Empty);
                InsertedUpdated?.Invoke(this, EventArgs.Empty);
                return;
            }
        }
        //if it doesn't add new one
        invoiceLine.ID = this.findInsertedInvoiceLinesAvaliableID();
        insertedInvoiceLines.Add(invoiceLine);
        this.updateRemainingInvoiceLines();
        Updated?.Invoke(this, EventArgs.Empty);
        InsertedUpdated?.Invoke(this, EventArgs.Empty);
    }

    public void insertInvoiceLine(List<InvoiceLineType> invoiceLines){
        foreach(InvoiceLineType invoiceLine in invoiceLines){
            insertInvoiceLine(invoiceLine);
        }
    }

    public void editInsertedInvoiceLine(InvoiceLineType invoiceLine){
        if(!long.TryParse(invoiceLine?.Item?.SellersItemIdentification?.ID, out _)) // numeric id is required
            return;

        int index = this.insertedInvoiceLines.FindIndex(i => i.ID.Value == invoiceLine.ID.Value);
        if(index != -1){
            this.insertedInvoiceLines[index] = invoiceLine;
            this.updateRemainingInvoiceLines();
            Updated?.Invoke(this, EventArgs.Empty);
            InsertedUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

    public void editInsertedInvoiceLine(List<InvoiceLineType> invoiceLines){
        foreach(InvoiceLineType invoiceLine in invoiceLines){
            editInsertedInvoiceLine(invoiceLine);
        }
    }

    public void removeInsertedInvoiceLine(String id){
        if(!long.TryParse(id, out _)){ //numeric id is required
            return;
        }
        for(int i = 0;i<this.insertedInvoiceLines.Count;i++){
            if(this.insertedInvoiceLines[i].ID.Value.Equals(id) && id != ""){
                this.insertedInvoiceLines.RemoveAt(i);
                updateRemainingInvoiceLines();
                Updated?.Invoke(this, EventArgs.Empty);
                InsertedUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void removeInsertedInvoiceLine(List<String> ids){
        foreach(String id in ids){
            removeInsertedInvoiceLine(id);
        }
    }

    public void updateRemainingInvoiceLines(){
        foreach(InvoiceLineType invoiceLine in this.invoice.InvoiceLine??new List<InvoiceLineType>()){
            //count quantity
            string code = invoiceLine.Item?.SellersItemIdentification?.ID?.Value ?? "N/a";
            if(code == "N/a") continue;
            decimal originalQuantity = this.getOriginalQuantityForCode(code);
            decimal insertedQuantity = this.getInsertedQuantityForCode(code);
            decimal remainingQuantity = originalQuantity - insertedQuantity;
            remainingQuantity *= -1;
            this.remainingQuantities[code] = remainingQuantity;            
        }
    }


    public decimal getInsertedQuantityForCode(String code){
        decimal quantity = 0;
        foreach(InvoiceLineType invoiceLine in this.insertedInvoiceLines){
            if(code.Equals(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"N/a")){
                quantity += invoiceLine.InvoicedQuantity?.Value ?? 0;
            }
        }
        return quantity;
    }

    public decimal getOriginalQuantityForCode(String code){
        foreach(InvoiceLineType invoiceLine in this.invoice.InvoiceLine??new List<InvoiceLineType>()){
            if(code.Equals(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"N/a")){
                return invoiceLine.InvoicedQuantity?.Value ?? 0;
            }
        }

        return 0;
    }

    public decimal getRemainingQuantityForCode(String code){
        if(this.remainingQuantities.ContainsKey(code)){
            return this.remainingQuantities[code];
        }
        return 0;
    }

    public List<decimal> getRemainingQuantityForCodes(List<String> codes){
        List<decimal> quantities = new List<decimal>();
        foreach(String code in codes){
            quantities.Add(getRemainingQuantityForCode(code));
        }
        return quantities;
    }
}