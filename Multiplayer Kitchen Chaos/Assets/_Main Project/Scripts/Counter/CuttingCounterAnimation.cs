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
        cuttingCounter.OnCuttingPrograssChanged += CuttingCounter_OnCuttingPrograssChanged;
    }

    private void CuttingCounter_OnCuttingPrograssChanged(object sender, CuttingCounter.OnCuttingPrograssChangedEventArgs e)
    {
        if(e.PrograssAmountNormalized != 0)
        {
            _counterAnimator.SetTrigger("Cut");
        }
    }
}
