using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;

        gameObject.SetActive(false);
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if(GameManager.Instance.IsCountdownState())
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(GameManager.Instance.IsCountdownState() )
        {
            timerText.text = (Mathf.Ceil(GameManager.Instance.GetCountdownTimer())).ToString();
        }
    }
}
