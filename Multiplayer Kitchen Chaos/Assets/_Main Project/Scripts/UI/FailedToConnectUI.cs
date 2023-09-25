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
            closeButton.gameObject.SetActive(false);
        });
    }

    private void Start()
    {
        MultiPlayerGameManager.Instance.OnFailedToConnect += MultiPlayerManager_OnFailedToConnect;
        GameLobbyManager.Instance.OnCreateLobbyStarted    += GameLobby_OnCreateLobbyStarted;
        GameLobbyManager.Instance.OnCreateLobbyFailed     += GameLobby_OnCreateLobbyFailed;
        GameLobbyManager.Instance.OnJoinStarted           += GameLobby_OnJoinStarted;
        GameLobbyManager.Instance.OnJoinFailed            += GameLobby_OnJoinFailed;
        GameLobbyManager.Instance.OnQuickJoinFailed       += GameLobby_OnQuickJoinFailed;

        gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
    }

    private void GameLobby_OnQuickJoinFailed()
    {
        ShowMessage("Could Not Find A Lobby To Join!",true);
    }

    private void GameLobby_OnJoinFailed()
    {
        ShowMessage("Failed To Join Lobby!", true);
    }

    private void GameLobby_OnJoinStarted()
    {
        ShowMessage("Joining Lobby..", false);
    }

    private void GameLobby_OnCreateLobbyFailed()
    {
        ShowMessage("Failed To Create Lobby", true);
    }

    private void GameLobby_OnCreateLobbyStarted()
    {
        ShowMessage("Creating Lobby..", false);
    }

    private void MultiPlayerManager_OnFailedToConnect(object sender, System.EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Failed To Connect!", true);
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason,true);
        }
    }

    private void ShowMessage(string _message,bool _buttonShow)
    {
        gameObject.SetActive(true);
        messageText.text = _message;

        closeButton.gameObject.SetActive(_buttonShow);
    }

    private void OnDestroy()
    {
        MultiPlayerGameManager.Instance.OnFailedToConnect -= MultiPlayerManager_OnFailedToConnect;
        GameLobbyManager.Instance.OnCreateLobbyStarted    -= GameLobby_OnCreateLobbyStarted;
        GameLobbyManager.Instance.OnCreateLobbyFailed     -= GameLobby_OnCreateLobbyFailed;
        GameLobbyManager.Instance.OnJoinStarted           -= GameLobby_OnJoinStarted;
        GameLobbyManager.Instance.OnJoinFailed            -= GameLobby_OnJoinFailed;
        GameLobbyManager.Instance.OnQuickJoinFailed       -= GameLobby_OnQuickJoinFailed;
    }
}
