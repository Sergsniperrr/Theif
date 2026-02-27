using BuildingBlocks.DataTypes;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] private InspectableDictionary<int, Item> _items = new InspectableDictionary<int, Item>();

    public Item GetItem(int key)
    {
        return _items[key];
    }
}
