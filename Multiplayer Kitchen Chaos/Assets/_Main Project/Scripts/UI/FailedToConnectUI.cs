using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class FailedToConnectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void Start()
    {
        MultiPlayerGameManager.Instance.OnFailedToConnect += MultiPlayerManager_OnFailedToConnect;

        gameObject.SetActive(false);
    }

    private void MultiPlayerManager_OnFailedToConnect(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);

        messageText.text = NetworkManager.Singleton.DisconnectReason;

        if(messageText.text == "")
        {
            messageText.text = "Failed To Connect!";
        }
    }


    private void OnDestroy()
    {
        MultiPlayerGameManager.Instance.OnFailedToConnect -= MultiPlayerManager_OnFailedToConnect;
    }
}
