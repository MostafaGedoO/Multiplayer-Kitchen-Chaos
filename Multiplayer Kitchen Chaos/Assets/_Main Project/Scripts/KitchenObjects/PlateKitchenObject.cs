using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> allowedKitchenObjectsSOList;

    public event EventHandler<OningredIantAddedEventArgs> OnIngrediantAdded;
    public class OningredIantAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    private List<KitchenObjectSO> ingrediantsInPlateList;

    private void Awake()
    {
        ingrediantsInPlateList = new List<KitchenObjectSO>();    
    }

    public bool TryAddIngrediant(KitchenObjectSO _kitchenObjectSO)
    {
        if(!allowedKitchenObjectsSOList.Contains(_kitchenObjectSO))
            return false;

        if (ingrediantsInPlateList.Contains(_kitchenObjectSO))
            return false;

        ingrediantsInPlateList.Add(_kitchenObjectSO);

        OnIngrediantAdded?.Invoke(this, new OningredIantAddedEventArgs
        {
            kitchenObjectSO = _kitchenObjectSO
        });

        return true;

    }

    public List<KitchenObjectSO> GetPlateIngrediants()
    {
        return ingrediantsInPlateList;
    }
}
