using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListData : MonoBehaviour
{
  public List<Recipe> recipes = new List<Recipe>();
  public List<Recipe> foods = new List<Recipe>();
  public static ListData instance = null;

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

  public void SetRecipes(List<Recipe> list)
  {
    recipes = list;
  }

  public void SaveRecipeList()
  {
    SavingSystem.Save(recipes, "RecipeList");
  }

  public void LoadRecipeList()
  {
    recipes = (List<Recipe>)SavingSystem.Load("RecipeList");
    if (recipes == null) recipes = new List<Recipe>();
  }

  public void SaveFoodList()
  {
    SavingSystem.Save(foods, "FoodList");
  }

  public void LoadFoodList()
  {
    foods = (List<Recipe>)SavingSystem.Load("FoodList");
    if (foods == null) foods = new List<Recipe>();
  }
}
