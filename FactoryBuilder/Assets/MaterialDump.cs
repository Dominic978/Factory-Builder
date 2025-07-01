
public class MaterialDump : Storage
{
    public override void AddItem(WorldItem worldItem)
    {
        Destroy(worldItem.gameObject);
    }

    public override bool AddItem(ItemData worldItem) => true;

    //It takes items not gives them
    public override ItemData RemoveItem() => null;
    public override bool RemoveItem(ItemData target) => false;
}
