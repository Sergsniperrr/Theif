using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
  [SerializeField] private LevelViewer _levelViewer;
  [SerializeField] private Button _button;
  
  private void OnEnable()
  {
    _button.onClick.AddListener(ActivateButton);
  }

  private void OnDisable()
  {
    _button.onClick.RemoveListener(ActivateButton);
  }

  private void OnTriggerEnter(Collider collider)
  {
    if (collider.TryGetComponent(out Player player))
      _button.interactable = true;
  }

  private void OnTriggerExit(Collider collider)
  {
    if (collider.TryGetComponent(out Player player))
      _button.interactable = false;
  }

  private void ActivateButton()
  {
    _levelViewer.gameObject.SetActive(true);
  }
}