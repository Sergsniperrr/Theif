using UnityEngine;

public class BoxAnimator : MonoBehaviour
{
  private const string IsOpen = "IsOpen";

  private Animator _boxAnimator;

  private void Awake()
  {
    _boxAnimator = GetComponentInChildren<Animator>();
  }

  public void ChangeDoorState(bool state) => _boxAnimator.SetBool(IsOpen, state);
}