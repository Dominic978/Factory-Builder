
public class Crafter : Storage
{
    public Recipe targetRecipe;

    public override bool AddItem(ItemData itemData, int amount = 1)
    {
        //will always be added as im making this a very simple storage script
        if (!storedItems.TryAdd(itemData, amount))
        {
            storedItems[itemData] += amount;//if we got it increase value
        }

        Craft();//tries to craft

        return true;
    }

    public void Craft()
    {
        if(CanCraft())
        {
            foreach (var itemAndAmount in targetRecipe.requiredItemAmounts)
            {
                storedItems[itemAndAmount.itemData] -= itemAndAmount.amount;
            }
            AddItem(targetRecipe.output.itemData, targetRecipe.output.amount);
        }
    }

    public bool CanCraft()
    {
        foreach (var itemAndAmount in targetRecipe.requiredItemAmounts)
        {
            if(!storedItems.TryGetValue(itemAndAmount.itemData, out int amount) || amount < itemAndAmount.amount)
                return false;
        }
        return true;
    }
}
