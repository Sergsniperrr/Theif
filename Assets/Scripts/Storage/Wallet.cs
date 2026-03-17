using System;
using MirraGames.SDK;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private const int ItemPrice = 50;

    [SerializeField] private int _coins;
    [SerializeField] private LevelViewer _levelViewer;
    [SerializeField] private LevelButton _levelButton;
    [SerializeField] private SellButton _sellButton;
    [SerializeField] private RewardAd _rewardAdButton;

    private int _totalCoins;
    
    public int Coins => _coins;

    public event Action<int> CoinChanged;

    private void Awake()
    {
        _coins = MirraSDK.Data.GetInt(SavableKeys.Coins);
        _totalCoins = MirraSDK.Data.GetInt(SavableKeys.Coins);
    }

    private void OnEnable()
    {
        _sellButton.ItemSold += IncreaseMoneyFromSaleItem;
        _levelButton.LevelsViewEnebled += SubscribeToLevelsViewer;
        _rewardAdButton.RewardAdShowed += IncreaseMoneyFromReward;
    }

    private void OnDisable()
    {
        _sellButton.ItemSold -= IncreaseMoneyFromSaleItem;
        _levelButton.LevelsViewEnebled -= SubscribeToLevelsViewer;
        _rewardAdButton.RewardAdShowed -= IncreaseMoneyFromReward;
    }

    private void SubscribeToLevelsViewer()
    {
        _levelViewer.LevelPurchased += DecreaseMoney;
        _levelViewer.Closed += UnsubscribeFromLevelsViewer;
    }
    
    private void UnsubscribeFromLevelsViewer()
    {
        _levelViewer.LevelPurchased -= DecreaseMoney;
        _levelViewer.Closed -= UnsubscribeFromLevelsViewer;
    }

    private void IncreaseMoneyFromSaleItem(int item)
    {
        IncreaseMoney(ItemPrice);
    }

    private void IncreaseMoneyFromReward(int coins)
    {
        IncreaseMoney(coins);
    }
    
    private void IncreaseMoney(int coins)
    {
        if (coins <= 0)
            throw new ArgumentOutOfRangeException(nameof(coins));
        
        _coins += coins;
        _totalCoins += coins;

        CoinChanged?.Invoke(_coins);
        
        MirraSDK.Data.SetInt(SavableKeys.Coins, _coins);
        MirraSDK.Data.SetInt(SavableKeys.TotalCoins, _totalCoins);
        MirraSDK.Achievements.SetScore(LeaderboardParams.Name, _totalCoins);
    }

    private void DecreaseMoney(int coins)
    {
        if (coins > _coins || coins <= 0 || _coins == 0)
            return;
        
        _coins -= coins;
        CoinChanged?.Invoke(_coins);
        
        MirraSDK.Data.SetInt(SavableKeys.Coins, _coins);
    }
}