using UnityEngine;

public class UI : MonoBehaviour
{
  [field: SerializeField] public GameHud GameHUD { get; private set; }
  [field: SerializeField] public FailHud FailHud { get; private set; }
}