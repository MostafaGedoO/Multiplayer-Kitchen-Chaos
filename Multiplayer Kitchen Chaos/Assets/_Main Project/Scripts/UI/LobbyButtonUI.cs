using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    private Button button;

    private Lobby lobby;


    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            GameLobbyManager.Instance.JoinLobbyById(lobby.Id);
        });
    }

    public void SetLobby(Lobby _lobby)
    {
        lobbyNameText.text = _lobby.Name;
        lobby = _lobby;
    }
}
