using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompeletVisual : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [Space]
    [SerializeField] private List<PlateKitchenObjectSOWithGameObject> platesIngrediants;

    private void Start()
    {
        plateKitchenObject.OnIngrediantAdded += PlateKitchenObject_OnIngrediantAdded;
    }

    private void PlateKitchenObject_OnIngrediantAdded(object sender, PlateKitchenObject.OningredIantAddedEventArgs e)
    {
        foreach (var item in platesIngrediants)
        {
            if(item.KitchenObjectSO == e.kitchenObjectSO)
            {
                item.KitchenObjectGameObject.SetActive(true);
            }
        }
    }
}

[Serializable]
public struct PlateKitchenObjectSOWithGameObject
{
    public KitchenObjectSO KitchenObjectSO;
    public GameObject KitchenObjectGameObject;
}
