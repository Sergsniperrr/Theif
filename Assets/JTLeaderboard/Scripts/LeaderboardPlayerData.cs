using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace JTLeaderboard.Scripts
{
    public sealed class LeaderboardPlayerData : MonoBehaviour
    {
        [SerializeField] private TMP_Text _positionText;
        [SerializeField] private RawImage _avatarImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _scoreText;

        public void Set(int position, string displayName, int score)
        {
            if (_positionText != null) 
                _positionText.text = position.ToString();

            if (_nameText != null)
                _nameText.text = displayName;
            
            if (_scoreText != null) 
                _scoreText.text = score.ToString();
        }
        
        public void LoadAvatar(string url)
        {
            if (_avatarImage == null)
                return;

            ClearAvatar();

            if (string.IsNullOrEmpty(url))
                return;

            StartCoroutine(LoadTexture(url));
        }

        private void SetAvatar(Texture texture)
        {
            if (_avatarImage == null) 
                return;
            
            _avatarImage.texture = texture;
            
            _avatarImage.enabled = texture != null;
        }

        private void ClearAvatar()
        {
            if (_avatarImage == null)
                return;
            
            _avatarImage.texture = null;
            _avatarImage.enabled = false;
        }

        private IEnumerator LoadTexture(string url)
        {
            using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                yield break;

            Texture texture = DownloadHandlerTexture.GetContent(request);
            SetAvatar(texture);
        }
    }
}