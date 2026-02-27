using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerNoise : MonoBehaviour
{
  [SerializeField] private GameObject _noiseEffect;

  private PlayerMovement _playerMovement;
  private CapsuleCollider _capsuleCollider;
  private bool _isAction;
  private Tween tween;

  private void Awake()
  {
    _playerMovement = GetComponent<PlayerMovement>();
    _capsuleCollider = GetComponent<CapsuleCollider>();
  }

  public void MakeIsAction() => _isAction = true;
  public void MakeIsInactiveAction() => _isAction = false;

  private void Update()
  {
    if (_isAction == false)
      ChangeNoiseSize();
  }

  public void ChangeColliderRadius(float radius)
  {
    _capsuleCollider.radius = radius;
  }

  public void ChangeScale(float value)
  {
    float duration = 0.5f;
    Vector3 endValue = new Vector3(value, value, value);

    tween.Kill();
    
    if(_noiseEffect.transform.lossyScale != endValue)
    tween = _noiseEffect.gameObject.transform.DOScale(endValue, duration);
  }

  private void ChangeNoiseSize()
  {
    if (_playerMovement.IsMove)
    {
      ChangeColliderRadius(Constant.NoiseColliderSize);
      ChangeScale(Constant.MoveNoiseSize);
    }
    else
    {
      ChangeColliderRadius(Constant.BaseRadiusPlayerCollider);
      ChangeScale(Constant.StandartNoiseSize);
    }
  }
}