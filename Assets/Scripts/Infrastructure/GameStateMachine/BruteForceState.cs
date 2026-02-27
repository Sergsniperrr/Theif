using UnityEngine;

public class BruteForceState : IState
{
  private readonly UI _ui;
  
  public BruteForceState(UI ui)
  {
    _ui = ui;
  }
  
  public void Enter()
  {
    _ui.GameHUD.Hide();
    Time.timeScale = 0;
  }

  public void Exit()
  {
    Time.timeScale = 1;
  }

  public void Update()
  {
  }
}