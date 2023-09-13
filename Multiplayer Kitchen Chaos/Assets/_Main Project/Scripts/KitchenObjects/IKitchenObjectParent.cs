using Unity.Netcode;
using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetFollowTrandormPoint();

    public void SetKitchenObject(KitchenObject _kitchenObject);

    public KitchenObject GetKitchenObject();

    public void ClearkitchenObject();

    public bool HasKitchenObject();

    public NetworkObject GetNetworkObject();
   
}
