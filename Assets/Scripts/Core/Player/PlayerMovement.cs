using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
  private const float Angle = 135;

  private DynamicJoystick _dynamicJoystick;
  private NavMeshAgent _navMeshAgent;
  private Animator _animator;
  private Player _player;
  private float _deceleratingSpeed = 0.13f;

  public bool IsMove { get; private set; } = false;

  [Inject]
  private void Construct(DynamicJoystick dynamicJoystick)
  {
    _dynamicJoystick = dynamicJoystick;
  }

  private void Awake()
  {
    _navMeshAgent = GetComponent<NavMeshAgent>();
    _player = GetComponent<Player>();
    _animator = GetComponentInChildren<Animator>();
  }

  private void Start() => _navMeshAgent.updateRotation = false;

  private void Update()
  {
    Move();
    ChangeAnimationState();
  }

  public void DecreaseSpeed() => _navMeshAgent.speed -= _deceleratingSpeed;

  public void SetScaredAnimation()
  {
    _animator.SetBool(PlayerAnimator.Params.IsScared, true);
    _navMeshAgent.Stop();
  }

  private void ChangeAnimationState()
  {
    _animator.SetBool(PlayerAnimator.Params.IsMove, IsMove);
  }

  private void Move()
  {
    float angleRad = Angle * Mathf.Deg2Rad;
    Vector2 newJoystick = new Vector2(_dynamicJoystick.Horizontal, _dynamicJoystick.Vertical);
    newJoystick.x = _dynamicJoystick.Horizontal * Mathf.Cos(angleRad) - _dynamicJoystick.Vertical * Mathf.Sin(angleRad);
    newJoystick.y = _dynamicJoystick.Horizontal * Mathf.Sin(angleRad) + _dynamicJoystick.Vertical * Mathf.Cos(angleRad);
    Vector3 targetPos = transform.position + new Vector3(newJoystick.x, 0, newJoystick.y);

    if (_navMeshAgent.velocity.sqrMagnitude > Constant.MinimumMovementSpeed ||
        _navMeshAgent.velocity.sqrMagnitude > Constant.MinimumMovementSpeed)
    {
      transform.LookAt(targetPos);
      IsMove = true;
    }
    else
      IsMove = false;


    _navMeshAgent.SetDestination(targetPos);
  }
}