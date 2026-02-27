using System.Collections.Generic;
using UnityEngine;
using System;

public class OwnerStateMachine : StateMachine
{
  [SerializeField] private Owner _owner;

  private void OnDisable()
  {
    _owner.PlayerDetector.HeardPlayer -= SetPanicState;
    _owner.OwnerMover.CameToLastPositionPlayer -= SetPatrolState;
  }
  
  protected override void InitStates()
  {
    StatesMap = new Dictionary<Type, IState>()
    {
      [typeof(PatrolState)] = new PatrolState(_owner),
      [typeof(PanicState)] = new PanicState(_owner),
    };
  }

  protected override void SetStateByDefault() => SetPatrolState();

  private void SetPatrolState()
  {
    _owner.PlayerDetector.HeardPlayer += SetPanicState;
    _owner.OwnerMover.CameToLastPositionPlayer -= SetPatrolState;
    var state = GetState<PatrolState>();
    SetState(state);
  }

  private void SetPanicState()
  {
    _owner.PlayerDetector.HeardPlayer -= SetPanicState;
    _owner.OwnerMover.CameToLastPositionPlayer += SetPatrolState;
    var state = GetState<PanicState>();
    SetState(state);
  }
}