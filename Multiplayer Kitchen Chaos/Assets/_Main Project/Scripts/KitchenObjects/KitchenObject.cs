using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent _kitchenObjectParent)
    {
        if(kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearkitchenObject();
        }

        kitchenObjectParent = _kitchenObjectParent;

        if(kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IkitchenObjectParent Has Already A kitchen Object");
        }

        kitchenObjectParent.SetKitchenObject(this);

        //transform.parent = kitchenObjectParent.GetFollowTrandormPoint();
        //transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearkitchenObject();
        Destroy(gameObject);
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
