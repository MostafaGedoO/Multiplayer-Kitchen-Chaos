using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconImage : MonoBehaviour
{
    [SerializeField] private Image iconImage;


    public void SetIngrediantIcon(KitchenObjectSO _kitchenObjectSO)
    {
        iconImage.sprite = _kitchenObjectSO.ObjectIcon;
    }
}
