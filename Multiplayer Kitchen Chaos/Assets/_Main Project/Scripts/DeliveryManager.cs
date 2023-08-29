using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private RecipeListSO recipeListSO;

    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnWattingRcipesListChanged;
    public event EventHandler OnDeliveryFailed;
    public event EventHandler OnDeliveryCompleted;

    private List<RecipeSO> waittingRecipeSOList;

    private float spawnRecipeTime = 5f;
    private int maxRecipesNumber = 5;
    private int spawnedRecipes;
    private int deliverdRecipes;
    private float timer = 0;
    private bool once;
    

    private void Awake()
    {
        Instance = this;
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

                RecipeSO _waittingRecipeSo = recipeListSO.RecipeSOList[UnityEngine.Random.Range(0, recipeListSO.RecipeSOList.Count)];
                waittingRecipeSOList.Add(_waittingRecipeSo);
                OnWattingRcipesListChanged?.Invoke(this,EventArgs.Empty);
                spawnedRecipes++;
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject _plateKitchenObject)
    {
        for(int i = 0 ; i < waittingRecipeSOList.Count ; i++)
        {
            RecipeSO _waittingRecipeSo = waittingRecipeSOList[i];

            if(_waittingRecipeSo.RecipeIngredaintsList.Count == _plateKitchenObject.GetPlateIngrediantsList().Count)
            {
                //Waitting recipe is the same length with the plate ingrediants
                bool _PlateContentMatchesRecipe = true;
                foreach(KitchenObjectSO _recipeKitchenObjectSO in _waittingRecipeSo.RecipeIngredaintsList)
                {
                    //Loop in each ingredaint in recipe
                    bool _ingredaintFound = false;
                    foreach(KitchenObjectSO _plateKitchenObjectSO in _plateKitchenObject.GetPlateIngrediantsList())
                    {
                        //loop in each ingredaint in the plate
                        if(_plateKitchenObjectSO == _recipeKitchenObjectSO)
                        {
                            _ingredaintFound = true;
                            break;
                        }
                    } 
                    if(!_ingredaintFound)
                    {
                        // loop ends without finding the ingredaint in the plate
                        _PlateContentMatchesRecipe = false;
                    }
                }
                if (_PlateContentMatchesRecipe)
                {
                    //Player deliverd the correct recipe
                    waittingRecipeSOList.RemoveAt(i);
                    OnWattingRcipesListChanged?.Invoke(this,EventArgs.Empty);
                    OnDeliveryCompleted?.Invoke(this,EventArgs.Empty);
                    deliverdRecipes++;
                    spawnedRecipes--;
                    return;
                }
            }
        }
        OnDeliveryFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWattingRcipesList()
    {
        return waittingRecipeSOList;
    }

    public int GetDeliverdRecipesCount()
    {
        return deliverdRecipes;
    }
}

