using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeTemplateUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private GameObject iconsContainer;
    [SerializeField] private GameObject iconTemplate;

    private void Awake()
    {
        iconTemplate.SetActive(false);
    }

    public void SetRecipeInfo(RecipeSO _recipeSO)
    {
        recipeName.text = _recipeSO.RecipeName;

        foreach (KitchenObjectSO _kitchenObjectSO in _recipeSO.RecipeIngredaintsList)
        {
            GameObject _icon = Instantiate(iconTemplate, iconsContainer.transform);
            _icon.SetActive(true);
            _icon.GetComponent<Image>().sprite = _kitchenObjectSO.ObjectIcon;
        }
    }

}
