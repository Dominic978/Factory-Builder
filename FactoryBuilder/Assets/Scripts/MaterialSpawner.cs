
public class MaterialSpawner : Storage
{
    public ItemData itemToGive;
    public override ItemData RemoveItem() => itemToGive;
    public override bool RemoveItem(ItemData target, int amount = 1, bool careAboutFilter = true) => target == itemToGive;

    //It gives items not takes them
    public override void AddItem(WorldItem worldItem) { }
    public override bool AddItem(ItemData worldItem, int amount = 1, bool careAboutFilter = true) => false;
}
