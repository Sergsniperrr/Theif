using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailHud : HUD
{
    [SerializeField] private Button _goToHomeButton;
    [SerializeField] private Button _restartButton;

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
        _canvasGroup.DOFade(_endValue, _duration)
            .SetDelay(_delay)
            .OnComplete(GameStoper.Stop);
    }

    private void GoToIdleZone()
    {
        ReloadScene(Constant.IdleZone);
    }

    private void RestartLevel()
    {
        ReloadScene(SceneManager.GetActiveScene().name);
    }

    private void ReloadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameStoper.Restart();
    }
}