using UblSharp;
using UblSharp.CommonAggregateComponents;

interface IInvoiceOrchestrator {
    public InvoiceType getInvoice();
    public List<InvoiceLineType> getInserted();
    public String getUnitOfMeasurementFromCode(String code);
    public void insertInvoiceLine(InvoiceLineType invoiceLine);
    public void editInsertedInvoiceLine(InvoiceLineType invoiceLine);
    public void removeInsertedInvoiceLine(String id);
    public void updateRemainingInvoiceLines();
    public decimal getInsertedQuantityForCode(String code);
    public decimal getOriginalQuantityForCode(String code);
    public decimal getRemainingQuantityForCode(String code);
    public List<decimal> getRemainingQuantityForCodes(List<String> codes);

    public event EventHandler InsertedUpdated;
}