using System;
using TMPro;
using UnityEngine;

public class CoinsDisplay : MonoBehaviour
{
  [SerializeField] private TMP_Text _coinsText;
  [SerializeField] private Wallet _wallet;
  
  private void OnEnable() => _wallet.CoinChanged += ChangeValue;

  private void OnDisable() => _wallet.CoinChanged -= ChangeValue;

  private void Start()
  {
    _coinsText.text = $"{_wallet.Coins}";
  }

  private void ChangeValue(int coins)
  {
    _coinsText.text = $"{coins}";
  }
}