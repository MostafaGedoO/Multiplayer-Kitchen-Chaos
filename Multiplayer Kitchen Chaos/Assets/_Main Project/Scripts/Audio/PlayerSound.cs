using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;

    private float timer;
    private float maxTimeToPlayAsound = 0.15f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if(player.IsWalking())
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                SoundManager.instance.PlayFootStepSoundServerRpc();
                timer = maxTimeToPlayAsound;
            }
        }
    }
}
