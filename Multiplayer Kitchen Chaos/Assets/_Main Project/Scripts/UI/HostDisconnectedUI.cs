using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectedUI : MonoBehaviour
{
    [SerializeField] private Button MainMenuButton;

    private void Awake()
    {
        MainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.LoadScene(Loader.Scene.MainMenu);
        });  
    }

    void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;

        gameObject.SetActive(false);
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong _clientId)
    {
        if(_clientId == NetworkManager.ServerClientId)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if(NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        }
    }
}
