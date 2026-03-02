using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private const float MaximumValueSlider = 1;

    [SerializeField] private Image _slider;
    [SerializeField] protected Button InteractButton;
    [SerializeField] private Image _grabImage;
    [SerializeField] private Sprite _weightSprite;
    [SerializeField] private Sprite _grabSprite;
    [field: SerializeField] public Sprite Icon;
    [field: SerializeField] public float Weight;
    [field: SerializeField] public bool OnWall;
    [field: SerializeField] public int Index;

    private Player _player;
    private Vector3 _rotationToCamera = new Vector3(55, -135, 0);
    private Tween _fillAnimation;
    private Sequence fullAnimation;
    private InteractUI _interactUI;
    private bool _isButtonActivate;
    private bool _isWork = true;
    private float _distance = 1.7f;
    private int _jumpCapacity = 1;
    private int _jumpPower = 1;
    private float _jumpDuration = .5f;
    protected Collider ItemCollider;

    private void Awake()
    {
        ItemCollider = GetComponent<Collider>();
        _interactUI = GetComponentInChildren<InteractUI>();
    }

    private void OnEnable()
    {
        InteractButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        InteractButton.onClick.RemoveListener(OnButtonClick);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.TryGetComponent(out Player player) &&
            Vector3.Distance(transform.position, player.transform.position) <= _distance && _isWork)
        {
            _player = player;
            SetRotation(InteractButton.gameObject);
            ChangeUiView(true);
        }
        else if (collider.TryGetComponent(out Player player1) &&
                 Vector3.Distance(transform.position, player1.transform.position) > _distance)
        {
            ChangeUiView(false);

            if (_slider != null)
            {
                _isWork = true;
                _slider.fillAmount = 0;
                
                if (_fillAnimation != null && _fillAnimation.IsActive())
                    _fillAnimation.Kill();

                _slider.gameObject.SetActive(false);
            }
        }
    }

    public void DeactivateItem()
    {
        _interactUI.gameObject.SetActive(false);
        ItemCollider.enabled = false;
        enabled = false;
    }

    private void OnTriggerExit(Collider collider)
    {
        _isWork = true;

        if (collider.TryGetComponent(out Player player) && _isButtonActivate)
        {
            _isButtonActivate = false;
            player.EnabledStealth();
        }
    }

    private void ChangeUiView(bool state) => InteractButton.gameObject.SetActive(state);

    private void SetRotation(GameObject item) => item.transform.rotation = Quaternion.Euler(_rotationToCamera);

    protected virtual void OnButtonClick()
    {
        float nextWeight = _player.Weight + Weight;

        if (nextWeight <= Constant.MaximumWeight)
        {
            _player.TurnOffStealth();
            _player.DeactivateStealth();
            Deactivate();
            FillToSlider();
        }
        else
        {
            _grabImage.sprite = _weightSprite;
            var imageTransform = _grabImage.GetComponent<RectTransform>();
            float duration = 0.5f;
            float strength = 0.3f;

            imageTransform.DOShakePosition(duration, strength).OnComplete((() => { _grabImage.sprite = _grabSprite; }));
        }
    }

    private void JumpToPlayer()
    {
        transform.DOJump(_player.transform.position, _jumpPower, _jumpCapacity, _jumpDuration)
            .OnComplete(() =>
            {
                transform.SetParent(_player.transform);
                gameObject.SetActive(false);
            });

        _player.TakeItem(this);
    }

    private void FillToSlider()
    {
        _slider.gameObject.SetActive(true);
        SetRotation(_slider.gameObject);
        _isButtonActivate = true;

        _fillAnimation = _slider.DOFillAmount(MaximumValueSlider, Constant.SliderMultiplier * Weight)
            .SetEase(Ease.Linear)
            .OnComplete((() =>
            {
                _slider.gameObject.SetActive(false);
                JumpToPlayer();

                _player.EnabledStealth();
                _player.ActivateStealth();
                // _player.PlayerNoise.MakeIsInactiveAction();
                // _player.PlayerNoise.ChangeColliderRadius(Constant.BaseRadiusPlayerCollider);
                // _player.PlayerNoise.ChangeScale(Constant.StandartNoiseSize);
            }));
    }

    protected virtual void Deactivate()
    {
        _slider.gameObject.SetActive(false);
        _isWork = false;
        InteractButton.gameObject.SetActive(false);
    }
}