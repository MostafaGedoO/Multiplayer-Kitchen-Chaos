using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerReadyManager : NetworkBehaviour
{
    private Dictionary<ulong, bool> playerIDReadyDictionary;
    public static PlayerReadyManager instance { get; private set; }

    public event EventHandler OnPlayersReadyChanged;

    private void Awake()
    {
        instance = this;
        playerIDReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void CheckPlayersReady()
    {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerIDReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
        SetplayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

        bool isAllPlayersReady = true;
        foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerIDReadyDictionary.ContainsKey(clientID) || !playerIDReadyDictionary[clientID])
            {
                isAllPlayersReady = false;
                break;
            }
        }

        if (isAllPlayersReady & NetworkManager.Singleton.ConnectedClientsIds.Count > 1)
        {
            Loader.LoadNetworkScene(Loader.Scene.MainGameScene);
        }
    }

    [ClientRpc]
    private void SetplayerReadyClientRpc(ulong _clientID)
    {
        playerIDReadyDictionary[_clientID] = true;

        OnPlayersReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReady(ulong _playerClientId)
    {
        return playerIDReadyDictionary.ContainsKey(_playerClientId) && playerIDReadyDictionary[_playerClientId];
    }
}
