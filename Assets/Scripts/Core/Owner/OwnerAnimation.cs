using System;
using System.Collections;
using DG.Tweening;
using FieldOfViewAsset;
using UnityEngine;
using Random = UnityEngine.Random;

public class OwnerAnimation : MonoBehaviour
{
  [SerializeField] private GameObject _model;

  private FieldOfView _fieldOfView;
  private OwnerMover _ownerMover;
  private Animator _animator;
  private Coroutine _setAnimationRoutine;
  private string[] _animations = {"Action1", "Action3", "Action4", "Action5", "Action6", "Action7",};

  public event Action AnimationIsFinished;

  private void Awake()
  {
    _ownerMover = GetComponent<OwnerMover>();
    _animator = GetComponentInChildren<Animator>();
    _fieldOfView = GetComponentInChildren<FieldOfView>();
  }

  private void OnEnable()
  {
    _fieldOfView.TargetDetected += TurnToPlayer;
    _ownerMover.CameToPathPoint += SetRandomAnimation;
  }

  private void TurnToPlayer(GameObject target)
  {
    float duration = 0.5f;
    transform.DOLookAt(target.transform.position, duration, AxisConstraint.Y);
  }

  private void OnDisable()
  {
    _fieldOfView.TargetDetected -= TurnToPlayer;
      _ownerMover.CameToPathPoint -= SetRandomAnimation;
  }

  public void SetCallAnimation()
  {
    _ownerMover.StopMove();
    _ownerMover.StopMoveToPath();
    _ownerMover.StopMoveToPoint();
    _ownerMover.CameToPathPoint -= SetRandomAnimation;
    
    if(_setAnimationRoutine != null)
      StopCoroutine(_setAnimationRoutine);
    
    _animator.SetBool(OwnerAnimator.Params.IsWalk, false);
    _animator.SetBool(OwnerAnimator.Params.IsCall, true);
  }
  
  private void SetRandomAnimation()
  {
    int randomAnimation = Random.Range(0, _animations.Length);
    _setAnimationRoutine = StartCoroutine(SetAnimationRoutine(_animations[randomAnimation]));
  }

  private IEnumerator SetAnimationRoutine(string key)
  {
    _animator.SetTrigger(key);
    _animator.SetBool(OwnerAnimator.Params.IsWalk, false);
    float delay = 5f;
    
    WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
    yield return waitForSeconds;
    
    _model.transform.localRotation = Quaternion.Euler(Vector3.zero);
    AnimationIsFinished?.Invoke();
  }
}