using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class Pin : MonoBehaviour
{
  [SerializeField] private Sprite _sprite;

  private RectTransform _rectTransform;
  private Image _image;
  private float _yOffset1 = 50f;
  private float _yOffset2 = 10f;
  private float _duration1 = 0.5f;
  private float _duration2 = 0.8f;
  private Sequence _pickupAnimation;

  public RectTransform RectTransform => _rectTransform;

  private void Awake()
  {
    _rectTransform = GetComponent<RectTransform>();
    _image = GetComponent<Image>();
  }

  public void RiseUp()
  {
    _pickupAnimation = DOTween.Sequence();

    _pickupAnimation
      .Append(_rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y + _yOffset1, _duration1)).SetUpdate(UpdateType.Normal, true)
      .Append(_rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y - _yOffset2, _duration2)).SetUpdate(UpdateType.Normal, true);
  }

  public void ChangeSprite() => _image.sprite = _sprite;

  public void MoveToFinishPosition()
  {
    _pickupAnimation.Kill();
      _rectTransform.DOAnchorPosY(Constant.MaximumAnchorY, 1).SetUpdate(UpdateType.Normal, true);
  }
}