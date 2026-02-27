using System;
using UnityEngine;

public class Container : Item
{
  [SerializeField] private BruteforcePanel _bruteforcePanel;
  [field: SerializeField] public Point Point { get; private set; }
  
  private ContainerType _containerType;

  public ContainerType ContainerType => _containerType;
  
  public event Action ButtonPressed;

  public void SetContainerType(ContainerType type)
  {
    _containerType = type;

    if (ContainerType == ContainerType.Point)
    {
      InteractButton.gameObject.SetActive(false);
      enabled = false;
      ItemCollider.enabled = false;
    }
  }
  
  protected override void OnButtonClick()
  {
    ButtonPressed?.Invoke();
    base.Deactivate();
    _bruteforcePanel.Show();
  }
}

