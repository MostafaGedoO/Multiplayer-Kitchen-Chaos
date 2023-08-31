using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetcodeTestUI : MonoBehaviour
{
    [SerializeField] private Button host;
    [SerializeField] private Button client;

    private void Awake()
    {
        host.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            GameManager.Instance.StartCountdown();
            gameObject.SetActive(false);
        });
        
        client.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            GameManager.Instance.StartCountdown();
            gameObject.SetActive(false);
        });
    }
}
