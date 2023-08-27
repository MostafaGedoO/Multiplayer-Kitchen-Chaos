using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterFX : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveParticals;
    [SerializeField] private GameObject stoveFireGlow;

    private void Start()
    {
        stoveCounter.OnStateChenged += StoveCounter_OnStateChenged;
    }

    private void StoveCounter_OnStateChenged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showFx = e.state == StoveCounter.State.Frying | e.state == StoveCounter.State.Fried;

        stoveParticals.SetActive(showFx);
        stoveFireGlow.SetActive(showFx);
    }
}
