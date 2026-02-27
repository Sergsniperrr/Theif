using System;
using System.Collections.Generic;
using FieldOfViewAsset;
using UnityEngine;

public class GameStateMachine : StateMachine
{
  [SerializeField] private UI _uI;
  [SerializeField] private ItemGenerator _itemGenerator;
  [SerializeField] private OwnerStateMachine[] _ownerStateMachine;
  [SerializeField] private Owner[] _owner;
  [SerializeField] private Player _player;
  [SerializeField] private DynamicJoystick _dynamicJoystick;
  [SerializeField] private FieldOfView[] _fieldOfView;

  private void OnEnable()
  {
    _itemGenerator.ContainerGenerated += SubscribeToContainers;

    for (int i = 0; i < _owner.Length; i++)
    {
      _owner[i].PlayerDetector.PlayerDetected += SetFailState;
    }
  }

  private void OnDisable()
  {
    _itemGenerator.ContainerGenerated -= SubscribeToContainers;

    for (int i = 0; i < _owner.Length; i++)
    {
      _fieldOfView[i].TargetDetected -= SetFailState;
      _owner[i].PlayerDetector.PlayerDetected += SetFailState;
    }
    
    for (int i = 0; i < _itemGenerator.SpawnedContainer.Count; i++)
    {
      _itemGenerator.SpawnedContainer[i].ButtonPressed -= SetBruteforceState;
    }
  }

  protected override void InitStates()
  {
    StatesMap = new Dictionary<Type, IState>()
    {
      [typeof(GameState)] = new GameState(_uI, _itemGenerator),
      [typeof(BruteForceState)] = new BruteForceState(_uI),
      [typeof(FailState)] = new FailState(_owner, _player, _dynamicJoystick, _uI, _ownerStateMachine),
    };
  }

  protected override void SetStateByDefault() => SetGameState();

  private void SetGameState()
  {
    for (int i = 0; i < _owner.Length; i++)
    {
      _fieldOfView[i].TargetDetected += SetFailState;
    }

    var state = GetState<GameState>();
    SetState(state);
  }

  private void SetFailState(GameObject target)
  {
    var state = GetState<FailState>();
    SetState(state);
  }

  private void SetBruteforceState()
  {
    var state = GetState<BruteForceState>();
    SetState(state);
  }

  private void SubscribeToContainers()
  {
    for (int i = 0; i < _itemGenerator.SpawnedContainer.Count; i++)
    {
      _itemGenerator.SpawnedContainer[i].ButtonPressed += SetBruteforceState;
      _itemGenerator.SpawnedContainer[i].GetComponentInChildren<BruteForce>().BrutforceCompleted += SetGameState;
    }
  }
}