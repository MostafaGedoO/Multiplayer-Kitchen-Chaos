using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    private void Start()
    {
        MultiPlayerGameManager.Instance.OnTryingToConnect += MulityPlayerManager_OnTryingToConnect;
        MultiPlayerGameManager.Instance.OnFailedToConnect += MultiPlayerManager_OnFailedToConnect;

        gameObject.SetActive(false);
    }

    private void MultiPlayerManager_OnFailedToConnect(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void MulityPlayerManager_OnTryingToConnect(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        MultiPlayerGameManager.Instance.OnTryingToConnect -= MulityPlayerManager_OnTryingToConnect;
        MultiPlayerGameManager.Instance.OnFailedToConnect -= MultiPlayerManager_OnFailedToConnect;
    }
}
