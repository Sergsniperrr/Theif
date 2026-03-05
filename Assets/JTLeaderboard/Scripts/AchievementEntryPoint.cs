using UnityEngine;

namespace JTLeaderboard.Scripts
{
    public class AchievementEntryPoint : MonoBehaviour
    {
        [Header("Handlers")]
        [SerializeField] private LeaderboardHandler _leaderboardHandler;
        [SerializeField] private AuthorizationHandler _authorizationHandler;

        private void Start()
        {
            _leaderboardHandler.Initialize(_authorizationHandler);
        }
    }
}