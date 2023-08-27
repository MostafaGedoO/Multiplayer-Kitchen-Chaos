using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
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

        transform.parent = kitchenObjectParent.GetFollowTrandormPoint();
        transform.localPosition = Vector3.zero;
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

    public static KitchenObject SpownKitchenObject(KitchenObjectSO _kitchenObjectSO,IKitchenObjectParent _kitchenObjectParent)
    {
        GameObject _kitchenObject = Instantiate(_kitchenObjectSO.ObjectPrefab);
        KitchenObject _spownedKitchenObject = _kitchenObject.GetComponent<KitchenObject>();
        _spownedKitchenObject.SetKitchenObjectParent(_kitchenObjectParent);

        return _spownedKitchenObject;
    }
}
