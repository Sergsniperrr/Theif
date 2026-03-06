using System;
using MirraGames.SDK;
using MirraGames.SDK.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelViewer : MonoBehaviour
{
    [SerializeField] private Level[] _levels;
    [SerializeField] private bool[] _buyLevels;
    [SerializeField] private CoinsDisplay _coinsDisplay;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private int _price;
    [SerializeField] private TMP_Text _levelName;
    [SerializeField] private Image _icon;
    [SerializeField] private bool _isBuy;
    [Header("Buttons")] [SerializeField] private Button _exitButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _leftArrow;
    [SerializeField] private Button _rightArrow;
    [SerializeField] private AnalyticManager _analytic;

    private int _currentLevel = 0;
    private string _sceneName;

    public event Action<int> LevelPurchased;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(CloseMenu);
        _leftArrow.onClick.AddListener(SwipeLeft);
        _rightArrow.onClick.AddListener(SwipeRight);
        _playButton.onClick.AddListener(SetLevel);
        _buyButton.onClick.AddListener(BuyLevel);
        
        GameStoper.Stop();
    }

    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(CloseMenu);
        _leftArrow.onClick.RemoveListener(SwipeLeft);
        _rightArrow.onClick.RemoveListener(SwipeRight);
        _playButton.onClick.RemoveListener(SetLevel);
        _buyButton.onClick.RemoveListener(BuyLevel);
        
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        LevelPurchaseData levelsData = new LevelPurchaseData(_buyLevels);
        levelsData = MirraSDK.Data.GetObject(SavableKeys.PurchaseLevels, levelsData);
        //MirraSDK.Data.SetObject(SavableKeys.PurchaseLevels, new LevelPurchaseData(_buyLevels));
        
        _buyLevels = levelsData.Levels;
        
        ChangeLevel();
    }
    
    private void SwipeLeft()
    {
        _currentLevel--;

        if (_currentLevel == 0)
            _leftArrow.gameObject.SetActive(false);

        _rightArrow.gameObject.SetActive(true);
        ChangeLevel();
    }

    private void SwipeRight()
    {
        _currentLevel++;

        if (_currentLevel == _levels.Length - 1)
            _rightArrow.gameObject.SetActive(false);

        _leftArrow.gameObject.SetActive(true);
        ChangeLevel();
    }

    private void ChangeLevel()
    {
        _price = _levels[_currentLevel].Price;
        _sceneName = _levels[_currentLevel].SceneName;
        _levelName.text = _levels[_currentLevel].TitleRu;
        _icon.sprite = _levels[_currentLevel].Icon;
        _isBuy = _buyLevels[_currentLevel];

        SwapLevelTitle();
        
        if (_isBuy == false)
        {
            _playButton.gameObject.SetActive(false);
            _buyButton.gameObject.SetActive(true);
        }
        else
        {
            _playButton.gameObject.SetActive(true);
            _buyButton.gameObject.SetActive(false);
        }
    }

    private void CloseMenu()
    {
        GameStoper.Restart();
        gameObject.SetActive(false);
    }

    private void SetLevel()
    {
        SceneManager.LoadScene(_sceneName);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void BuyLevel()
    {
        int nextValue = _wallet.Coins - _price;
        
        if (nextValue > 0 && _wallet.Coins >= _price)
        {
            int purchasesNumber = 0;
            purchasesNumber = ES3.Load(SaveProgress.TitleKey.PurchasesNumber, SaveProgress.FilePath.Purchases,
                purchasesNumber);

            purchasesNumber++;
            _buyLevels[_currentLevel] = true;
            
            LevelPurchased?.Invoke(_price);
            _buyButton.gameObject.SetActive(false);
            _playButton.gameObject.SetActive(true);
            
            MirraSDK.Data.SetObject(SavableKeys.PurchaseLevels, new LevelPurchaseData(_buyLevels));
        }
    }

    private void SwapLevelTitle()
    {
        if (MirraSDK.Language.Current == LanguageType.Russian)
            _levelName.text = _levels[_currentLevel].TitleRu + "\n ЦЕНА: " + _levels[_currentLevel].Price;
        else if (MirraSDK.Language.Current == LanguageType.Turkish)
            _levelName.text = _levels[_currentLevel].TitleTr + "\n FIYAT: " + _levels[_currentLevel].Price;
        else
            _levelName.text = _levels[_currentLevel].TitleEn + "\n PRICE: " + _levels[_currentLevel].Price;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameStoper.Restart();
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}