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
            //counter has an object
            if (player.HasKitchenObject())
            {
                //player has an object
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject _plateKitchenObject))
                {
                    //player has a plate
                    if(_plateKitchenObject.TryAddIngrediant(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
                else
                {
                    //Player carrying something that is not a plate
                    if(GetKitchenObject().TryGetPlate(out _plateKitchenObject))
                    {
                        if(_plateKitchenObject.TryAddIngrediant(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
                        }

                    }
                }
            }
            else
            {
                //player has nothing
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
