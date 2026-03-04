using MirraGames.SDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSwitcher : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMobile;
    [SerializeField] private TMP_Text _textPC;
    [SerializeField] private Image _imageMobile;
    [SerializeField] private Image _imagePC;

    private void Awake()
    {
        ChangeActiveMobile(MirraSDK.Device.IsMobile);
    }

    private void ChangeActiveMobile(bool isActive)
    {
        _textMobile.gameObject.SetActive(isActive);
        _imageMobile.gameObject.SetActive(isActive);
        
        _textPC.gameObject.SetActive(!isActive);
        _imagePC.gameObject.SetActive(!isActive);
    }
}
