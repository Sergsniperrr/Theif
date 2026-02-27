using System;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
  public event Action HeardPlayer;
  public event Action<GameObject> PlayerDetected;

  public Vector3 PlayerLastPoint { get; private set; }

  private void OnTriggerEnter(Collider collider)
  {
    if (collider.TryGetComponent(out Player player))
    {
      PlayerDetected?.Invoke(gameObject);
      //     PlayerLastPoint = player.transform.position;
      //     HeardPlayer?.Invoke();
    }
  }
}