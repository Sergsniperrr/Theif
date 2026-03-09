using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ListData<T>
{
    public List<T> Items;
    
    public ListData(List<T> items)
    {
        Items = items;
    }
}
