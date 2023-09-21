using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class ChracterSelectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        readyButton.onClick.AddListener(() =>
        {
            PlayerReadyManager.instance.CheckPlayersReady();
            gameObject.SetActive(false);
        });
        
        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            GameLobbyManager.Instance.LeaveLobby();
            Loader.LoadScene(Loader.Scene.MainMenu);
        });
    }

    private void Start()
    {
        Lobby _lobby = GameLobbyManager.Instance.GetJoinedLobby();
        lobbyNameText.text = "Lobby Name: " + _lobby.Name;
        lobbyCodeText.text = "Code: " + _lobby.LobbyCode;
    }
}
