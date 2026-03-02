using System.Collections;
using System.Collections.Generic;
using MirraGames.SDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SDKLoader : MonoBehaviour
{
    private const string InitialScene = "Initial";
    
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        MirraSDK.WaitForProviders(() =>
        {
            SceneManager.LoadScene(InitialScene, LoadSceneMode.Single);
        });
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
