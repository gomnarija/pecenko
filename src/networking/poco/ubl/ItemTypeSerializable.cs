using UblSharp.CommonAggregateComponents;
using UblSharp.UnqualifiedDataTypes;

[Serializable]
public class ItemTypeSerializable {

    public NameType? name;
    public ItemIdentificationType? sellersItemIdentification;

    public ItemTypeSerializable(){

    }

    public ItemTypeSerializable(ItemType itemType){
        this.name = itemType.Name;
        this.sellersItemIdentification = itemType.SellersItemIdentification;
    }


    public ItemType toTimeType(){
        ItemType itemType = new ItemType();
        itemType.Name = name;
        itemType.SellersItemIdentification = sellersItemIdentification;
        return itemType;
    }
}