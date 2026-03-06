using MirraGames.SDK;
using MirraGames.SDK.Common;
using UnityEngine;
using UnityEngine.UI;

namespace JTLeaderboard.Scripts
{
    public class LeaderboardHandler : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private int _topCount = 5;

        [Header("UI")] 
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Player Data")]
        [SerializeField]
        private Transform _contentHolder;

        [SerializeField] private LeaderboardPlayerData _leaderboardPlayerDataPrefab;

        private AuthorizationHandler _authorizationHandler;
        
        public void Initialize(AuthorizationHandler authorizationHandler)
        {
            _authorizationHandler = authorizationHandler;
        }
        
        private void OnEnable()
        {
            _openButton.onClick.AddListener(OnOpenClicked);
            _closeButton.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            _openButton.onClick.RemoveListener(OnOpenClicked);
            _closeButton.onClick.RemoveListener(Hide);
        }

        private void OnOpenClicked()
        {
#if UNITY_EDITOR
            Show();
            LoadTop();
            return;
#endif

            if (MirraSDK.Player.IsLoggedIn == false)
            {
                _authorizationHandler.OpenAuthPanel();
                return;
            }

            Show();
            LoadTop();
        }

        private void Show()
        {
            _canvasGroup.SetVisible(true);
            GameStoper.Stop();
        }

        private void Hide()
        {
            _canvasGroup.SetVisible(false);
            GameStoper.Restart();
        }

        private void LoadTop()
        {
            if (_contentHolder == null || _leaderboardPlayerDataPrefab == null)
                return;

            ClearPlayerData();

#if UNITY_EDITOR
            SpawnFakeData();
#else
    LoadFromSDK();
#endif
        }

        private void ClearPlayerData()
        {
            for (int i = _contentHolder.childCount - 1; i >= 0; i--)
                Destroy(_contentHolder.GetChild(i).gameObject);
        }

        private void SpawnFakeData()
        { 
            const int MinFakeScore = 1000; 
            const int MaxFakeScore = 10000;
            
            for (int i = 0; i < _topCount; i++)
            {
                var playerData = Instantiate(_leaderboardPlayerDataPrefab, _contentHolder);

                int position = i + 1;
                string name = $"Player {position}";
                int score = Random.Range(MinFakeScore, MaxFakeScore);

                playerData.Set(position, name, score);
            }
        }

        private void LoadFromSDK()
        {
            MirraSDK.Achievements.GetLeaderboard(LeaderboardParams.Name, leaderboard =>
            {
                if (leaderboard?.players == null)
                    return;

                int count = Mathf.Min(_topCount, leaderboard.players.Length);

                for (int i = 0; i < count; i++)
                {
                    PlayerScore playerScore = leaderboard.players[i];

                    LeaderboardPlayerData playerData = Instantiate(_leaderboardPlayerDataPrefab, _contentHolder);

                    playerData.Set(playerScore.position, playerScore.displayName, playerScore.score);

                    playerData.LoadAvatar(playerScore.profilePictureUrl);
                }
            });
        }
    }
}