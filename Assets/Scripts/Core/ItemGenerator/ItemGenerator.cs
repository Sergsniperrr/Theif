using System;
using System.Collections.Generic;
using BuildingBlocks.DataTypes;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ObjectRandomizer))]
public class ItemGenerator : MonoBehaviour
{
  [SerializeField] private ContainerHandler _containerHandler;
  [Range(1, 20)] [SerializeField] private int _capacityItems;
  [SerializeField] private List<Point> _potentialPoints = new List<Point>();
  [SerializeField] InspectableDictionary<string, ObjectRandomizer> _containers = new InspectableDictionary<string, ObjectRandomizer>();
  
  private Randomizer _randomizer;
  private DiContainer _diContainer;
  private List<Container> _spawnedContainer = new List<Container>();
  
  public IReadOnlyList<Container> SpawnedContainer => _spawnedContainer;

  public event Action ContainerGenerated;

  [Inject]
  private void Construct(DiContainer diContainer)
  {
    _diContainer = diContainer;
  }

  public void GenerateItems()
  {
    _containerHandler.SetTypeContainers();
    TryAddPotentialPoints();
    ContainerGenerated?.Invoke();
    _randomizer = new Randomizer(_potentialPoints.Count);
    
    for (int i = 0; i != _capacityItems; i++)
    {
      InstantiateItem();
    }
  }

  public GameObject GetItem(string key)
  {
    GameObject gameObject = (GameObject) _containers[key].RandomObject();
    return gameObject;
  }

  private void InstantiateItem()
  {
    Point potentialItemPoint = _potentialPoints[_randomizer.Select()];
    GameObject gameObject = (GameObject) _containers[Constant.PotentialItems].RandomObject();
    Item item = gameObject.GetComponent<Item>();

    if (potentialItemPoint.OnWall == item.OnWall)
      _diContainer.InstantiatePrefabForComponent<Item>(item, potentialItemPoint.transform);
    else
    {
      item = ReselectGameobject(potentialItemPoint, item);
      _diContainer.InstantiatePrefabForComponent<Item>(item, potentialItemPoint.transform);
    }
  }

  private Item ReselectGameobject(Point potentialItemPoint, Item item)
  {
    while (potentialItemPoint.OnWall != item.OnWall)
    {
      GameObject randomObject = (GameObject) _containers[Constant.PotentialItems].RandomObject();
      item = randomObject.GetComponent<Item>();

      if (potentialItemPoint.OnWall == item.OnWall)
        return item;
    }

    return null;
  }

  private void TryAddPotentialPoints()
  {
    for (int i = 0; i < _containerHandler.SpawnedContainers.Count; i++)
    {
      if (_containerHandler.SpawnedContainers[i].ContainerType == ContainerType.Point)
      {
        _potentialPoints.Add(_containerHandler.SpawnedContainers[i].Point);
        _containerHandler.SpawnedContainers[i].Point.gameObject.transform.SetParent(transform);
      }
      else
        _spawnedContainer.Add(_containerHandler.SpawnedContainers[i]);
    }
  }
}