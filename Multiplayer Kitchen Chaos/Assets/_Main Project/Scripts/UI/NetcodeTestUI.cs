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
            MultiPlayerGameManager.Instance.StartHost();
            gameObject.SetActive(false);
        });
        
        client.onClick.AddListener(() =>
        {
            MultiPlayerGameManager.Instance.StartClient();
            gameObject.SetActive(false);
        });
    }
}
