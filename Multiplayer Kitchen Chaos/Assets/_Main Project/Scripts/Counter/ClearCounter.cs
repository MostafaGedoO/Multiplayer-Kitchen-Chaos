using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    //[SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //nothing on the counter
            if (player.HasKitchenObject())
            {
                //player has a kitchen object
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //player has nothing
            }
        }
        else
        {
            //counter had an object
            if (player.HasKitchenObject())
            {
                //player has an object
            }
            else
            {
                //player has nothing
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
