using System.Collections;
using Cinemachine;
using MirraGames.SDK;
using UnityEngine;

public class IdleZoneTutorial : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _mainCamera;
    [SerializeField] private CinemachineVirtualCamera[] _cameras;
    [SerializeField] private Canvas[] _canvases;
    [SerializeField] private TutorialCameras _tutorialCameras;
    [SerializeField] private TutorialCanvases _tutorialCanvases;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Canvas _shopCanvas;
    [SerializeField] private Canvas _levelsCanvas;
    [SerializeField] private Canvas _gameCanvas;

    private float _delay = 3f;
    private float _startDelay = 0.5f;
    private bool _isShowed;
    private int _currentIndex;

    private void Start()
    {
        _isShowed = MirraSDK.Data.GetBool(SavableKeys.IsShowedTutorial);

        if (_isShowed)
            return;
        
        _tutorialCameras.gameObject.SetActive(true);
        _tutorialCanvases.gameObject.SetActive(true);

        StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_delay);
        WaitForSeconds waitForSeconds1 = new WaitForSeconds(_startDelay);
        
        LockGameplay();

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

        _tutorialCameras.gameObject.SetActive(false);
        _tutorialCanvases.gameObject.SetActive(false);
        
        MirraSDK.Data.SetBool(SavableKeys.IsShowedTutorial, _isShowed);
        
        yield return waitForSeconds1;
        
        UnlockGameplay();
    }

    private void LockGameplay()
    {
        _playerMovement.StopMoving();
        
        _shopCanvas.gameObject.SetActive(false);
        _levelsCanvas.gameObject.SetActive(false);
        _gameCanvas.gameObject.SetActive(false);
    }

    private void UnlockGameplay()
    {
        _playerMovement.StartMoving();
        
        _shopCanvas.gameObject.SetActive(true);
        _levelsCanvas.gameObject.SetActive(true);
        _gameCanvas.gameObject.SetActive(true);
    }
}