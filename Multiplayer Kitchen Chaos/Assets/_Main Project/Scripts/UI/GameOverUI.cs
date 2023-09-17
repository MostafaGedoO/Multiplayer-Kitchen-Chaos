using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deliverdNumberText;
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.LoadScene(Loader.Scene.MainMenu);
        });
    }

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
