using UnityEngine;

public abstract class HUD : MonoBehaviour
{
  public void Show()
  {
    gameObject.SetActive(true);
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }
} 