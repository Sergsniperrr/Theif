using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailHud : HUD
{
  [SerializeField] private Button _goToHomeButton;
  [SerializeField] private Button _restartButton;
  [SerializeField] private AnalyticManager _analytic;

  private CanvasGroup _canvasGroup;
  private float _endValue = 1;
  private float _duration = 0.5f;
  private float _delay = 3f;
  
  private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

  private void OnEnable()
  {
    _goToHomeButton.onClick.AddListener(GoToIdleZone);
    _restartButton.onClick.AddListener(RestartLevel);
  }

  private void OnDisable()
  {
    _goToHomeButton.onClick.RemoveListener(GoToIdleZone);
    _restartButton.onClick.RemoveListener(RestartLevel);
  }

  public void ChangeCanvasAlphaValue()
  {
    _analytic.SendEventOnFail(SceneManager.GetActiveScene().buildIndex);
    _canvasGroup.DOFade(_endValue, _duration).SetDelay(_delay);
  }

  private void GoToIdleZone()
  {
    _analytic.SendEventOnLevelComplete(SceneManager.GetActiveScene().buildIndex);
    SceneManager.LoadScene(Constant.IdleZone);
  }

  private void RestartLevel()
  {
    _analytic.SendEventOnLevelRestart(SceneManager.GetActiveScene().buildIndex);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}