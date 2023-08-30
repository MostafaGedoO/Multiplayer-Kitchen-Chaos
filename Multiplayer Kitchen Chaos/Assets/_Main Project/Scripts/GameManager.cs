using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

    public enum State { WattingToStart, CountdownToStart, GamePlaying, GameOver }


    public event EventHandler OnGameStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;


    [SerializeField] private float gamePlayingTimerMax = 120f;
    private State state;
    private float wattingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;


    private bool isGamePaused;
 
    private void Awake()
    {
        Instance = this;
        state = State.WattingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePausedGame();
    }

    private void Update()
    {
        if(state == State.WattingToStart)
        {
            wattingToStartTimer -= Time.deltaTime;
            if(wattingToStartTimer < 0f)
            {
                state = State.CountdownToStart;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        else if(state == State.CountdownToStart)
        {
            countdownToStartTimer -= Time.deltaTime;
            if(countdownToStartTimer < 0f)
            {
                state = State.GamePlaying;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                gamePlayingTimer = gamePlayingTimerMax;
            }
        }
        else if( state == State.GamePlaying)
        {
            gamePlayingTimer -= Time.deltaTime;
            if(gamePlayingTimer < 0f)
            {
                state = State.GameOver;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        else if(state == State.GameOver)
        {

        }
    }


    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownState()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownTimer()
    {
        return countdownToStartTimer;
    }

    public bool IsGameOverStete()
    {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return gamePlayingTimer / gamePlayingTimerMax;
    }

    public void TogglePausedGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1;
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
    }
}

