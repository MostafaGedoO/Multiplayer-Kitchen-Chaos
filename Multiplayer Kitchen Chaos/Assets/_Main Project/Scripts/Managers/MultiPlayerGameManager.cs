using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiPlayerGameManager : NetworkBehaviour
{
    [SerializeField] private KitchenObjectListSO kitchenObjectListSO;

    public static MultiPlayerGameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SpownKitchenObject(KitchenObjectSO _kitchenObjectSO, IKitchenObjectParent _kitchenObjectParent)
    {
        SpownKitchenObjectServerRpc(GetKitchenObjectSOIndexFromList(_kitchenObjectSO),_kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpownKitchenObjectServerRpc(int _kitchenObjectSOIndex, NetworkObjectReference _kitchenObjectParentNetworkObjectReference)
    {
        //Instantiate the kitchen object and then spawn it over the network
        GameObject _kitchenObject = Instantiate(GetKitchenObjectSOFromList(_kitchenObjectSOIndex).ObjectPrefab);
        _kitchenObject.GetComponent<NetworkObject>().Spawn(true);

        //Getting the IKitchenObjectParent from a network object refernce
        _kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject _kitchenObjectParentNetworkObject);
        IKitchenObjectParent _kitchenObjectParent = _kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        //Getting the kitchen object and setting the parent
        KitchenObject _spownedKitchenObject = _kitchenObject.GetComponent<KitchenObject>();
        _spownedKitchenObject.SetKitchenObjectParent(_kitchenObjectParent);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyKitchenObjectServerRpc(NetworkObjectReference _kitchenObjectNetworkObjectRefernce) 
    {
        _kitchenObjectNetworkObjectRefernce.TryGet(out NetworkObject _kitchenObjectNetworkObject);
        KitchenObject _kitchenObject = _kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        ClearKitchenObjectOnParentClientRpc(_kitchenObjectNetworkObjectRefernce);
        _kitchenObject.DestroySelf();
    }

    [ClientRpc]
    private void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference _kitchenObjectNetworkObjectRefernce)
    {
        _kitchenObjectNetworkObjectRefernce.TryGet(out NetworkObject _kitchenObjectNetworkObject);
        KitchenObject _kitchenObject = _kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        _kitchenObject.ClearKitchenObjectOnParent();
    }

    public int GetKitchenObjectSOIndexFromList(KitchenObjectSO _kitchenObjectSO)
    {
        return kitchenObjectListSO.kitchenObjectSOList.IndexOf(_kitchenObjectSO);
    }

    public KitchenObjectSO GetKitchenObjectSOFromList(int _index)
    {
        return kitchenObjectListSO.kitchenObjectSOList[_index];
    }
}
