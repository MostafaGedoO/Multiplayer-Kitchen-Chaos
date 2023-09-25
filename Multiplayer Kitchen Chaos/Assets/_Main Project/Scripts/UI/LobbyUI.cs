using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button JoinByCode;
    [SerializeField] private TMP_InputField codeInputField; 
    [SerializeField] private TMP_InputField playerNameInputField;
    [Space]
    [SerializeField] private Transform contianer;
    [SerializeField] private Transform lobbyButtonTemplate;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            GameLobbyManager.Instance.LeaveLobby();
            Loader.LoadScene(Loader.Scene.MainMenu);
        });

        quickJoinButton.onClick.AddListener(() =>
        {
            GameLobbyManager.Instance.QuickJoinLobby();
        });

        JoinByCode.onClick.AddListener(() =>
        {
            if (codeInputField.text == "")
                return;

            GameLobbyManager.Instance.JoinLobbyByCode(codeInputField.text);
        });
    }

    private void Start()
    {
        playerNameInputField.text = MultiPlayerGameManager.Instance.GetPlayerName();

        playerNameInputField.onValueChanged.AddListener((x) => {
            MultiPlayerGameManager.Instance.SetPlayerName(playerNameInputField.text);
        });

        //Lobbies UI List
        GameLobbyManager.Instance.OnLobbiesListChanged += GameLobby_OnLobbiesListChanged;
        UpdateLobbiesList(new List<Lobby>());
        lobbyButtonTemplate.gameObject.SetActive(false);
    }

    private void GameLobby_OnLobbiesListChanged(List<Lobby> _lobbiesList)
    {
        UpdateLobbiesList(_lobbiesList);
    }

    private void UpdateLobbiesList(List<Lobby> _lobbiesList)
    {
        foreach(Transform _child in contianer)
        {
            if (_child == lobbyButtonTemplate) continue;
            Destroy(_child.gameObject);
        }

        foreach(Lobby _lobby in  _lobbiesList)
        {
            Transform _lobbyTranform =  Instantiate(lobbyButtonTemplate, contianer);
            _lobbyTranform.gameObject.SetActive(true);
            _lobbyTranform.GetComponent<LobbyButtonUI>().SetLobby(_lobby);
        }
    }

    private void OnDestroy()
    {
        GameLobbyManager.Instance.OnLobbiesListChanged -= GameLobby_OnLobbiesListChanged;
    }
}
