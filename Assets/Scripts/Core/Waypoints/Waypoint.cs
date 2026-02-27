using System;
using UnityEngine;

public abstract class Waypoint : MonoBehaviour
{
  [SerializeField] private WaypointType _waypointType;

  public event Action OwnerAtPoint;

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.TryGetComponent(out Owner owner))
    {
      OwnerAtPoint?.Invoke();
    }
  }

  private enum WaypointType
  {
    Stand,
    Action,
  }
}