using System;
using System.Collections;
using System.Collections.Generic;
using MirraGames.SDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardAd : MonoBehaviour
{
    private const int RewardAmount = 300;
    
    private readonly WaitForSeconds _waitForOneSecond = new (1f);
    
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _rewardAmountText;
    [SerializeField] private Image _icon;

    private bool _isReady = true;
    private Coroutine _checkRewardCoroutine;
    
    public event Action<int> RewardAdShowed;

    private void Awake()
    {
        _rewardAmountText.text = $"x{RewardAmount}";
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ShowRewardAd);
        
        if (_checkRewardCoroutine != null)
            StopCoroutine(_checkRewardCoroutine);
            
        _checkRewardCoroutine = StartCoroutine(CheckRewardAdReady());
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ShowRewardAd);
        
        if (_checkRewardCoroutine != null)
            StopCoroutine(_checkRewardCoroutine);
    }
    
    private void ShowRewardAd()
    {
        if (_isReady == false)
            return;
        
        MirraSDK.Ads.InvokeRewarded(
            onOpen: () => Debug.Log("Реклама за вознаграждение открыта"),
            onClose: HandleShowRewardAd);
    }

    private IEnumerator CheckRewardAdReady()
    {
        while (isActiveAndEnabled)
        {
            yield return _waitForOneSecond;

            if (MirraSDK.Ads.IsRewardedReady)
            {
                _icon.color = Color.white;
                _isReady = true;
            }
            else
            {
                _icon.color = Color.grey;
                _isReady = false;
            }
        }
    }
    
    private void HandleShowRewardAd(bool isSuccess)
    {
        if (isSuccess)
        {
            RewardAdShowed?.Invoke(RewardAmount);
        }
    }
}
