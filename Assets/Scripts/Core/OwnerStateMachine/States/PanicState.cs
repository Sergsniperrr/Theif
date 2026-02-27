public class PanicState : IState
{
  private Owner _owner;

  public PanicState(Owner owner)
  {
    _owner = owner;
  }

  public void Enter()
  {
    _owner.OwnerMover.MoveToPoint(_owner.PlayerDetector.PlayerLastPoint);
    _owner.OwnerMover.SetMovementSpeed(3f);
  }

  public void Exit()
  {
    _owner.OwnerMover.StopMoveToPoint();
    _owner.OwnerMover.SetMovementSpeed(1.8f);
  }

  public void Update()
  {
  }
}