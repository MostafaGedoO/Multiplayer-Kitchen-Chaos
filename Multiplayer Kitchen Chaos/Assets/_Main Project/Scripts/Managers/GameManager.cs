using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
   public static GameManager Instance { get; private set; }

    public enum State { WattingToStart, CountdownToStart, GamePlaying, GameOver }


    public event EventHandler OnGameStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;
    public event EventHandler OnLoaclPlayerReadyChanged;


    [SerializeField] private float gamePlayingTimerMax = 120f;
    private NetworkVariable<State> state = new NetworkVariable<State>(State.WattingToStart);
    private float countdownToStartTimer = 3f;
    private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(0f);
    private bool isLocalPlayerReady;

    private bool isGamePaused;

    private Dictionary<ulong, bool> playerIDReadyDictionary;

    private void Awake()
    {
        Instance = this;
        playerIDReadyDictionary = new Dictionary<ulong, bool>();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += OnStateValueChanged;
    }

    private void OnStateValueChanged(State previousValue, State newValue)
    {
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePausedGame();
    }

    private void Update()
    {
        if (!IsServer) return;

        if(state.Value == State.CountdownToStart)
        {
            countdownToStartTimer -= Time.deltaTime;
            if(countdownToStartTimer < 0f)
            {
                state.Value = State.GamePlaying;
                gamePlayingTimer.Value = gamePlayingTimerMax;
            }
        }
        else if( state.Value == State.GamePlaying)
        {
            gamePlayingTimer.Value -= Time.deltaTime;
            if(gamePlayingTimer.Value < 0f)
            {
                state.Value = State.GameOver;
            }
        }
        else if(state.Value == State.GameOver)
        {

        }
    }


    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }

    public bool IsCountdownState()
    {
        return state.Value == State.CountdownToStart;
    }  
    
    public bool IsWattingToStartState()
    {
        return state.Value == State.WattingToStart;
    }

    public float GetCountdownTimer()
    {
        return countdownToStartTimer;
    }

    public bool IsGameOverStete()
    {
        return state.Value == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return gamePlayingTimer.Value / gamePlayingTimerMax;
    }

    public void TogglePausedGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            //Time.timeScale = 0;
            //OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1;
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public void LoaclPlayerReady()
    {
        isLocalPlayerReady = true;
        OnLoaclPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
        
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerIDReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool isAllPlayersReady = true;
        foreach(ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(!playerIDReadyDictionary.ContainsKey(clientID) || !playerIDReadyDictionary[clientID])
            {
                isAllPlayersReady = false;
                break;
            }
        }

        if(isAllPlayersReady & NetworkManager.Singleton.ConnectedClientsIds.Count > 1)
        {
            state.Value = State.CountdownToStart;
        }
    }


    public bool GetIsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }
}

