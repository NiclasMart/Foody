using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RecipeSlot : MonoBehaviour
{
  [SerializeField] protected Image typeIndicator;
  [SerializeField] protected Color cookColor, bakeColor, otherColor, markedColor;
  [SerializeField] Toggle toggle;
  protected Recipe recipe;


  public virtual void Initialize(Recipe recipe)
  {
    this.recipe = recipe;
    GetComponentInChildren<TextMeshProUGUI>().text = recipe.name;
    SetIndicatorColor(recipe.type);
    SetMarkedColor(recipe.marked);
  }

  private void SetMarkedColor(bool marked)
  {
    if (marked) GetComponent<Image>().color = markedColor;
    else GetComponent<Image>().color = Color.white;
  }

  private void SetIndicatorColor(RecipeType type)
  {
    switch (type)
    {
      case RecipeType.Kochen:
        typeIndicator.color = cookColor;
        break;
      case RecipeType.Backen:
        typeIndicator.color = bakeColor;
        break;
      default:
        typeIndicator.color = otherColor;
        break;
    }
  }

  public void ShowRecipe()
  {
    RecipeListHandler listHandler = FindObjectOfType<RecipeListHandler>();
    listHandler.DisplayRecipe(recipe);
  }

  public void DeleteSlot()
  {
    FindObjectOfType<FoodListHandler>()?.DeleteFoodFromList(recipe);
  }

  public Recipe GetRecipe()
  {
    return recipe;
  }

  public bool ExportRecipe()
  {
    return GetComponentInChildren<Toggle>().isOn;
  }

  public void ToggleExportOption(bool on)
  {
    toggle.gameObject.SetActive(on);
    if (!on) toggle.isOn = false;
  }
}
