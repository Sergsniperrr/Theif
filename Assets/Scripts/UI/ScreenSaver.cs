using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenSaver : MonoBehaviour
{
  private void Start()
  {
    int currentLevel = SceneManager.GetActiveScene().buildIndex;
    ES3.Save(SaveProgress.TitleKey.LastScene, currentLevel, SaveProgress.FilePath.LastScene);
  }
}
