using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "FactoryStuff/Recipe")]
public class Recipe : ScriptableObject
{
    [System.Serializable]
    public struct ItemAmount
    {
        public ItemData itemData;
        public int amount;
    }

    public ItemAmount[] requiredItemAmounts;

    public ItemAmount output;
}
