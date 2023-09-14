using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabedKitchenObject;
    [SerializeField] bool isBredCounter;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (!player.HasKitchenObject())
            {
                //Spown kitchen object and give it to the player
                KitchenObject.SpownKitchenObject(kitchenObjectSO, player);

                InteractVisualServerRpc();
            }
            else if (isBredCounter)
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject _plateKichenObject))
                {
                    if (_plateKichenObject.TryAddIngrediant(kitchenObjectSO))
                    {
                        //Spawn a bred and add it to the plate
                        InteractVisualServerRpc();
                    }
                }
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void InteractVisualServerRpc()
    {
        InteractVisualClientRpc();
    }

    [ClientRpc]
    private void InteractVisualClientRpc()
    {
        OnPlayerGrabedKitchenObject?.Invoke(this, EventArgs.Empty);
    }
}
