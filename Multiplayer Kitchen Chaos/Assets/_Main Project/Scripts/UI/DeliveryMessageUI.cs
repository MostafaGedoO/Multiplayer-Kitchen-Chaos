using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryMessageUI : MonoBehaviour
{
    [SerializeField] private GameObject successDeliveryMessage;
    [SerializeField] private GameObject failedDeliveryMessage;

    private void Start()
    {
        DeliveryManager.Instance.OnDeliveryCompleted += DeliveryManager_OnDeliveryCompleted;
        DeliveryManager.Instance.OnDeliveryFailed += DeliveryManager_OnDeliveryFailed;
    }

    private void DeliveryManager_OnDeliveryFailed(object sender, System.EventArgs e)
    {
        failedDeliveryMessage.SetActive(false);
        failedDeliveryMessage.SetActive(true);
    }

    private void DeliveryManager_OnDeliveryCompleted(object sender, System.EventArgs e)
    {
        successDeliveryMessage.SetActive(false);
        successDeliveryMessage.SetActive(true);
    }
}
