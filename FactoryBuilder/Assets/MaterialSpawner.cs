
public class MaterialSpawner : Storage
{
    public ItemData itemToGive;
    public override ItemData RemoveItem() => itemToGive;
    public override bool RemoveItem(ItemData target) => target == itemToGive;

    //It gives items not takes them
    public override void AddItem(WorldItem worldItem) { }
    public override bool AddItem(ItemData worldItem) => false;
}
