using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasPrograss
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSO;

    public event EventHandler<IHasPrograss.OnCuttingPrograssChangedEventArgs> OnCuttingPrograssChanged;
    
    public event EventHandler OnCuttingAnimation;

    private int cuttingPrograss;
    private int cuttingCount = 3;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //nothing on the counter
            if (player.HasKitchenObject())
            {
                //player has a kitchen object
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingPrograss = 0;

                OnCuttingPrograssChanged?.Invoke(this, new IHasPrograss.OnCuttingPrograssChangedEventArgs
                {
                    PrograssAmountNormalized = 0
                });
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

    public override void InteractAlternate(Player player)
    {
        if(HasKitchenObject())
        {
            foreach(CuttingRecipeSO _cuttingRecipeSO in cuttingRecipeSO)
            {
                if(_cuttingRecipeSO.InputObject == GetKitchenObject().GetKitchenObjectSO())
                {
                    cuttingPrograss++;

                    OnCuttingPrograssChanged?.Invoke(this, new IHasPrograss.OnCuttingPrograssChangedEventArgs
                    {
                        PrograssAmountNormalized = (float)cuttingPrograss / cuttingCount
                    });

                    OnCuttingAnimation?.Invoke(this,EventArgs.Empty);

                    if (cuttingPrograss >= cuttingCount)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpownKitchenObject(_cuttingRecipeSO.OutputSlicedObject, this);
                        break;
                    }
                }
            }
        }
    }

}
