using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private GameObject iconTemplate;

    private void Awake()
    {
        iconTemplate.SetActive(false);
    }

    private void Start()
    {
        plateKitchenObject.OnIngrediantAdded += PlateKitchenObject_OnIngrediantAdded;
    }

    private void PlateKitchenObject_OnIngrediantAdded(object sender, PlateKitchenObject.OningredIantAddedEventArgs e)
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate.transform) continue;
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO _kitchenObjectSO in plateKitchenObject.GetPlateIngrediantsList())
        {
            GameObject _iconTeplate = Instantiate(iconTemplate, transform);
           _iconTeplate.SetActive(true);
           _iconTeplate.GetComponent<PlateIconImage>().SetIngrediantIcon(_kitchenObjectSO);
        }
    }
}
