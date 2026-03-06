using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private AnalyticManager _analytic;

    private Button _goToHomeButton;

    public event Action PlayerLeft;

    private void Awake()
    {
        _goToHomeButton = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        _goToHomeButton.onClick.AddListener(GoToIdleZone);
    }

    private void OnDisable()
    {
        _goToHomeButton.onClick.RemoveListener(GoToIdleZone);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out FieldOfViewTarget player))
            _goToHomeButton.interactable = true;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out FieldOfViewTarget player))
            _goToHomeButton.interactable = false;
    }

    private void GoToIdleZone()
    {
        GameStoper.Stop();
        SceneManager.LoadScene(Constant.IdleZone);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        GameStoper.Restart();
    }
}