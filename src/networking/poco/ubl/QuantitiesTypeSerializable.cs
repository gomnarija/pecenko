[Serializable]
public class QuantitiesTypeSerializable {
    public string code = "N/a";
    public decimal originalQuantity;
    public decimal insertedQuantity;

    public QuantitiesTypeSerializable(string code, decimal originalQuantity, decimal insertedQuantity){
        this.code = code;
        this.originalQuantity = originalQuantity;
        this.insertedQuantity = insertedQuantity;
    }

    public QuantitiesTypeSerializable(){
        
    }
}