using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CuttingRecipe", menuName = "Kitchen/CuttingRecipe")]

public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO InputObject;
    public KitchenObjectSO OutputSlicedObject;
}
