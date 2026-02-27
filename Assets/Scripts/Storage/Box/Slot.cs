using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Slot : MonoBehaviour
{
  [SerializeField] private Image _icon;
  [SerializeField] private string _saveKeyForSlot;
  [SerializeField] private ItemFactory _itemFactory;
  [SerializeField] private SellButton _sellButton;

  private Button _button;
  private Item _item;
  private int _indexItem;
  private Player _player;
  private bool _isBusy = false;

  public bool IsBusy => _isBusy;

  public event Action<int> ItemWasPutDown;

  [Inject]
  private void Constructor(Player player) => _player = player;

  private void Awake()
  {
    _sellButton.ItemSold += RemoveItem;
    _button = GetComponent<Button>();
  }

  private void OnEnable()
  {
    _button.onClick.AddListener(JumpToPlayer);
  }

  private void OnDisable()
  {
    _button.onClick.RemoveListener(JumpToPlayer);
  }

  private void Start()
  {
    if (ES3.KeyExists(_saveKeyForSlot, SaveProgress.FilePath.Slots))
    {
      _indexItem = ES3.Load(_saveKeyForSlot, SaveProgress.FilePath.Slots, _indexItem);
      InstantiateSaveItem();
    }
  }

  private void RemoveItem(int item)
  {
    ES3.DeleteKey(_saveKeyForSlot, SaveProgress.FilePath.Slots);
    print("отработало");
  }

  private void InstantiateSaveItem()
  {
    Item itemPrefab = _itemFactory.GetItem(_indexItem);
    Item item = Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
    _item = item;
    _icon.sprite = _item.Icon;
    _icon.gameObject.SetActive(true);
    _item.gameObject.SetActive(false);
  }

  public void TakeItem(Item item)
  {
    _isBusy = true;
    ItemWasPutDown?.Invoke(item.Index);
    _item = item;
    _indexItem = item.Index;
    ES3.Save(_saveKeyForSlot, _indexItem, SaveProgress.FilePath.Slots);
    _icon.gameObject.SetActive(true);
    _icon.sprite = item.Icon;
  }

  private void JumpToPlayer()
  {
    if (_item != null)
    {
      _item.gameObject.SetActive(true);
      _item.DeactivateItem();
      int numJumps = 1;
      float duration = 0.3f;
      float jumpPower = 2;

      _item.transform.DOJump(_player.transform.position, jumpPower, numJumps, duration).OnComplete((() =>
      {
        _item.gameObject.SetActive(false);
        _item.transform.SetParent(_player.transform);
        _player.AddItem(_item);
        _indexItem = 0;
        _item = null;
        _isBusy = false;
        _icon.gameObject.SetActive(false);
      }));
    }
  }
}