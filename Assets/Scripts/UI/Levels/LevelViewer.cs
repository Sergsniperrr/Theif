using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelViewer : MonoBehaviour
{
  [SerializeField] private Level[] _levels;
  [SerializeField] private bool[] _buyLevels;
  [SerializeField] private BitcoinDisplay _bitcoinDisplay;
  [SerializeField] private Wallet _wallet;
  [SerializeField] private float _price;
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

  public event Action<float> LevelPurchased;

  private void OnEnable()
  {
    _exitButton.onClick.AddListener(CloseMenu);
    _leftArrow.onClick.AddListener(SwipeLeft);
    _rightArrow.onClick.AddListener(SwipeRight);
    _playButton.onClick.AddListener(SetLevel);
    _buyButton.onClick.AddListener(BuyLevel);
  }

  private void OnDisable()
  {
    _exitButton.onClick.RemoveListener(CloseMenu);
    _leftArrow.onClick.RemoveListener(SwipeLeft);
    _rightArrow.onClick.RemoveListener(SwipeRight);
    _playButton.onClick.RemoveListener(SetLevel);
    _buyButton.onClick.RemoveListener(BuyLevel);
  }

  private void Start()
  {
    if (ES3.KeyExists(SaveProgress.TitleKey.PurchasedLevels, SaveProgress.FilePath.PurchasedLevels))
      _buyLevels = ES3.Load(SaveProgress.TitleKey.PurchasedLevels, SaveProgress.FilePath.PurchasedLevels, _buyLevels);

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
    _levelName.text = _levels[_currentLevel].Title;
    _icon.sprite = _levels[_currentLevel].Icon;
    _isBuy = _buyLevels[_currentLevel];

    if (_isBuy == false)
    {
      _levelName.text = _levels[_currentLevel].Title + "\n PRICE: " + _levels[_currentLevel].Price + " .BTC";
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
    gameObject.SetActive(false);
  }

  private void SetLevel()
  {
    SceneManager.LoadScene(_sceneName);
  }

  private void BuyLevel()
  {
    float nextValue = _wallet.Bitcoin - _price;

    if (nextValue > 0 && _wallet.Bitcoin > _price)
    {
      int purchasesNumber = 0;
      purchasesNumber = ES3.Load(SaveProgress.TitleKey.PurchasesNumber, SaveProgress.FilePath.Purchases, purchasesNumber);

      purchasesNumber++;
      ES3.Save(SaveProgress.TitleKey.PurchasesNumber, purchasesNumber, SaveProgress.FilePath.Purchases);
      _buyLevels[_currentLevel] = true;
      _levelName.text = _levels[_currentLevel].Title;
      LevelPurchased?.Invoke(_price);
      _buyButton.gameObject.SetActive(false);
      _playButton.gameObject.SetActive(true);
      
      _analytic.SendEventOnSoftSpend("Buy Levels", _levels[_currentLevel].Title, _levels[_currentLevel].Price, purchasesNumber);
    }
  }
}