using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button JoinByCode;
    [SerializeField] private TMP_InputField codeInputField; 
    [SerializeField] private TMP_InputField playerNameInputField;

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
    }
}
