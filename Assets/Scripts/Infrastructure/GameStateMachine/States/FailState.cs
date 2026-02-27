public class FailState : IState
{
  private readonly Owner[] _owner;
  private readonly Player _player;
  private readonly DynamicJoystick _dynamicJoystick;
  private readonly UI _ui;
  private readonly OwnerStateMachine[] _ownerStateMachine;

  public FailState(Owner[] owner, Player player, DynamicJoystick dynamicJoystick, UI ui,
    OwnerStateMachine[] ownerStateMachine)
  {
    _ownerStateMachine = ownerStateMachine;
    _owner = owner;
    _ui = ui;
    _player = player;
    _dynamicJoystick = dynamicJoystick;
  }

  public void Enter()
  {
    _dynamicJoystick.gameObject.SetActive(false);
    _player.TurnOffStealth();
    _player.DeactivateStealth();
    _player.PlayerMover.SetScaredAnimation();

    for (int i = 0; i < _owner.Length; i++)
    {
      _owner[i].OwnerAnimation.SetCallAnimation();
      _ownerStateMachine[i].enabled = false;
      _ownerStateMachine[i].gameObject.SetActive(false);
    }

    _ui.GameHUD.Hide();
    _ui.FailHud.Show();
    _ui.FailHud.ChangeCanvasAlphaValue();
  }

  public void Exit()
  {
  }

  public void Update()
  {
  }
}