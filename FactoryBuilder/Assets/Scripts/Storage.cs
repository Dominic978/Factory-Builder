using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storage : Building
{
    //Key = item stored, Value = amount of that Item
    public Dictionary<ItemData, int> storedItems = new Dictionary<ItemData, int>();

    public bool HasItem(ItemData item) => storedItems.ContainsKey(item);

    //Im making the storage building handle the outputs, so first I could have it only output to 1 conveyerbelt at a time and to switch between lanes
    private ConveyerBelt[] outputConveyerBelts = new ConveyerBelt[0];

    public void AddConveyerBelt(ConveyerBelt belt)
    {
        foreach (var conveyerBelt in outputConveyerBelts)
        {
            if (conveyerBelt == belt)
                return; // it is already in the output array
        }

        ConveyerBelt[] biggerArr = new ConveyerBelt[outputConveyerBelts.Length + 1];

        for (int i = 0; i < outputConveyerBelts.Length; i++)
        {
            biggerArr[i] = outputConveyerBelts[i];
        }
        biggerArr[outputConveyerBelts.Length] = belt;
        outputConveyerBelts = biggerArr;
    }

    public override void UpdateBuilding()
    {
        //UpdateBuilding will also be called when a building is destroyed so im just having this here for when I add that
        outputConveyerBelts = outputConveyerBelts.Where(belt => belt != null).ToArray();
    }

    private const float timeBetweenOutputs = 1.5f;
    private float time;

    private int currentIndexOfOutput = 0;

    private void Update()
    {
        if (outputConveyerBelts.Length == 0)
            return;

        time += Time.deltaTime;
        if(time >= timeBetweenOutputs)
        {
            time -= timeBetweenOutputs;
            ItemData itemData = RemoveItem();
            if(itemData != null )
            {
                int num = currentIndexOfOutput % 2;
                ConveyerBelt belt = outputConveyerBelts[(currentIndexOfOutput - num) / 2];
                //give the object a position under the map so we can't see it when it spawns
                if (num == 0 && belt.conveyerChain[2].item == null)
                    belt.conveyerChain[2].item = Instantiate(itemData.worldItem, new Vector3(0,-100,0), Quaternion.identity).GetComponent<WorldItem>();
                else if (num == 1 && belt.conveyerChain[0].item == null)
                    belt.conveyerChain[0].item = Instantiate(itemData.worldItem, new Vector3(0, -100, 0), Quaternion.identity).GetComponent<WorldItem>();
            }
            currentIndexOfOutput = (currentIndexOfOutput + 1) % (outputConveyerBelts.Length * 2);
        }
    }

    public SerializableHashSet<ItemData> inputFilter = new SerializableHashSet<ItemData>();
    public SerializableHashSet<ItemData> outputFilter = new SerializableHashSet<ItemData>();

    /// <summary>
    /// Do not fret about removing the world item as this method will remove it if it can be added
    /// </summary>
    /// <param name="worldItem"></param>
    public virtual void AddItem(WorldItem worldItem)
    {
        if(AddItem(worldItem.itemData))
            Destroy(worldItem.gameObject);
    }

    public virtual bool AddItem(ItemData itemData, int amount = 1, bool careAboutFilter = true)
    {
        if (outputFilter.Count == 0)
            careAboutFilter = false;

        if (!careAboutFilter || inputFilter.Contains(itemData))
        {
            //will always be added as im making this a very simple storage script
            if (storedItems.TryAdd(itemData, amount))
                UpdateRandomItemsToGrab();
            else
                storedItems[itemData] += amount;//if we got it increase value
            return true;
        }
        return false;
    }

    //random items to randomly remove for the "ItemData RemoveItem()" function
    protected ItemData[] randomItemsToGrab = new ItemData[0];

    /// <summary>
    /// removes random item from inventory if ItemData and decreases count if return is not null
    /// </summary>
    /// <returns></returns>
    public virtual ItemData RemoveItem()
    {
        if (randomItemsToGrab.Length != 0)
        {
            ItemData item = randomItemsToGrab[Random.Range(0, randomItemsToGrab.Length)];
            if (RemoveItem(item))
                return item;
        }
        return null;
    }

    /// <summary>
    /// </summary>
    /// <param name="target"></param>
    /// <returns>if we succesfully removed the target item</returns>
    public virtual bool RemoveItem(ItemData target, int amount = 1, bool careAboutFilter = true)
    {
        if (outputFilter.Count == 0)
            careAboutFilter = false;

        if ((!careAboutFilter || outputFilter.Contains(target)) && storedItems.TryGetValue(target, out int itemAmount) && itemAmount >= amount)
        {
            if (itemAmount == amount)
            {
                storedItems.Remove(target);
                UpdateRandomItemsToGrab();
            }
            else
                storedItems[target] -= amount;
            return true;
        }
        return false;
    }

    protected void UpdateRandomItemsToGrab()
    {
        ItemData[] storedItemsKeyArr = storedItems.Keys.ToArray();
        if (outputFilter.Count != 0)
        {
            List<int> removeIndex = new List<int>();
            for (int i = 0; i < storedItemsKeyArr.Length; i++)
            {
                if (!outputFilter.Contains(storedItemsKeyArr[i]))
                    removeIndex.Add(i);
            }

            randomItemsToGrab = new ItemData[storedItemsKeyArr.Length - removeIndex.Count];
            int index = 0;
            for (int i = 0; i < storedItemsKeyArr.Length; i++)
            {
                if (!removeIndex.Contains(i))
                {
                    randomItemsToGrab[index++] = storedItemsKeyArr[i];
                }
            }
        }
    }
}
