using UnityEngine;
using UnityEngine.SceneManagement;

public class AnalyticHelper : MonoBehaviour
{
  [SerializeField] private Data _data;
  [SerializeField] private AnalyticManager _analytic;

  private void Awake()
  {
    _data.Load();
    _data.SetLevelIndex(SceneManager.GetActiveScene().buildIndex);
    _data.Save();
  }
  
  private void Start()
  {
    int currentLevel = SceneManager.GetActiveScene().buildIndex;
    ES3.Save(SaveProgress.TitleKey.LastScene, currentLevel, SaveProgress.FilePath.LastScene);
    
    _analytic.SendEventOnLevelStart(SceneManager.GetActiveScene().buildIndex);
  }

  private void OnApplicationPause(bool pauseStatus)
  {
    if (pauseStatus)
    {
      _analytic.SendEventOnGameExit(_data.GetRegistrationDate(), _data.GetSessionCount(), _data.GetNumberDaysAfterRegistration(), GetCurrentBitcoin());
      _data.Save();
    }
  }

  private void OnApplicationQuit()
  {
    _analytic.SendEventOnGameExit(_data.GetRegistrationDate(), _data.GetSessionCount(), _data.GetNumberDaysAfterRegistration(), GetCurrentBitcoin());
    _data.Save();
  }

  private float GetCurrentBitcoin()
  {
    float currentBitcoin = 0;

    currentBitcoin = ES3.Load(SaveProgress.TitleKey.Money, SaveProgress.FilePath.Money, currentBitcoin);
    return currentBitcoin;
  }
}
