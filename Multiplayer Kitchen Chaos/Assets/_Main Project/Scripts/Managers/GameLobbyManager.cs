using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    //Events
    public event Action OnCreateLobbyStarted;
    public event Action OnCreateLobbyFailed;
    public event Action OnJoinStarted;
    public event Action OnJoinFailed;
    public event Action OnQuickJoinFailed;

    //Lobby Event
    public event Action<List<Lobby>> OnLobbiesListChanged;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        InitializeLobbyServices();

        //Sending a heartBeat to keep the lobby visable (lobby goes invisacble after 30s)
        InvokeRepeating("SetHeartBeat", 20, 20);

        //Refreshing the lobby List every 5 seconds
        InvokeRepeating("HandleListingLobbies", 1, 5);
    }

    private async void InitializeLobbyServices()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized) // Initialize only one time
        {
            InitializationOptions _options = new InitializationOptions();
            _options.SetProfile(UnityEngine.Random.Range(0, 1000).ToString()); //defferant profiles to be able to test in the same pc with multible builds

            await UnityServices.InitializeAsync(_options);

            await AuthenticationService.Instance.SignInAnonymouslyAsync(); //The clock on Pc mush match the world Clock
        }
    }

    public async void CreateLobby(string _lobbyName, bool _isPrivate)
    {
        try
        {
            OnCreateLobbyStarted?.Invoke();

            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(_lobbyName, maxPlayerNumber, new CreateLobbyOptions{ IsPrivate = _isPrivate });
            
            //Start Host
            MultiPlayerGameManager.Instance.StartHost();
            Loader.LoadNetworkScene(Loader.Scene.CharcterSelect);
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e.Message);
            OnCreateLobbyFailed?.Invoke();
        }
    }

    public async void QuickJoinLobby()
    {
        try
        {
            OnJoinStarted?.Invoke();

            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            //Start Client
            MultiPlayerGameManager.Instance.StartClient();
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e.Message);
            OnQuickJoinFailed?.Invoke();
        }
    }

    public async void JoinLobbyByCode(string _code)
    {
        try
        {
            OnJoinStarted?.Invoke();

            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(_code);

            //Start Client
            MultiPlayerGameManager.Instance.StartClient();
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e.Message);
            OnJoinFailed?.Invoke();
        }
    }
    
    public async void JoinLobbyById(string _lobbyId)
    {
        try
        {
            OnJoinStarted?.Invoke();

            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(_lobbyId);

            //Start Client
            MultiPlayerGameManager.Instance.StartClient();
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e.Message);
            OnJoinFailed?.Invoke();
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

    private async void ListAviliableLobbies()
    {
        try
        {
            //Filtering the Lobby Search Fot only lobbies with aviailable slots
            QueryLobbiesOptions _queryLobbiesOptions = new QueryLobbiesOptions()
            {
                Filters = new List<QueryFilter>()
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
                }
            };

            QueryResponse _queryResponce = await LobbyService.Instance.QueryLobbiesAsync(_queryLobbiesOptions);

            //Firing Event With the Lobbies List
            OnLobbiesListChanged?.Invoke(_queryResponce.Results);
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void HandleListingLobbies()
    {
        if(joinedLobby == null & AuthenticationService.Instance.IsSignedIn)
        {
            ListAviliableLobbies();
        }
    }

}
