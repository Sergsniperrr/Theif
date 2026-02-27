using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BruteForce : MonoBehaviour
{
  [SerializeField] private Pin[] _pins;
  [SerializeField] private Button _button;
  [SerializeField] private MasterKey _masterKey;
  [SerializeField] private ParticleSystem _poofParticle;

  private float _duration = 1f;
  private float _compressionDuration = 0.4f;
  private int _currentPin = 0;
  private ItemGenerator _itemGenerator;

  public event Action BrutforceCompleted;

  [Inject]
  private void Contructor(ItemGenerator itemGenerator)
  {
    _itemGenerator = itemGenerator;
  }

  private void OnEnable() => _button.onClick.AddListener(PickUpPin);

  private void OnDisable() => _button.onClick.RemoveListener(PickUpPin);

  private void PickUpPin()
  {
    if (_pins[_currentPin].RectTransform.anchoredPosition.y <= Constant.MaximumAnchorY)
      _pins[_currentPin].RiseUp();
    else
    {
      _pins[_currentPin].ChangeSprite();

      if (_currentPin != _pins.Length - 1)
      {
        _currentPin++;
        _pins[_currentPin - 1].MoveToFinishPosition();
        _masterKey.transform.SetParent(_pins[_currentPin].transform);
        _masterKey.MoveToPinPosition();
        MoveButton();
      }
      else
      {
        _pins[_currentPin].MoveToFinishPosition();
        _masterKey.transform.SetParent(_pins[0].transform);
        Vector2 position = new Vector2(_pins[0].RectTransform.anchorMax.x, 0);
        float duration = 1f;
        _button.transform.DOScale(Vector3.zero, _compressionDuration).SetUpdate(UpdateType.Normal, true);
        _masterKey.GetComponent<RectTransform>().DOAnchorPos(position, duration)
          .SetUpdate(UpdateType.Normal, true).OnComplete((() =>
          {
            BrutforceCompleted?.Invoke();
            gameObject.SetActive(false);
            Container container = GetComponentInParent<Container>();
            _poofParticle.transform.position = container.Point.transform.position;
            int key = (int) container.ContainerType;
            string keyString = key.ToString();
            GameObject newItem = _itemGenerator.GetItem(keyString);
            Instantiate(newItem, container.Point.transform.position, Quaternion.identity, null);
            _poofParticle.Play();
          }));
      }
    }
  }

  private void MoveButton()
  {
    RectTransform buttonRect = _button.GetComponent<RectTransform>();
    buttonRect.DOAnchorPosX(_pins[_currentPin].RectTransform.anchoredPosition.x, _duration)
      .SetUpdate(UpdateType.Normal, true);
  }
}