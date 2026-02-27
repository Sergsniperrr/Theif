using DG.Tweening;
using UnityEngine;

public class MasterKey : MonoBehaviour
{
  private RectTransform _rectTransform;
  private float _duration = 0.5f;
    
  private void Awake() => _rectTransform = GetComponent<RectTransform>();

  public void MoveToPinPosition() => _rectTransform.DOAnchorPos(Vector2.zero, _duration).SetUpdate(UpdateType.Normal, true);
}