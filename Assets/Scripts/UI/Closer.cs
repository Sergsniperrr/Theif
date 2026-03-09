using System;
using UnityEngine;
using UnityEngine.UI;

public class Closer : MonoBehaviour
{
    [SerializeField] private Button _exitButton;
    
    public event Action Closed;

    private void OnEnable()
    {
        if (_exitButton != null)
            _exitButton.onClick.AddListener(InvokeClose);
    }

    private void OnDisable()
    {
        if (_exitButton != null)
            _exitButton.onClick.RemoveListener(InvokeClose);
    }
    
    public void InvokeClose()
    {
        Closed?.Invoke();
    }
}
