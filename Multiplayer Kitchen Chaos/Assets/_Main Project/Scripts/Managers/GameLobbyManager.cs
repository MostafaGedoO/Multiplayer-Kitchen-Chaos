using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

public class GameLobbyManager : MonoBehaviour
{
    public static GameLobbyManager Instance { get; private set; }
    private int maxPlayerNumber = 4;
    private Lobby joinedLobby;
    private const string RELAY_JOIN_CODE_KEY = "RelayJoinCode";
    private const string CONNECTION_PROTOCAL = "udp";

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

    #region Lobby Functions
    
    public async void CreateLobby(string _lobbyName, bool _isPrivate)
    {
        try
        {
            //Creating the lobby
            OnCreateLobbyStarted?.Invoke();
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(_lobbyName, maxPlayerNumber, new CreateLobbyOptions{ IsPrivate = _isPrivate });

            //Allocating the relay
            Allocation _allocation = await AllocateRelay();
            
            //Getting the relay code and pass it to lobby data
            string relayJoinCode = await GetRelayJoinCode(_allocation);

            await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {RELAY_JOIN_CODE_KEY,new DataObject(DataObject.VisibilityOptions.Member,relayJoinCode)}
                }
            });

            //Set the relay server data in unity transport
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(_allocation, "dtls"));

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
            //Joining the lobby
            OnJoinStarted?.Invoke();
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            //Getting the relay join code and join it
            string _relayJoinCode = joinedLobby.Data[RELAY_JOIN_CODE_KEY].Value;
            JoinAllocation _joinAllocation = await JoinRelayByCode(_relayJoinCode);

            ////Set the relay server data in unity transport
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(_joinAllocation, CONNECTION_PROTOCAL));

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
            //joining lobby
            OnJoinStarted?.Invoke();
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(_code);

            //Getting the relay join code and join it
            string _relayJoinCode = joinedLobby.Data[RELAY_JOIN_CODE_KEY].Value;
            JoinAllocation _joinAllocation = await JoinRelayByCode(_relayJoinCode);

            //Setting the unity transport server data
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(_joinAllocation, CONNECTION_PROTOCAL));

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
            //joining lobby
            OnJoinStarted?.Invoke();
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(_lobbyId);

            //Getting the relay join code and join it
            string _relayJoinCode = joinedLobby.Data[RELAY_JOIN_CODE_KEY].Value;
            JoinAllocation _joinAllocation = await JoinRelayByCode(_relayJoinCode);

            //Setting the unity transport server data
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(_joinAllocation, CONNECTION_PROTOCAL));

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
        if(joinedLobby == null && AuthenticationService.Instance.IsSignedIn && SceneManager.GetActiveScene().name == Loader.Scene.Lobby.ToString())
        {
            ListAviliableLobbies();
        }
    }

    #endregion

    #region Relay Functions

    //Create Relay Allocation this is called befor starting the host
    private async Task<Allocation> AllocateRelay()
    {
        try
        {
            Allocation _allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayerNumber);
            return _allocation;
        }
        catch(RelayServiceException e)
        {
            Debug.LogError(e.Message);
            return default;
        }
    }

    private async Task<string> GetRelayJoinCode(Allocation _allocation)
    {
        try
        {
            string relayCode = await RelayService.Instance.GetJoinCodeAsync(_allocation.AllocationId);
            return relayCode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e.Message);
            return default;
        }
    }

    //joining Relay With Code this is called before starting the client
    private async Task<JoinAllocation> JoinRelayByCode(string _relayCode)
    {
        try
        {
            JoinAllocation _joindAllocation = await RelayService.Instance.JoinAllocationAsync(_relayCode);
            return _joindAllocation;
        }
        catch(RelayServiceException ex)
        {
            Debug.LogError(ex.Message);
            return default;
        }
    }

    #endregion
}
