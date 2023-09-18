using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaittingForPlayersUI : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        gameObject.SetActive(true);
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if(GameManager.Instance.IsCountdownState())
        {
            gameObject.SetActive(false);
        }
    }

}
