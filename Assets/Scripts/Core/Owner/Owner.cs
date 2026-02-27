using UnityEngine;

[RequireComponent(typeof(OwnerMover))]
[RequireComponent(typeof(PlayerDetector))]
public class Owner : MonoBehaviour
{
  private Coroutine _watching;
  
  [field: SerializeField] public OwnerMover OwnerMover { get; private set; }
  [field: SerializeField] public PlayerDetector PlayerDetector { get; private set; }
  [field: SerializeField] public OwnerAnimation OwnerAnimation { get; private set; }
  
}