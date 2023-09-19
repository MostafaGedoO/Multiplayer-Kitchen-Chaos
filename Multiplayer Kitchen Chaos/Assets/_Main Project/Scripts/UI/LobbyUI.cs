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

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
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
}
