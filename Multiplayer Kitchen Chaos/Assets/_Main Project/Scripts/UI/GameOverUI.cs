using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deliverdNumberText;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;

        gameObject.SetActive(false);
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOverStete())
        {
            gameObject.SetActive(true);
            deliverdNumberText.text = DeliveryManager.Instance.GetDeliverdRecipesCount().ToString();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
