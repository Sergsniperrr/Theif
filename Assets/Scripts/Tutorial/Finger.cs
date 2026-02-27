using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Finger : MonoBehaviour
{
  [SerializeField] private Sprite _baseFinger;
  [SerializeField] private Sprite _enteredFinger;
  [SerializeField] private Image _finger;
  [SerializeField] private RectTransform _rectTransform;

  private float _endValue = 0.41f;
  private float _startValue = -0.74f;
  private float _duration1 = 1f;
  private float duration2 = 0.01f;

  private void Start()
  {
    StartCoroutine(MoveRoutine());
  }

  private IEnumerator MoveRoutine()
  {
    float delay1 = 0.5f;
    float delay2 = 0.3f;
    WaitForSeconds waitForSeconds1 = new WaitForSeconds(delay1);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(delay2);
    Tween tween;

    while (true)
    {
      yield return waitForSeconds1;
      _finger.sprite = _enteredFinger;
      yield return waitForSeconds2;
      tween = _rectTransform.DOAnchorPosY(_endValue, _duration1);

      yield return tween.WaitForCompletion();

      _finger.sprite = _baseFinger;
      _rectTransform.DOAnchorPosY(_startValue, duration2);
    }
  }
}