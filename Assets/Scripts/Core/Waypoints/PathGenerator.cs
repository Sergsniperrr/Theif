using UnityEngine;

public class PathGenerator : MonoBehaviour
{
  private Waypoint[] _path;
  private Randomizer _randomizer;

  private bool _isPlayer;
  
  private void Awake()
  {
    _path = GetComponentsInChildren<Waypoint>();
    _randomizer = new Randomizer(_path.Length);
  }

  public Transform GetPoint()
  {
    return _path[_randomizer.Select(true)].transform;
  }
}