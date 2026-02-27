using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Levels/Create new levels")]
public class Level : ScriptableObject
{
  [field: SerializeField] public string SceneName;
  [field: SerializeField] public string Title;
  [field: SerializeField] public Sprite Icon;
  [field: SerializeField] public float Price;
}