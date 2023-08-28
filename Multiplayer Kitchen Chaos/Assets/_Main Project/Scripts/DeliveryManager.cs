using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waittingRecipeSOList;

    private float spawnRecipeTime = 4f;
    private int maxRecipesNumber = 5;
    private int spawnedRecipes;
    private float timer = 0;
    private bool once;
    

    private void Awake()
    {
        waittingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        if (spawnedRecipes < maxRecipesNumber)
        {
            timer += Time.deltaTime;
            if (timer >= spawnRecipeTime)
            {
                timer = 0;

                if (!once)
                {
                    once = true;
                    spawnRecipeTime = 10f;
                }

                RecipeSO _waittingRecipeSo = recipeListSO.RecipeSOList[Random.Range(0, recipeListSO.RecipeSOList.Count)];
                waittingRecipeSOList.Add(_waittingRecipeSo);
                Debug.Log(_waittingRecipeSo.RecipeName);
                spawnedRecipes++;
            }
        }
    }
}

