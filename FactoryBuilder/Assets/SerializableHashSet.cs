using System;
using System.Collections.Generic;
using UnityEngine;

// stolen code from "https://stackoverflow.com/questions/69671076/how-can-you-add-a-serializable-hashset-to-unity-c" with a bit of alteration

[Serializable]
public class SerializableHashSet<T> : ISerializationCallbackReceiver
{
    public bool Contains(T item) => hashSet.Contains(item);

    public int Count { get { return hashSet.Count; } }

    [SerializeField]
    private List<T> values = new List<T>();
    private HashSet<T> hashSet = new HashSet<T>();

    public SerializableHashSet() {}

    public SerializableHashSet(IEnumerable<T> collection)
    { 
        hashSet = new HashSet<T>(collection);
    }

    public void OnBeforeSerialize()
    {
        var cur = new HashSet<T>(values);

        foreach (var val in hashSet)
        {
            if (!cur.Contains(val))
                values.Add(val);
        }
    }

    public void OnAfterDeserialize()
    {
        hashSet.Clear();

        foreach (var val in values)
        {
            if (val != null)
                hashSet.Add(val);
        }
    }
}
