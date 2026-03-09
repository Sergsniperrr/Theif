using System;
using System.Collections;
using System.Collections.Generic;
using MirraGames.SDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SDKLoader : MonoBehaviour
{
    [SerializeField] private Bootstrap _bootstrap;

    private void Start()
    {
        MirraSDK.WaitForProviders(() =>
        {
            _bootstrap.gameObject.SetActive(true);
        });
    }
}
