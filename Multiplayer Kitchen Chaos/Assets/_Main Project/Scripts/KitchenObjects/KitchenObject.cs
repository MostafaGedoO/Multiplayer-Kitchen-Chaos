using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;
    private FollowTransform followTransform;

    protected virtual void Awake()
    {
        followTransform = GetComponent<FollowTransform>();
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent _kitchenObjectParent)
    {
        SetKitchenObjectParentServerRpc(_kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference _kitchenObjectParentNetworkObjectReference)
    {
        SetKitchenObjectParentClientRpc(_kitchenObjectParentNetworkObjectReference);
    }

    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference _kitchenObjectParentNetworkObjectReference)
    {
        //Getting the IKitchenObjectParent from a network object refernce
        _kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject _kitchenObjectParentNetworkObject);
        IKitchenObjectParent _kitchenObjectParent = _kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        if (kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearkitchenObject();
        }

        kitchenObjectParent = _kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IkitchenObjectParent Has Already A kitchen Object");
        }

        kitchenObjectParent.SetKitchenObject(this);

        followTransform.SetTargetTransform(kitchenObjectParent.GetFollowTrandormPoint());
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void ClearKitchenObjectOnParent()
    {
        kitchenObjectParent.ClearkitchenObject();
    }

    public static void DestroyKitchenObject(KitchenObject _kitchenObject)
    {
        MultiPlayerGameManager.Instance.DestroyKitchenObjectServerRpc(_kitchenObject.NetworkObject);
    }

    public static void SpownKitchenObject(KitchenObjectSO _kitchenObjectSO,IKitchenObjectParent _kitchenObjectParent)
    {
        MultiPlayerGameManager.Instance.SpownKitchenObject(_kitchenObjectSO, _kitchenObjectParent);
    }

    public bool TryGetPlate(out PlateKitchenObject _plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            _plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            _plateKitchenObject = null;
            return false;
        }
    } 
}
