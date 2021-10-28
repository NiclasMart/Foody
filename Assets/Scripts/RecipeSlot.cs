using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RecipeSlot : MonoBehaviour
{
  [SerializeField] protected Image typeIndicator;
  [SerializeField] protected Color cookColor, bakeColor, otherColor;
  protected Recipe recipe;
  

  public virtual void Initialize(Recipe recipe)
  {
    this.recipe = recipe;
    GetComponentInChildren<TextMeshProUGUI>().text = recipe.name;
    SetIndicatorColor(recipe.type);
  }

  private void SetIndicatorColor(RecipeType type)
  {
    switch (type)
    {
      case RecipeType.Kochen: typeIndicator.color = cookColor;
      break;
      case RecipeType.Backen: typeIndicator.color = bakeColor;
      break;
      default: typeIndicator.color = otherColor;
      break;
    }
  }

  public void ShowRecipe()
  {
    RecipeDisplay display = FindObjectOfType<RecipeDisplay>();
    display.ToggleRecipeDisplay(true);
    display.Display(recipe);
  }

  public void DeleteSlot()
  {
    FindObjectOfType<FoodListHandler>()?.DeleteFoodFromList(recipe);
  }
}
