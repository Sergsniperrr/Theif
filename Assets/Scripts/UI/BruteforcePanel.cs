using UnityEngine;

public class BruteforcePanel : MonoBehaviour
{
  [SerializeField] private GameObject _panel;
  
  private BruteForce _bruteForce;
  
  public BruteForce BruteForce => _bruteForce;
  
  
  private void Awake() => _bruteForce = GetComponent<BruteForce>();

  public void Show() => _panel.SetActive(true);

  public void Hide() => gameObject.SetActive(false);
}
