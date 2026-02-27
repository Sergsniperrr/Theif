using System.Collections.Generic;
using UnityEngine;

public class ContainerHandler : MonoBehaviour
{
  [SerializeField] private List<Container> _spawnedContainers = new List<Container>();

  public IReadOnlyList<Container> SpawnedContainers => _spawnedContainers;

  public void SetTypeContainers()
  {
    for (int i = 0; i < _spawnedContainers.Count; i++)
    {
        _spawnedContainers[i].SetContainerType(ContainerType.Point);
    }
  }
}

public enum ContainerType
{
  Point = 0,
  Level_1 = 1,
  Level_2 = 2,
  Level_3 = 3,
  GoldKey = 4
}