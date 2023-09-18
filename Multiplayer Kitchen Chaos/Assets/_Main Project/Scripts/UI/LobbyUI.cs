using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button createGameButton;
    [SerializeField] private Button joinGameButton;


    private void Awake()
    {
        createGameButton.onClick.AddListener(() =>
        {
            MultiPlayerGameManager.Instance.StartHost();
            Loader.LoadNetworkScene(Loader.Scene.CharcterSelect);

        });
        
        joinGameButton.onClick.AddListener(() =>
        {
            MultiPlayerGameManager.Instance.StartClient();
        });
    }
}
