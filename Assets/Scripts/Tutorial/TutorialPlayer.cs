using System.Collections;
using UnityEngine;
using DG.Tweening;

public class TutorialPlayer : MonoBehaviour
{
  [SerializeField] private Transform _targetPosition;
  [SerializeField] private GameObject _box;
  [SerializeField] private GameObject _model;

  private Vector3 _startPosition;
  private Animator _animator;
  private Tween _moveAnimation;
  private ParticleSystem _poofEffect;

  private void Start()
  {
    _animator = GetComponentInChildren<Animator>();
    _poofEffect = GetComponentInChildren<ParticleSystem>();
    _startPosition = transform.position;
    StartCoroutine(ShowTransformation());
  }

  private IEnumerator ShowTransformation()
  {
    float delay1 = 0.5f;
    float delay2 =2f;
    float duration = 2.5f;
    WaitForSeconds waitForSeconds1 = new WaitForSeconds(delay1);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(delay2);

    while (true)
    {
      yield return waitForSeconds1;
      _animator.SetBool(PlayerAnimator.Params.IsMove, true);
      _moveAnimation = transform.DOMove(_targetPosition.position, duration).SetEase(Ease.Linear);
      yield return _moveAnimation.WaitForCompletion();
      _animator.SetBool(PlayerAnimator.Params.IsMove, false);
      yield return waitForSeconds2;
      ActivateStealth();
      yield return waitForSeconds2;
      DeactivateStealth();
      transform.position = _startPosition;
    }
  }

  private void ActivateStealth()
  {
    _poofEffect.Play();
    _box.gameObject.SetActive(true);
    _model.gameObject.SetActive(false);
  }

  private void DeactivateStealth()
  {
    _poofEffect.Play();
    _box.gameObject.SetActive(false);
    _model.gameObject.SetActive(true);
  }
}