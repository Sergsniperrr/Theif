using MirraGames.SDK;

public static class GameStoper
{
    private const float PauseTimeScale = 0f;
    private const float NormalTimeScale = 1f;
    
    public static void Start()
    {
        MirraSDK.Analytics.GameplayStart();
    }

    public static void Stop()
    {
        MirraSDK.Time.Scale = PauseTimeScale;
        MirraSDK.Analytics.GameplayStop();
    }

    public static void Restart()
    {
        MirraSDK.Time.Scale = NormalTimeScale;
        MirraSDK.Analytics.GameplayRestart();
    }
}
