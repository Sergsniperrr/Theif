using System;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
  private void OnTriggerExit(Collider collider)
  {
    if (collider.gameObject.TryGetComponent(out Player player))
    {
      player.EnabledStealth();
    }
  }
}
