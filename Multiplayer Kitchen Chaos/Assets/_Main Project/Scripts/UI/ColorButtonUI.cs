using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButtonUI : MonoBehaviour
{
    [SerializeField] private int colorId;
    [SerializeField] private Button colorButton;
    [SerializeField] private GameObject selectedVisual;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            MultiPlayerGameManager.Instance.ChangePlayerColor(colorId);
        });
    }

    private void Start()
    {
        MultiPlayerGameManager.Instance.OnPlayerDataListChanged += MultiplayerManager_OnPlayerDataListChanged;
        GetComponent<Image>().color = MultiPlayerGameManager.Instance.GetAColorFromList(colorId);
        UpdateIsSelected();
    }

    private void MultiplayerManager_OnPlayerDataListChanged(object sender, System.EventArgs e)
    {
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        if (MultiPlayerGameManager.Instance.GetLocalPlayerDate().colorId == colorId)
        {
            selectedVisual.SetActive(true);
        }
        else
        {
            selectedVisual.SetActive(false);
        }
    }


    private void OnDestroy()
    {
        MultiPlayerGameManager.Instance.OnPlayerDataListChanged -= MultiplayerManager_OnPlayerDataListChanged;
    }
}
