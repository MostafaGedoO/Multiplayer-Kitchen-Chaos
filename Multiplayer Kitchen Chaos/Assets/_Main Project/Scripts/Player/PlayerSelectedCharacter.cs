using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSelectedCharacter : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyText;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private TextMeshPro playerNameText;

    private void Start()
    {
        MultiPlayerGameManager.Instance.OnPlayerDataListChanged += MultiplayerManager_OnPlayerDataListChanged;
        PlayerReadyManager.instance.OnPlayersReadyChanged += playerReadyManager_OnPlayersReadyChanged;
        
        UpdatePlayers();
    }

    private void playerReadyManager_OnPlayersReadyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayers();
    }

    private void MultiplayerManager_OnPlayerDataListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayers();
    }

    private void UpdatePlayers()
    {
        if(MultiPlayerGameManager.Instance.IsPlayersListHasPlayerIndex(playerIndex))
        {
            gameObject.SetActive(true);

            PlayerData _playerDate = MultiPlayerGameManager.Instance.GetPlayerData(playerIndex);

            playerNameText.text = _playerDate.playerName.ToString();

            readyText.SetActive(PlayerReadyManager.instance.IsPlayerReady(_playerDate.playerClientId));

            playerVisual.SetPlayerColor(MultiPlayerGameManager.Instance.GetAColorFromList(_playerDate.colorId));
        }
        else
        {
            gameObject.SetActive(false);
            readyText.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        MultiPlayerGameManager.Instance.OnPlayerDataListChanged -= MultiplayerManager_OnPlayerDataListChanged;
    }
}
