
public class MaterialDump : Storage
{
    public override void AddItem(WorldItem worldItem)
    {
        Destroy(worldItem.gameObject);
    }

    public override bool AddItem(ItemData worldItem, int amount = 1) => true;

    //It takes items not gives them
    public override ItemData RemoveItem() => null;
    public override bool RemoveItem(ItemData target, int amount = 1) => false;
}
