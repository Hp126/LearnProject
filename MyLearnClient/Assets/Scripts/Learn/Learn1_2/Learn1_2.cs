using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Learn1_2 : MonoBehaviour
{
    bool isOnLogining = false;

    private void Start()
    {
        isOnLogining = true;
        cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        DoLoginBegain(token);
    }

    void OnEnable()
    {
        EventCenter.AddListener("OnCancelLogin", OnCancelLogin, 0);
    }

    void OnDisable()
    {
        EventCenter.RemoveListener("OnCancelLogin", OnCancelLogin);
    }

    private void Update()
    {
        if(isOnLogining)
        {
            Debug.Log("ÕýÔÚµÇÂ¼");
        }

    }

    CancellationTokenSource cts;
    private void OnCancelLogin()
    {
        cts?.Cancel();
    }

    private async UniTask DoLoginBegain(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(2000, cancellationToken: token);

            Debug.LogError("Learn_1_2--- µÇÂ¼½áÊø");
            isOnLogining = false;
        }
        catch(OperationCanceledException)
        {
            Debug.LogError("Learn_1_2--- ÖÐ¶ÏµÇÂ¼");
            isOnLogining = false;
        }

        cts?.Dispose();
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = null;
    }

}
