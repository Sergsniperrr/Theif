using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticManager : MonoBehaviour
{
    
    [SerializeField] private GameAnalyticsObject _gameAnalyticsObject;

    public void SendEventOnGameInitialize(int sessionCount)
    {
        _gameAnalyticsObject.OnGameInitialize(sessionCount);
    }

    public void SendEventOnLevelStart(int levelNumber)
    {
        _gameAnalyticsObject.OnLevelStart(levelNumber); 
    }

    public void SendEventOnLevelComplete(int levelNumber)
    {
        _gameAnalyticsObject?.OnLevelComplete(levelNumber);
    }

    public void SendEventOnFail(int levelNumber)
    {
        _gameAnalyticsObject?.OnFail(levelNumber);
    }

    public void SendEventOnLevelRestart(int levelNumber)
    {
        _gameAnalyticsObject?.OnLevelRestart(levelNumber);
    }

    public void SendEventOnSoftSpend(string purchaseType, string storeName, float purchaseAmount, int purchasesCount)
    {
        _gameAnalyticsObject.OnSoftSpend(purchaseType,storeName, purchaseAmount, purchasesCount);
    }

    public void SendEventOnGameExit(string registrationDate, int sessionCount, int daysInGame)
    {
        _gameAnalyticsObject.OnGameExit(registrationDate,sessionCount, daysInGame);
    }

    public void SendEventOnGameExit(string registrationDate, int sessionCount, int daysInGame, float currentSoft)
    {
       _gameAnalyticsObject.OnGameExit(registrationDate, sessionCount, daysInGame,currentSoft);
    }
}
