using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
  protected Dictionary<Type, IState> StatesMap;
  protected IState CurrentState;

  protected virtual void Awake()
  {
    InitStates();
  }

  protected virtual void Start()
  {
    SetStateByDefault();
  }
  
  protected abstract void InitStates();

  protected abstract void SetStateByDefault();

  protected void SetState(IState newState)
  {
    if (CurrentState != null)
      CurrentState.Exit();

    CurrentState = newState;
    CurrentState.Enter();
  }

  protected IState GetState<T>() where T : IState
  {
    var type = typeof(T);
    return StatesMap[type];
  }
}