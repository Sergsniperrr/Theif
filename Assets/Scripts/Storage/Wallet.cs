using System;
using MirraGames.SDK;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private const int RoundingDepth = 2;
    private const int ItemPrice = 50;

    [SerializeField] private int _coins;
    [SerializeField] private LevelViewer _levelViewer;
    [SerializeField] private SellButton _sellButton;

    public float Coins => _coins;

    public event Action<int> CoinChanged;

    private void Awake()
    {
        _coins = MirraSDK.Data.GetInt(SavableKeys.Coins);
    }

    public void OnEnable()
    {
        _sellButton.ItemSold += IncreaseMoney;
        _levelViewer.LevelPurchased += DecreaseMoney;
    }

    private void OnDisable()
    {
        _sellButton.ItemSold += IncreaseMoney;
        _levelViewer.LevelPurchased -= DecreaseMoney;
    }

    private void IncreaseMoney(int item)
    {
        _coins += ItemPrice;

        CoinChanged?.Invoke(_coins);
        MirraSDK.Data.SetInt(SavableKeys.Coins, _coins);
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