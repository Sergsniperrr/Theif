using UnityEngine;
using UnityEngine.UI;

public class BoxUI : MonoBehaviour
{
  [SerializeField] private Button _boxButton;
  [SerializeField] private GameObject _storage;
  
  private Vector3 _rotationToCamera = new Vector3(55, -135, 0);

  private void OnEnable()
  {
    _boxButton.onClick.AddListener(ActivateStorage);
    _boxButton.onClick.AddListener(DeactivateButton);
  }

  private void OnDisable()
  {
    _boxButton.onClick.RemoveListener(ActivateStorage);
    _boxButton.onClick.RemoveListener(DeactivateButton);
  }

  private void Start() => SetUIRotation(gameObject);

  public void ActivateButton() => _boxButton.gameObject.SetActive(true);
  
  public void DeactivateButton() => _boxButton.gameObject.SetActive(false);

  public void DeactivateStorage() => _storage.gameObject.SetActive(false);

  private void ActivateStorage() => _storage.gameObject.SetActive(true);
  
  private void SetUIRotation(GameObject panel) => panel.transform.rotation = Quaternion.Euler(_rotationToCamera);
}