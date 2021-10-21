using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodList : MonoBehaviour
{
  List<Recipe> recipes = new List<Recipe>();
  List<string> tags = new List<string>();
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
  }

  public void DeleteRecipe(Recipe recipe)
  {
    if (recipe == null) return; 
    recipes.Remove(recipe);
  }

  public void AddTags(List<string> newTags)
  {
    
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
