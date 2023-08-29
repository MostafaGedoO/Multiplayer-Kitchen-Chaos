using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayClockUI : MonoBehaviour
{
    [SerializeField] private Image clockImage;

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            clockImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
        }
    }
}
