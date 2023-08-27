using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FryingRecipe", menuName = "Kitchen/FryingRecipe")]

public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO InputObject;
    public KitchenObjectSO OutputObject;
    public float timer;
}
