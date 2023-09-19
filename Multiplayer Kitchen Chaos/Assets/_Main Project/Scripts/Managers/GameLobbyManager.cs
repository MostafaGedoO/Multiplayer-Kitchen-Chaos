using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameLobbyManager : MonoBehaviour
{
    public static GameLobbyManager Instance { get; private set; }
    private int maxPlayerNumber = 4;
    private Lobby joinedLobby;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        InitializeLobbyServices();

        InvokeRepeating("SetHeartBeat", 20, 20);
    }

    private async void InitializeLobbyServices()
    {
        try
        {

            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                InitializationOptions _options = new InitializationOptions();
                _options.SetProfile(Random.Range(0, 1000).ToString());

                await UnityServices.InitializeAsync(_options);

                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        catch (AuthenticationException e)
        {
            Debug.LogError(e.Message);
        }
    } 

    public async void CreateLobby(string _lobbyName, bool _isPrivate)
    {
        try
        {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(_lobbyName, maxPlayerNumber, new CreateLobbyOptions{ IsPrivate = _isPrivate });
            
            MultiPlayerGameManager.Instance.StartHost();
            Loader.LoadNetworkScene(Loader.Scene.CharcterSelect);
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }

    public async void QuickJoinLobby()
    {
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            MultiPlayerGameManager.Instance.StartClient();
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }

    public async void JoinLobbyByCode(string _code)
    {
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(_code);

            MultiPlayerGameManager.Instance.StartClient();
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }

    public Lobby GetJoinedLobby()
    {
        return joinedLobby;
    }

    private void SetHeartBeat()
    {
        if(IsLobbyHost())
        {
            LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
        }
    }

    private bool IsLobbyHost()
    {
        if(joinedLobby == null)
        {
            return false;
        }
        else
        {
           return joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
        }
    }
}
