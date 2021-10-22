using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeSlot : MonoBehaviour
{
  Recipe recipe;

  public void Initialize(Recipe recipe)
  {
    this.recipe = recipe;
    GetComponentInChildren<TextMeshProUGUI>().text = recipe.name;
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
