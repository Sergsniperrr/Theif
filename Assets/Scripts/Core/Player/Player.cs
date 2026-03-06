using System;
using System.Collections.Generic;
using MirraGames.SDK;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    private const float TimeBetweenIsInBox = 1f;

    [SerializeField] private ExitButton _exitButton;
    [SerializeField] private ParticleSystem _poofEffect;
    [SerializeField] private GameObject _box;
    [SerializeField] private GameObject _basicZoneBlue;
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject _targetForFieldOfView;
    [SerializeField] private bool _isTutorial;

    private PlayerMovement _playerMover;
    private PlayerNoise _playerNoise;
    private bool _isFirstEnable =  true;
    private bool _isInBox;
    private bool _isGameOver;
    private float _elapsedTime;
    private float _weight;
    private readonly List<int> _itemsIndex = new ();
    private readonly List<Item> _items = new ();

    public IReadOnlyList<Item> Items => _items;

    public PlayerMovement PlayerMover => _playerMover;

    public float Weight => _weight;

    public event Action<float> WeightIncreased;

    private void OnEnable()
    {
        if (_exitButton != null)
            _exitButton.PlayerLeft += SaveItems;
    }

    private void OnDisable()
    {
        if (_exitButton != null)
            _exitButton.PlayerLeft -= SaveItems;
    }

    private void Awake()
    {
        _playerNoise = GetComponent<PlayerNoise>();
        _playerMover = GetComponent<PlayerMovement>();

        if (_isTutorial)
            TurnOffStealth();
    }

    private void Update()
    {
        if (_isGameOver)
            return;

        _elapsedTime += Time.deltaTime;

        if (_playerMover.IsMove)
        {
            _elapsedTime = 0;
            DeactivateStealth();
        }
        else if (_elapsedTime > TimeBetweenIsInBox && _playerMover.IsMove == false)
        {
            _elapsedTime = 0;
            ActivateStealth();
        }
    }

    public void EnabledStealth()
    {
        _isGameOver = false;
    }

    public void TurnOffStealth()
    {
        _isGameOver = true;
    }

    public void DeactivateStealth()
    {
        if (_isInBox)
        {
            _isInBox = false;
            _poofEffect.Play();
            _box.gameObject.SetActive(false);
            _model.gameObject.SetActive(true);
            _targetForFieldOfView.gameObject.SetActive(true);
        }
    }

    public void ActivateStealth()
    {
        if (_isInBox == false)
        {
            _isInBox = true;
            _poofEffect.Play();
            _box.gameObject.SetActive(true);
            _model.gameObject.SetActive(false);
            _targetForFieldOfView.gameObject.SetActive(false);
        }
    }

    private void SaveItems() => ES3.Save(SaveProgress.TitleKey.Items, _itemsIndex, SaveProgress.FilePath.Items);

    public void AddItem(Item item) => _items.Add(item);

    public void DeleteItem(Item item) => _items.Remove(item);

    public void TakeItem(Item item)
    {
        _weight += item.Weight;
        _playerMover.DecreaseSpeed();
        _itemsIndex.Add(item.Index);
        WeightIncreased?.Invoke(item.Weight);
    }
}