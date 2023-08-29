using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource sizzleAudioSource;

    private void Awake()
    {
        sizzleAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChenged += StoveCounter_OnStateChenged;
    }

    private void StoveCounter_OnStateChenged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if (e.state == StoveCounter.State.Frying | e.state == StoveCounter.State.Fried)
        {
            sizzleAudioSource.Play();
        }
        else
        {
            sizzleAudioSource.Stop();
        }
    }
}
