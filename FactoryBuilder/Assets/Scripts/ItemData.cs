using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "FactoryStuff/ItemData")]
public class ItemData : ScriptableObject
{
    [Tooltip("Code breaks if the gameObject doesn't have a WorldItem script")]
    public GameObject worldItem;
}
