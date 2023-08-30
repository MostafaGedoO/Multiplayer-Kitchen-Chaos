using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveWarringUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private Animator barAnimator;

    private void Start()
    {
        stoveCounter.OnFryingPrograssChanged += StoveCounter_OnFryingPrograssChanged;

        gameObject.SetActive(false);
        barAnimator.SetBool("Burning", false);

        GetComponentInChildren<AudioSource>().playOnAwake = true;
    }

    private void StoveCounter_OnFryingPrograssChanged(object sender, IHasPrograss.OnCuttingPrograssChangedEventArgs e)
    {
        bool show = e.PrograssAmountNormalized > 0.5f & stoveCounter.IsFriedState();

        if (show)
        {
            gameObject.SetActive(true);
            barAnimator.SetBool("Burning",true);
        }
        else
        {
            gameObject.SetActive(false);
            barAnimator.SetBool("Burning",false);
        }
    }
}
