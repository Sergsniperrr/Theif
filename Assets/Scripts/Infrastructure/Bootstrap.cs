using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
  [SerializeField] private Data _data;
  [SerializeField] private AnalyticManager _analytic;
  [SerializeField] private bool _isRemoveDataOnStart;

  private int _lastSceneNumber = 1;
  private bool _isFirstLoad;

  private void Awake()
  {
    if (_isRemoveDataOnStart)
      _data.RemoveData();

    _lastSceneNumber = ES3.Load(SaveProgress.TitleKey.LastScene, SaveProgress.FilePath.LastScene, _lastSceneNumber);
    SceneManager.LoadScene(_lastSceneNumber);

    CheckSaveFile();
    _data.AddSession();
    _data.SetLastLoginDate(DateTime.Now);
    _analytic.SendEventOnGameInitialize(_data.GetSessionCount());
    _data.Save();

    LoadLevel();
  }

  private void LoadLevel()
  {
    if (_isFirstLoad)
    {
      _isFirstLoad = false;
      ES3.Save(SaveProgress.TitleKey.IsFirstLoad, _isFirstLoad, SaveProgress.FilePath.Load);
      SceneManager.LoadScene(1);
    }
    else
    {
      _analytic.SendEventOnLevelStart(_lastSceneNumber);
      _analytic.SendEventOnLevelComplete(_lastSceneNumber);
      SceneManager.LoadScene(_lastSceneNumber);
    }
      
  }

  private void CheckSaveFile()
  {
    if (PlayerPrefs.HasKey(_data.GetKeyName()))
      _data.Load();
    else
      _data.SetDateRegistration(DateTime.Now);
  }
}