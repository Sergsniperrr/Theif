using System;
using System.Net;
using MirraGames.SDK;
using MirraGames.SDK.Common;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[DisallowMultipleComponent]
[RequireComponent(typeof(TMP_Text))]
public sealed class LocalizedTMPText : MonoBehaviour
{
    [Title("Localized TMP")] [SerializeField, TextArea(2, 8), LabelText("RU")]
    private string _ru;

    [SerializeField, TextArea(2, 8), LabelText("EN")]
    private string _en;

    [SerializeField, TextArea(2, 8), LabelText("TR")]
    private string _tr;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _text.text = GetTextLanguage();
    }

    private string GetTextLanguage()
    {
        LanguageType lang = MirraSDK.Language.Current;

        return lang switch
        {
            LanguageType.Russian => _ru,
            LanguageType.Turkish => _tr,
            _ => _en
        };
    }

#if UNITY_EDITOR
    [ShowInInspector, ReadOnly, PropertyOrder(100)] [LabelText("Translate status")]
    private string _processTranslateLabel;

    private void Reset()
    {
        TMP_Text text = GetComponent<TMP_Text>();

        if (text == null)
            return;

        if (string.IsNullOrEmpty(_ru) && string.IsNullOrEmpty(text.text) == false)
            _ru = text.text;
    }

    [Button("Auto Translate (RU → EN/TR)", ButtonSizes.Medium), PropertyOrder(101)]
    [GUIColor(0.35f, 0.8f, 0.35f)]
    [LabelText("Auto Translate (RU → EN/TR)")]
    private void AutoTranslate()
    {
        if (string.IsNullOrWhiteSpace(_ru))
        {
            _processTranslateLabel = "RU is empty";
            return;
        }

        int translated = 0;
        _processTranslateLabel = "Processing...";

        try
        {
            if (string.IsNullOrWhiteSpace(_en))
            {
                _en = TranslateGoogle(_ru, "en");
                translated++;
            }

            if (string.IsNullOrWhiteSpace(_tr))
            {
                _tr = TranslateGoogle(_ru, "tr");
                translated++;
            }

            _processTranslateLabel = $"completed ({translated})";
        }
        catch (Exception exception)
        {
            _processTranslateLabel = "Error";
            Debug.LogError(exception);
        }
    }

    private static string TranslateGoogle(string sourceText, string translationTo)
    {
        sourceText = sourceText.Replace("\r\n", "\n");

        string url = string.Format(
            "https://translate.google.com/translate_a/single?client=gtx&dt=t&sl={0}&tl={1}&q={2}",
            "auto",
            translationTo,
            WebUtility.UrlEncode(sourceText)
        );

        using UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.SendWebRequest();

        while (webRequest.isDone == false)
        {
        }

        if (webRequest.result != UnityWebRequest.Result.Success)
            throw new InvalidOperationException($"Translate failed: {webRequest.error}");

        string response = webRequest.downloadHandler.text;

        string translatedText = ExtractGoogleTranslateText(response);

        if (string.IsNullOrEmpty(translatedText))
            throw new InvalidOperationException("Translate returned empty result.");

        return translatedText;
    }

    [Button(ButtonSizes.Medium), PropertyOrder(200)]
    [GUIColor(0.9f, 0.35f, 0.35f)]
    [LabelText("Clear All Texts")]
    private void ClearAll()
    {
        _ru = string.Empty;
        _en = string.Empty;
        _tr = string.Empty;

        _processTranslateLabel = "Cleared";
    }

    private static string ExtractGoogleTranslateText(string response)
    {
        int firstQuote = response.IndexOf("\"", StringComparison.Ordinal);
        if (firstQuote < 0)
            return null;

        int secondQuote = response.IndexOf("\"", firstQuote + 1, StringComparison.Ordinal);
        if (secondQuote < 0)
            return null;

        string text = response.Substring(firstQuote + 1, secondQuote - firstQuote - 1);

        return text.Replace("\\n", "\n").Replace("\\\"", "\"");
    }

#endif
}