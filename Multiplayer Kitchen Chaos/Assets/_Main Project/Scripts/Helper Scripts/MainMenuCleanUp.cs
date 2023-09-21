using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanUp : MonoBehaviour
{
    private void Awake()
    {
        if(NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if(MultiPlayerGameManager.Instance != null)
        {
            Destroy(MultiPlayerGameManager.Instance.gameObject);
        }

        if(GameLobbyManager.Instance != null)
        {
            Destroy(GameLobbyManager.Instance.gameObject);
        }
    }
}
