
public class Crafter : Storage
{
    public Recipe targetRecipe;

    public override bool AddItem(ItemData itemData, int amount = 1, bool careAboutFilter = true)
    {
        bool added = base.AddItem(itemData, amount, careAboutFilter);

        if(added)//no point in trying to craft if no more items were added
            Craft();//tries to craft

        return added;
    }

    public void Craft()
    {
        if(CanCraft())
        {
            foreach (var itemAndAmount in targetRecipe.requiredItemAmounts)
            {
                storedItems[itemAndAmount.itemData] -= itemAndAmount.amount;
            }
            AddItem(targetRecipe.output.itemData, targetRecipe.output.amount, false);
        }
    }


    public bool CanCraft()
    {
        foreach (var itemAndAmount in targetRecipe.requiredItemAmounts)
        {
            if (!storedItems.TryGetValue(itemAndAmount.itemData, out int amount) || amount < itemAndAmount.amount)
                return false;
        }
        return true;
    }
}
