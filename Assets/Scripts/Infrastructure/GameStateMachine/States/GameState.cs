using UnityEngine.SceneManagement;

public class GameState : IState
{
  private readonly UI _ui;
  private readonly ItemGenerator _itemGenerator;

  private bool _isFirst = false;

  public GameState(UI ui, ItemGenerator itemGenerator)
  {
    _ui = ui;
    _itemGenerator = itemGenerator;
  }

  public void Enter()
  {
    if (_isFirst == false)
    {
      _isFirst = true;
      _itemGenerator.GenerateItems();
      SetCurrentLevel();
    }

    _ui.GameHUD.Show();
  }

  public void Exit()
  {
  }

  private void SetCurrentLevel()
  {
    int currentLevel = SceneManager.GetActiveScene().buildIndex;
    ES3.Save(SaveProgress.TitleKey.LastScene, currentLevel, SaveProgress.FilePath.LastScene);
  }

  public void Update()
  {
  }
}