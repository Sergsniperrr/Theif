using System.Collections;
using Cinemachine;
using UnityEngine;

public class IdleZoneTutorial : MonoBehaviour
{
  [SerializeField] private CinemachineVirtualCamera _mainCamera;
  [SerializeField] private CinemachineVirtualCamera[] _cameras;
  [SerializeField] private Canvas[] _canvases;

  private float _delay = 4;
  private float _startDelay = 0.5f;
  private bool _isShowed;
  private int _currentIndex;

  private void Start()
  {
    _isShowed = ES3.Load(SaveProgress.TitleKey.ShowIdleZone, SaveProgress.FilePath.IdleZoneTutorial, _isShowed);

    if (_isShowed)
      return;

    StartCoroutine(Show());
  }

  private IEnumerator Show()
  {
    WaitForSeconds waitForSeconds = new WaitForSeconds(_delay);
    WaitForSeconds waitForSeconds1 = new WaitForSeconds(_startDelay);

    yield return waitForSeconds1;
    _mainCamera.Priority--;

    for (int i = 0; i < _cameras.Length; i++)
    {
      _cameras[i].Priority++; 
      _canvases[i].gameObject.SetActive(true);

      if (i != 0)
        _cameras[i - 1].Priority--;

      yield return waitForSeconds;
    }

    _mainCamera.Priority++;
    _cameras[^1].Priority--;
    _isShowed = true;
    ES3.Save(SaveProgress.TitleKey.ShowIdleZone, _isShowed, SaveProgress.FilePath.IdleZoneTutorial);
  }
}