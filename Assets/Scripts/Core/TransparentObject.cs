using DG.Tweening;
using UnityEngine;

// [RequireComponent(typeof(MeshRenderer))]
public class TransparentObject : MonoBehaviour
{
  private const float MaximumValueAlpha = 1;
  private const float MinimumValueAlpha = 0;
  
  [SerializeField] private LayerMask _raycastMask;
  [SerializeField] private float _duration;

  private MeshRenderer _meshRenderer;
  private float _rayDistance = 100f;
  private bool _isEnable;

  private void Awake()
  {
    _meshRenderer = GetComponent<MeshRenderer>();
  }

  private void OnTriggerStay(Collider other)
  {
    if (other.gameObject.TryGetComponent(out Player player))
      Hit(player);
  }

  private void Hit(Player player)
  {
    RaycastHit hit;
    Vector3 direction = player.transform.position - Camera.main.transform.position;

    if (Physics.Raycast(Camera.main.transform.position, direction, out hit, _rayDistance, _raycastMask))
    {
      if (hit.collider.TryGetComponent(out TransparentObject transparentObject))
        Hide(transparentObject);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.TryGetComponent(out Player player))
      Show();
  }

  private void Hide(TransparentObject transparentObject)
  {
    transparentObject._meshRenderer.material.DOFade(MinimumValueAlpha, _duration);
  }

  private void Show()
  {
    _meshRenderer.material.DOFade(MaximumValueAlpha, _duration);
  }
}