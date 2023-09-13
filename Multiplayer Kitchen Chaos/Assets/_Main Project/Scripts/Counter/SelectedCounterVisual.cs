using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] counterSelectedVisual;

    private void Start()
    {
        if(Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedCounterChanged += OnPlayerSelectedCounterChanged;
        }
        else
        {
            Player.OnAnyPlayerSpawnd += Player_OnAnyPlayerSpawnd;
        }
    }

    private void Player_OnAnyPlayerSpawnd(object sender, System.EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedCounterChanged -= OnPlayerSelectedCounterChanged;
            Player.LocalInstance.OnSelectedCounterChanged += OnPlayerSelectedCounterChanged;
        }
    }

    private void OnPlayerSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(baseCounter == e.SelectedCounter)
        {
            foreach(GameObject obj in counterSelectedVisual)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject obj in counterSelectedVisual)
            {
                obj.SetActive(false);
            }
        }
    }
}
