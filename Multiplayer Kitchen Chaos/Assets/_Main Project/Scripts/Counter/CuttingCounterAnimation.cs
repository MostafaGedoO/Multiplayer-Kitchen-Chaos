using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterAnimation : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator _counterAnimator;

    private void Awake()
    {
        _counterAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCuttingAnimation += CuttingCounter_OnCuttingAnimation;
    }

    private void CuttingCounter_OnCuttingAnimation(object sender, System.EventArgs e)
    {
        _counterAnimator.SetTrigger("Cut");
    }

}
