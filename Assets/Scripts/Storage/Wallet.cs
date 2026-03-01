using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
  private const int RoundingDepth = 2;
  private const float ItemPrice = 0.05f;
  
  [SerializeField] private float _bitcoin;
  [SerializeField] private LevelViewer _levelViewer;
  [SerializeField] private SellButton _sellButton;

  public float Bitcoin => _bitcoin;
  
  public event Action<float> BitcoinChanged;

  private void Awake() => _bitcoin = ES3.Load(SaveProgress.TitleKey.Money, SaveProgress.FilePath.Money, _bitcoin);

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
    _bitcoin = (float)Math.Round(_bitcoin + ItemPrice, RoundingDepth, MidpointRounding.AwayFromZero);
    
    BitcoinChanged?.Invoke(_bitcoin);
    ES3.Save(SaveProgress.TitleKey.Money, _bitcoin, SaveProgress.FilePath.Money);
  }

  private void DecreaseMoney(float bitcoin)
  {
    _bitcoin -= bitcoin;
    BitcoinChanged?.Invoke(_bitcoin);
    ES3.Save(SaveProgress.TitleKey.Money, _bitcoin, SaveProgress.FilePath.Money);
  }
}