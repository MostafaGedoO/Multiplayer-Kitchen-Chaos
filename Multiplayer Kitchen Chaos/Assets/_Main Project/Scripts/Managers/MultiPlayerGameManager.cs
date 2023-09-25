using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiPlayerGameManager : NetworkBehaviour
{
    [SerializeField] private KitchenObjectListSO kitchenObjectListSO;
    [SerializeField] private List<Color> ColorList;

    public event EventHandler OnTryingToConnect;
    public event EventHandler OnFailedToConnect;
    public event EventHandler OnPlayerDataListChanged;

    public static MultiPlayerGameManager Instance { get; private set; }

    private NetworkList<PlayerData> PlayersDataList;

    //Player Name
    private const string PLAYER_NAME_KEY = "PlayerName";
    private string playerName;

    //Lobby Name
    private const string LOBBY_NAME_KEY = "LobbyName";
    private string lobbyName;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        playerName = PlayerPrefs.GetString(PLAYER_NAME_KEY,"Player" + UnityEngine.Random.Range(100,1000));

        lobbyName = PlayerPrefs.GetString(LOBBY_NAME_KEY,"Lobby"+ UnityEngine.Random.Range(100,1000));

        PlayersDataList = new NetworkList<PlayerData>();

        PlayersDataList.OnListChanged += PlayersDataList_OnListChanged;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public void SetPlayerName(string _playerName)
    {
        playerName = _playerName;
        PlayerPrefs.SetString(PLAYER_NAME_KEY, playerName);
    }

    public string GetLobbyName()
    {
        return lobbyName;
    }

    public void SetLobbyName(string _lobbyName)
    {
        lobbyName = _lobbyName;
        PlayerPrefs.SetString(LOBBY_NAME_KEY ,lobbyName);
    }

    private void PlayersDataList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartHost()
    {
        OnTryingToConnect?.Invoke(this, EventArgs.Empty);
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManagerConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong _clientId)
    {
        for(int i = 0; i < PlayersDataList.Count; i++)
        {
            PlayerData _playerData = PlayersDataList[i];
            if(_playerData.playerClientId == _clientId)
            {
                //This ID Is Disconnected
                PlayersDataList.RemoveAt(i);
            }
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong _clientId)
    {
        PlayersDataList.Add(new PlayerData
        {
            playerClientId = _clientId,
            colorId = GetFirstUnusedColorID(),
        });
        SetPlayerNameServerRpc(GetPlayerName());
    }

    private void NetworkManagerConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if(SceneManager.GetActiveScene().name != Loader.Scene.CharcterSelect.ToString())
        {
            response.Approved = false;
            response.Reason = "Game has already started";
            return;
        }

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= 4)
        {
            response.Approved = false;
            response.Reason = "Game is full";
            return;
        }

        response.Approved = true;
       
    }

    public void StartClient()
    {
        OnTryingToConnect?.Invoke(this, EventArgs.Empty);
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += Network_OnClientConnectedCallback; ;
        NetworkManager.Singleton.StartClient();
    }

    private void Network_OnClientConnectedCallback(ulong obj)
    {
        SetPlayerNameServerRpc(GetPlayerName());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc(string _playerName , ServerRpcParams _serverRpcParams = default)
    {
        int _playerDataIndex = GetPlayerDataIndexFromClientID(_serverRpcParams.Receive.SenderClientId);

        PlayerData _playerData = PlayersDataList[_playerDataIndex];

        _playerData.playerName = _playerName;

        PlayersDataList[_playerDataIndex] = _playerData;
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong obj)
    {
        OnFailedToConnect?.Invoke(this, EventArgs.Empty);
    }

    public void SpownKitchenObject(KitchenObjectSO _kitchenObjectSO, IKitchenObjectParent _kitchenObjectParent)
    {
        SpownKitchenObjectServerRpc(GetKitchenObjectSOIndexFromList(_kitchenObjectSO),_kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpownKitchenObjectServerRpc(int _kitchenObjectSOIndex, NetworkObjectReference _kitchenObjectParentNetworkObjectReference)
    {
        //Instantiate the kitchen object and then spawn it over the network
        GameObject _kitchenObject = Instantiate(GetKitchenObjectSOFromList(_kitchenObjectSOIndex).ObjectPrefab);
        _kitchenObject.GetComponent<NetworkObject>().Spawn(true);

        //Getting the IKitchenObjectParent from a network object refernce
        _kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject _kitchenObjectParentNetworkObject);
        IKitchenObjectParent _kitchenObjectParent = _kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        //Getting the kitchen object and setting the parent
        KitchenObject _spownedKitchenObject = _kitchenObject.GetComponent<KitchenObject>();
        _spownedKitchenObject.SetKitchenObjectParent(_kitchenObjectParent);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyKitchenObjectServerRpc(NetworkObjectReference _kitchenObjectNetworkObjectRefernce) 
    {
        _kitchenObjectNetworkObjectRefernce.TryGet(out NetworkObject _kitchenObjectNetworkObject);
        KitchenObject _kitchenObject = _kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        ClearKitchenObjectOnParentClientRpc(_kitchenObjectNetworkObjectRefernce);
        _kitchenObject.DestroySelf();
    }

    [ClientRpc]
    private void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference _kitchenObjectNetworkObjectRefernce)
    {
        _kitchenObjectNetworkObjectRefernce.TryGet(out NetworkObject _kitchenObjectNetworkObject);
        KitchenObject _kitchenObject = _kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        _kitchenObject.ClearKitchenObjectOnParent();
    }

    public int GetKitchenObjectSOIndexFromList(KitchenObjectSO _kitchenObjectSO)
    {
        return kitchenObjectListSO.kitchenObjectSOList.IndexOf(_kitchenObjectSO);
    }

    public KitchenObjectSO GetKitchenObjectSOFromList(int _index)
    {
        return kitchenObjectListSO.kitchenObjectSOList[_index];
    }

    public bool IsPlayersListHasPlayerIndex(int index)
    {
        return index < PlayersDataList.Count;
    }

    public PlayerData GetPlayerData(int index) 
    { 
        return PlayersDataList[index];
    }

    public Color GetAColorFromList(int _colorId)
    {
        return ColorList[_colorId];
    }

    public PlayerData GetLocalPlayerDate()
    {
        return GetPlayerDataFromClientID(NetworkManager.Singleton.LocalClientId);
    }

    public PlayerData GetPlayerDataFromClientID(ulong  _clientid)
    {
        foreach(PlayerData _playerDate in PlayersDataList)
        {
            if(_clientid == _playerDate.playerClientId)
            {
                return _playerDate;
            }
        }
        return default;
    } 
    
    public int GetPlayerDataIndexFromClientID(ulong  _clientid)
    {
        for(int i = 0;i< PlayersDataList.Count; i++)
        {
            if (PlayersDataList[i].playerClientId == _clientid)
            {
                return i;
            }
        }
        return -1;
    }


    public void ChangePlayerColor(int _colorId)
    {
        ChangePlayerColorServerRpc(_colorId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangePlayerColorServerRpc(int _colorId,ServerRpcParams _serverRpcPrams = default)
    {
        if (!IsColorAvailable(_colorId)) return;

        int _playerDataIndex = GetPlayerDataIndexFromClientID(_serverRpcPrams.Receive.SenderClientId);

        PlayerData _playerData = PlayersDataList[_playerDataIndex];

        _playerData.colorId = _colorId;

        PlayersDataList[_playerDataIndex] = _playerData;
    }

    private bool IsColorAvailable(int _colorId)
    {
        foreach(PlayerData _playerData in PlayersDataList)
        {
            if(_playerData.colorId == _colorId)
            {
                //Color is Used
                return false;
            }
        }
        return true;
    }

    private int GetFirstUnusedColorID()
    {
        for (int i = 0; i < ColorList.Count; i++)
        {
            if (IsColorAvailable(i))
            {
                return i;
            }
        }
        return -1;
    }
}
