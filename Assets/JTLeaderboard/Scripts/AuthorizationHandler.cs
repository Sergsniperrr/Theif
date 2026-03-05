using MirraGames.SDK;
using UnityEngine;
using UnityEngine.UI;

namespace JTLeaderboard.Scripts
{
    public class AuthorizationHandler : MonoBehaviour
    {
        [SerializeField] private Button _authButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        private void OnEnable()
        {
            _authButton.onClick.AddListener(OnAuthClicked);
            _closeButton.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            _authButton.onClick.RemoveListener(OnAuthClicked);
            _closeButton.onClick.RemoveListener(Hide);
        }

        public void OpenAuthPanel()
        {
            Show();
        }

        private void OnAuthClicked()
        {
            MirraSDK.Player.InvokeLogin();
            Hide();
        }

        private void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}