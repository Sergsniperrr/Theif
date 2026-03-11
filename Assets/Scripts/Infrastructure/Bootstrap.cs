using System;
using MirraGames.SDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    private readonly int _idleZoneIndex = 1;

    private void OnEnable()
    {
        SceneManager.LoadScene(_idleZoneIndex);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MirraSDK.Analytics.GameIsReady();
        MirraSDK.Analytics.GameplayStart();
    }
}