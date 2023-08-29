using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject continar;
    [SerializeField] private GameObject recipeTemplate;

    private void Awake()
    {
        recipeTemplate.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnWattingRcipesListChanged += DeliveryManager_OnWattingRcipesListChanged;
    }

    private void DeliveryManager_OnWattingRcipesListChanged(object sender, System.EventArgs e)
    {
        UpdateRecipesWattingListUI();
    }

    private void UpdateRecipesWattingListUI()
    {
        foreach(Transform _transform in continar.transform)
        {
            if (_transform == recipeTemplate.transform) continue;
            Destroy(_transform.gameObject);
        }

        foreach  (RecipeSO _recipeSO in DeliveryManager.Instance.GetWattingRcipesList())
        {
            GameObject _recipeUI = Instantiate(recipeTemplate, continar.transform);
            _recipeUI.SetActive(true);
            _recipeUI.GetComponent<RecipeTemplateUIHandler>().SetRecipeInfo(_recipeSO);
        }
    }
}
