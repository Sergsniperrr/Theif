using System;
using TMPro;
using UnityEngine;

public class BitcoinDisplay : MonoBehaviour
{
  [SerializeField] private TMP_Text _bitcoinText;
  [SerializeField] private Wallet _wallet;
  
  private void OnEnable() => _wallet.BitcoinChanged += ChangeValue;

  private void OnDisable() => _wallet.BitcoinChanged -= ChangeValue;

  private void Start()
  {
    _bitcoinText.text = _wallet.Bitcoin + " .BTC";
  }

  private void ChangeValue(float bitcoin)
  {
    _bitcoinText.text = bitcoin + " .BTC";
  }
}