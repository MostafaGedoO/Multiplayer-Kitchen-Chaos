using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : MonoBehaviour,IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    public static event EventHandler OnAnyKitchenObjectDropedOnACounter;

    private KitchenObject kitchenObject;

    public virtual void Interact(Player player)
    {
        Debug.LogError("This should not be called");
    } 
    
    public virtual void InteractAlternate(Player player)
    {
    }

    public Transform GetFollowTrandormPoint()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject _kitchenObject)
    {
        kitchenObject = _kitchenObject;
        if(_kitchenObject != null)
        {
            OnAnyKitchenObjectDropedOnACounter?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearkitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public static void ClearStaticDate()
    {
        OnAnyKitchenObjectDropedOnACounter = null;
    }

    public NetworkObject GetNetworkObject()
    {
        return null;
    }
}
