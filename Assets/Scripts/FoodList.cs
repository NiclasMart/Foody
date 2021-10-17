using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodList : MonoBehaviour
{
  List<Recipe> recipes = new List<Recipe>();
  public static FoodList instance = null;

  private void Awake()
  {
    if (instance != null)
    {
      Destroy(this);
      return;
    }
    instance = this;
    DontDestroyOnLoad(this);
  }

  public void AddRecipe(Recipe newRecipe)
  {
    recipes.Add(newRecipe);
    recipes.Sort();
  }

  public List<Recipe> GetRecipes()
  {
    return recipes;
  }

  public void SetRecipes(List<Recipe> list)
  {
    recipes = list;
  }
}
