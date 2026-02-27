using System;
using UnityEngine;
using Zenject;

public class FieldOfViewTarget : MonoBehaviour
{
  private Player _player;
  private Vector3 _playerLastPosition;

  [Inject]
  private void Constructor(Player player)
  {
    _player = player;
  }

  private void Update()
  {
    if (_playerLastPosition != _player.transform.position)
    {
      _playerLastPosition = _player.transform.position;
      transform.position = _playerLastPosition;
    }
  }
}