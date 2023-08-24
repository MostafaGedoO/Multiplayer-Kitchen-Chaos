using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabedKitchenObject;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //Spown kitchen object and give it to the player
            GameObject _kitchenObject = Instantiate(kitchenObjectSO.ObjectPrefab);
            _kitchenObject.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

            OnPlayerGrabedKitchenObject?.Invoke(this,EventArgs.Empty);
        }
    }

}
