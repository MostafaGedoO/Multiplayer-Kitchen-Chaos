using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinarCounterAnimation : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    private Animator _counterAnimator;

    private void Awake()
    {
        _counterAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabedKitchenObject += ContainerCounter_OnPlayerGrabedKitchenObject;
    }

    private void ContainerCounter_OnPlayerGrabedKitchenObject(object sender, System.EventArgs e)
    {
        _counterAnimator.SetTrigger("OpenClose");
    }
}
