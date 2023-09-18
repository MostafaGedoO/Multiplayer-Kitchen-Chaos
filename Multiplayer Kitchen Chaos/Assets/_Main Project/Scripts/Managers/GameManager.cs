using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public static GameManager Instance { get; private set; }

    public enum State { WattingToStart, CountdownToStart, GamePlaying, GameOver }

    public event EventHandler OnGameStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;
    public event EventHandler OnLoaclPlayerReadyChanged;

    [SerializeField] private float gamePlayingTimerMax = 120f;
    private NetworkVariable<State> state = new NetworkVariable<State>(State.WattingToStart);
    private float wattingToStartTimer = 3f;
    private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(0f);
    private bool isLocalPlayerReady;

    private bool isGamePaused;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += OnStateValueChanged;
        if(IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += NetworkSceneManager_OnLoadEventCompleted;
        }
    }

    private void NetworkSceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            GameObject _player = Instantiate(playerPrefab);
            _player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }

        state.Value = State.CountdownToStart;
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
            countdownToStartTimer.Value -= Time.deltaTime;
            if(countdownToStartTimer.Value < 0f)
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
        return countdownToStartTimer.Value;
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
    }


    public bool GetIsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }
}

