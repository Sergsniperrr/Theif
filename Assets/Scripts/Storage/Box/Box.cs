using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Box : MonoBehaviour
{
  [SerializeField] private BoxUI _boxUI;
  [SerializeField] private List<Slot> _slots = new List<Slot>();
  [SerializeField] private bool _isOpen;

  private int _size = 5;
  private BoxAnimator _boxAnimator;

  private void Start()
  {
    _boxAnimator = GetComponent<BoxAnimator>();
    _boxAnimator.ChangeDoorState(_isOpen);
  }

  private void OnTriggerEnter(Collider collider)
  {
    if (collider.TryGetComponent(out Player player))
    {
      ChangeBoxState(true);
      _boxUI.ActivateButton();

      if (player.Items.Count != 0)
        AddItems(player);
    }
  }

  private void OnTriggerExit(Collider collider)
  {
    if (collider.TryGetComponent(out Player player))
    {
      ChangeBoxState(false);
      _boxUI.DeactivateStorage();
      _boxUI.DeactivateButton();
    }
  }

  private void AddItems(Player player)
  {
    List<Item> itemsForRemove = new List<Item>();

    for (int i = 0; i < player.Items.Count; i++)
    {
      Slot slot = SearchEmptyCell();

      if (slot != null)
      {
        slot.TakeItem(player.Items[i]);
        JumpToBox(player.Items[i]);
        itemsForRemove.Add(player.Items[i]);
      }
    }

    for (int i = 0; i < itemsForRemove.Count; i++)
    {
      player.DeleteItem(itemsForRemove[i]);
    }
  }

  private Slot SearchEmptyCell()
  {
    for (int i = 0; i < _slots.Count; i++)
    {
      if (_slots[i].IsBusy == false)
        return _slots[i];
    }

    return null;
  }

  private void JumpToBox(Item item)
  {
    int numJumps = 1;
    float duration = 1f;
    float jumpPower = 2;
    item.gameObject.SetActive(true);

    item.transform.DOJump(transform.position, jumpPower, numJumps, duration).OnComplete((() =>
    {
      item.gameObject.SetActive(false);
      item.transform.SetParent(transform);
    }));
  }

  private void ChangeBoxState(bool state)
  {
    _isOpen = state;
    _boxAnimator.ChangeDoorState(_isOpen);
  }
}