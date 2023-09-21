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

        //Sending a heartBeat to keep the lobby visable (lobby goes invisacble after 30s)
        InvokeRepeating("SetHeartBeat", 20, 20);
    }

    private async void InitializeLobbyServices()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized) // Initialize only one time
        {
            InitializationOptions _options = new InitializationOptions();
            _options.SetProfile(Random.Range(0, 1000).ToString()); //defferant profiles to be able to test in the same pc with multible builds

            await UnityServices.InitializeAsync(_options);

            await AuthenticationService.Instance.SignInAnonymouslyAsync(); //The clock on Pc mush match the world Clock
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

    public async void DeleteLobby()
    {
        try
        {
            if(joinedLobby != null)
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

                joinedLobby = null;
            }
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            if (joinedLobby != null)
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id,AuthenticationService.Instance.PlayerId);
                joinedLobby = null;
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }

    public async void KickPlayer(string _playerID)
    {
        try
        {
            if(IsLobbyHost())
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, _playerID);
            }
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }
}
