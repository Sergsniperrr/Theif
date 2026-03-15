using MirraGames.SDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SDKLoader : MonoBehaviour
{
    private readonly int _idleZoneIndex = 1;
    
    private void Start()
    {
        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Ждём инициализации MirraSDK
        MirraSDK.WaitForProviders(() =>
        {
            // После инициализации SDK загружаем игровую сцену
            SceneManager.LoadScene(_idleZoneIndex, LoadSceneMode.Single);
        });
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Отписываемся от события
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Сообщаем SDK, что игра полностью загружена
        MirraSDK.Analytics.GameIsReady();
        MirraSDK.Analytics.GameplayStart();
    }
}
