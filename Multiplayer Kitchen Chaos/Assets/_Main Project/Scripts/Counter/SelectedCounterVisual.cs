using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] counterSelectedVisual;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += OnPlayerSelectedCounterChanged;
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
