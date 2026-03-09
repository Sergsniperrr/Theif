using System.Collections.Generic;
using DG.Tweening;
using MirraGames.SDK;
using UnityEngine;
using Random = UnityEngine.Random;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Slot[] _slots;
    [SerializeField] private SellButton _sellButton;

    private List<int> _itemsIndex = new List<int>();
    private List<Item> _items = new List<Item>();
    private ItemFactory _itemFactory;

    private void Awake()
    {
        _itemFactory = GetComponentInChildren<ItemFactory>();

        //ListData<int> itemsInCar = MirraSDK.Data.GetObject<ListData<int>>(SavableKeys.ItemsInCar);
        ListData<int> playerItems = MirraSDK.Data.GetObject<ListData<int>>(SavableKeys.PlayerItems);

        //_itemsIndex = itemsInCar.Items;
        //_itemsIndex.AddRange(playerItems.Items);

        _itemsIndex = playerItems.Items;
        
        InstantiateItems();
    }

    private void OnEnable()
    {
        _sellButton.ItemSold += RemoveItem;

        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].ItemWasPutDown += RemoveItem;
        }
    }

    private void OnDisable()
    {
        _sellButton.ItemSold -= RemoveItem;

        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].ItemWasPutDown -= RemoveItem;
        }
    }

    private void RemoveItem(int item)
    {
        _itemsIndex.Remove(item);

        MirraSDK.Data.SetObject(SavableKeys.PlayerItems, new ListData<int>(_itemsIndex));
        //ES3.Save(SaveProgress.TitleKey.Items, _itemsIndex, SaveProgress.FilePath.Items);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Player player))
        {
            for (int i = 0; i < _itemsIndex.Count; i++)
            {
                JumpToPlayer(_items[i], player);
                player.AddItem(_items[i]);
            }

            _itemsIndex.Clear();
        }
    }

    private void JumpToPlayer(Item item, Player player)
    {
        int numJumps = 1;
        float duration = 0.3f;
        float jumpPower = 2;

        item.transform.DOJump(player.transform.position, jumpPower, numJumps, duration).OnComplete((() =>
        {
            item.gameObject.SetActive(false);
            item.transform.SetParent(player.transform);
        }));
    }

    private void InstantiateItems()
    {
        if (_itemsIndex == null || _itemsIndex.Count == 0)
            return;

        for (int i = 0; i < _itemsIndex.Count; i++)
        {
            int randomPoint = Random.Range(0, _spawnPoints.Length);
            Item item = _itemFactory.GetItem(_itemsIndex[i]);
            Item newItem = Instantiate(item, _spawnPoints[randomPoint].transform.position, Quaternion.identity,
                transform);
            _items.Add(newItem);
            newItem.DeactivateItem();
        }
    }
}