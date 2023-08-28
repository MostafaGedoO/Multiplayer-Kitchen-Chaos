using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Kitchen/Recipe")]

public class RecipeSO : ScriptableObject
{
    public string RecipeName;
    public List<KitchenObjectSO> RecipeIngredaintsList;
}
