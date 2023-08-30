using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownUISound : MonoBehaviour
{
    private AudioSource countdownSound;

    private void Awake()
    {
        countdownSound = GetComponent<AudioSource>();
    }

    public void PlayerCountdownSound()
    {
        countdownSound.Play();
    }
}
