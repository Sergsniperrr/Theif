using UnityEngine;

namespace JTLeaderboard.Scripts
{
    public static class CanvasGroupExtensions
    {
        public static void SetVisible(this CanvasGroup canvasGroup, bool visible)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}