using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasPrograss
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSO;

    public event EventHandler<IHasPrograss.OnCuttingPrograssChangedEventArgs> OnFryingPrograssChanged;

    public static event EventHandler OnAnyCut;
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
                if (HasVaildKitchenobject(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    PlaceObjectOnCounterServerRpc();
                }
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
                if (player.HasKitchenObject())
                {
                    //player has an object
                    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject _plateKitchenObject))
                    {
                        //player has a plate
                        if (_plateKitchenObject.TryAddIngrediant(GetKitchenObject().GetKitchenObjectSO()))
                        {
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());
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

    [ServerRpc(RequireOwnership = false)]
    private void PlaceObjectOnCounterServerRpc()
    {
        PlaceObjectOnCounterClientRpc();
    }

    [ClientRpc]
    private void PlaceObjectOnCounterClientRpc()
    {
        cuttingPrograss = 0;

        OnFryingPrograssChanged?.Invoke(this, new IHasPrograss.OnCuttingPrograssChangedEventArgs
        {
            PrograssAmountNormalized = 0
        });
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            CuttingObjctServerRpc();
            CheckCuttingProgressServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CuttingObjctServerRpc()
    {
        CuttingObjctClientRpc();
    }

    [ClientRpc]
    private void CuttingObjctClientRpc()
    {
        foreach (CuttingRecipeSO _cuttingRecipeSO in cuttingRecipeSO)
        {
            if (_cuttingRecipeSO.InputObject == GetKitchenObject().GetKitchenObjectSO())
            {
                cuttingPrograss++;

                OnFryingPrograssChanged?.Invoke(this, new IHasPrograss.OnCuttingPrograssChangedEventArgs
                {
                    PrograssAmountNormalized = (float)cuttingPrograss / cuttingCount
                });

                OnCuttingAnimation?.Invoke(this, EventArgs.Empty);
                OnAnyCut?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CheckCuttingProgressServerRpc()
    {
        foreach (CuttingRecipeSO _cuttingRecipeSO in cuttingRecipeSO)
        {
            if (_cuttingRecipeSO.InputObject == GetKitchenObject().GetKitchenObjectSO())
            {
                if (cuttingPrograss >= cuttingCount)
                {
                    KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    KitchenObject.SpownKitchenObject(_cuttingRecipeSO.OutputSlicedObject, this);
                }
            }
        }
    }

    private bool HasVaildKitchenobject(KitchenObjectSO _kitchenObjectSO)
    {
        foreach(CuttingRecipeSO cuttingRecipeSOAllowed in cuttingRecipeSO)
        {
            if(_kitchenObjectSO == cuttingRecipeSOAllowed.InputObject)
            {
                return true;
            }
        }
        return false;
    }

    new public static void ClearStaticDate()
    {
        OnAnyCut = null;
    }
}
