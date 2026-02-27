using UnityEngine;

public class PatrolState : IState
{
  private Owner _owner;
  private Transform _currentPoint;

  public PatrolState( Owner owner)
  {
    _owner = owner;
  }

  public void Enter()
  {
    _owner.OwnerMover.MoveToPath();
  }

  public void Exit()
  {
    _owner.OwnerMover.StopMoveToPath();
  }

  public void Update()
  {
  }
}