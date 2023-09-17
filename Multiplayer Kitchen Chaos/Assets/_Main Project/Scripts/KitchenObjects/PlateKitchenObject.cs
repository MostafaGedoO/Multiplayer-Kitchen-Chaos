using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    protected override void Awake()
    {
        base.Awake();
        ingrediantsInPlateList = new List<KitchenObjectSO>();    
    }

    public bool TryAddIngrediant(KitchenObjectSO _kitchenObjectSO)
    {
        if(!allowedKitchenObjectsSOList.Contains(_kitchenObjectSO))
            return false;

        if (ingrediantsInPlateList.Contains(_kitchenObjectSO))
            return false;

        AddIngrediantToPlaterServerRpc(MultiPlayerGameManager.Instance.GetKitchenObjectSOIndexFromList(_kitchenObjectSO));

        return true;

    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngrediantToPlaterServerRpc(int _kitchenObjctSOIndex)
    {
        AddIngrediantToPlaterClientRpc(_kitchenObjctSOIndex);
    }

    [ClientRpc]
    private void AddIngrediantToPlaterClientRpc(int _kitchenObjctSOIndex)
    {
        KitchenObjectSO _kitchenObjectSO = MultiPlayerGameManager.Instance.GetKitchenObjectSOFromList(_kitchenObjctSOIndex);
        ingrediantsInPlateList.Add(_kitchenObjectSO);

        OnIngrediantAdded?.Invoke(this, new OningredIantAddedEventArgs
        {
            kitchenObjectSO = _kitchenObjectSO
        });

        SoundManager.instance.PlaySoundOnPlateIngrediantAdded();
    }

    public List<KitchenObjectSO> GetPlateIngrediantsList()
    {
        return ingrediantsInPlateList;
    }
}
