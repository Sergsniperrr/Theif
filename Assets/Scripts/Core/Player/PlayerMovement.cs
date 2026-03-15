using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour, IMovable
{
    private const float Angle = 135;

    private DynamicJoystick _dynamicJoystick;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Player _player;
    private float _deceleratingSpeed = 0.13f;
    private float _angleRad;
    private Vector3 _movementDirection;
    private Vector3 _newJoystick = Vector3.zero;
    private Vector3 _targetPos;
    private bool _canMove = true;

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
        if (_canMove == false)
            return;

        Move();
        ChangeAnimationState();
    }

    public void DecreaseSpeed() => _navMeshAgent.speed -= _deceleratingSpeed;

    public void SetScaredAnimation()
    {
        _animator.SetBool(PlayerAnimator.Params.IsScared, true);
        _navMeshAgent.Stop();
    }

    public void StartMoving() =>
        _canMove = true;

    public void StopMoving() =>
        _canMove = false;

    private void ChangeAnimationState()
    {
        _animator.SetBool(PlayerAnimator.Params.IsMove, IsMove);
    }

    private void Move()
    {
        _movementDirection.z = Input.GetAxis("Horizontal");
        _movementDirection.x = -Input.GetAxis("Vertical");

        _angleRad = Angle * Mathf.Deg2Rad;
        _newJoystick.x = _dynamicJoystick.Horizontal * Mathf.Cos(_angleRad) -
                         _dynamicJoystick.Vertical * Mathf.Sin(_angleRad);
        _newJoystick.z = _dynamicJoystick.Horizontal * Mathf.Sin(_angleRad) +
                         _dynamicJoystick.Vertical * Mathf.Cos(_angleRad);
        _targetPos = transform.position + _newJoystick + _movementDirection;

        if (_navMeshAgent.velocity.sqrMagnitude > Constant.MinimumMovementSpeed)
        {
            transform.LookAt(_targetPos);
            IsMove = true;
        }
        else
        {
            IsMove = false;
        }

        _navMeshAgent.SetDestination(_targetPos);
    }
}