using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class OwnerMover : MonoBehaviour
{
 [SerializeField] private PathGenerator _pathGenerator;
 
  private NavMeshAgent _navMeshAgent;
  private Animator _animator;
  private Coroutine _moveToPath;
  private Coroutine _moveToPoint;
  private NavmeshPathDraw _navMeshPathDraw;
  private OwnerAnimation _ownerAnimation;

  public event Action CameToLastPositionPlayer;
  public event Action CameToPathPoint;

  private void OnEnable() => _ownerAnimation.AnimationIsFinished += MoveToPath;

  private void OnDisable() => _ownerAnimation.AnimationIsFinished -= MoveToPath;

  private void Awake()
  {
    _animator = GetComponentInChildren<Animator>();
    _ownerAnimation = GetComponent<OwnerAnimation>();
    _navMeshAgent = GetComponent<NavMeshAgent>();
    _navMeshPathDraw = GetComponentInChildren<NavmeshPathDraw>();
  }

  public void SetMovementSpeed(float speed) => _navMeshAgent.speed = speed;

  public void MoveToPath() => _moveToPath = StartCoroutine(MoveToPathRoutine());

  public void MoveToPoint(Vector3 lastPlayerPosition)
  {
    _animator.SetBool(OwnerAnimator.Params.IsWalk, true);
    _navMeshAgent.Resume();
    _moveToPoint = StartCoroutine(MoveToPointRoutine(lastPlayerPosition));
  }

  public void StopMove() => _navMeshAgent.Stop();

  public void StopMoveToPath()
  {
    if(_moveToPath != null)
      StopCoroutine(_moveToPath);
  }

  public void StopMoveToPoint()
  {
    if(_moveToPoint != null)
      StopCoroutine(_moveToPoint);
  }

  private bool CheckDistanceToPoint()
  {
    if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
      return true;

    return false;
  }

  private IEnumerator MoveToPointRoutine(Vector3 lastPlayerPosition)
  {
    _navMeshAgent.SetDestination(lastPlayerPosition);
    
    _navMeshPathDraw.destination = lastPlayerPosition;

    while (CheckDistanceToPoint() == false)
      yield return null;

    CameToLastPositionPlayer?.Invoke();
  }

  private IEnumerator MoveToPathRoutine()
  {
    _animator.SetBool(OwnerAnimator.Params.IsWalk, true);
    Transform point = _pathGenerator.GetPoint().transform;
    var path = _navMeshAgent.path = new NavMeshPath();
    _navMeshAgent.CalculatePath(point.position, path);
    _navMeshAgent.SetPath(path);
    _navMeshPathDraw.destination = point.transform.position;
    _navMeshAgent.Resume();
    
    while (CheckDistanceToPoint() == false)
      yield return null;

    CameToPathPoint?.Invoke();
  }
}