using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Levels/Create new levels")]
public class Level : ScriptableObject
{
  [field: SerializeField] public string SceneName;
  [field: SerializeField] public string TitleRu;
  [field: SerializeField] public string TitleEn;
  [field: SerializeField] public string TitleTr;
  [field: SerializeField] public Sprite Icon;
  [field: SerializeField] public int Price;
}