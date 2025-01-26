using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListData : MonoBehaviour
{
  public List<Recipe> recipes = new List<Recipe>();
  public List<string> foods = new List<string>();
  public Dictionary<string, int> purchases = new Dictionary<string, int>();
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

    //Todo: can be deleted after all recipes have an ID
    foreach(var recipe in recipes)
    {
        if (recipe.ID == null)
        {
            recipe.ID = Guid.NewGuid().ToString();
            Debug.Log("Generated new gui");
        }
    }

    if (recipes == null) recipes = new List<Recipe>();
  }

  public void SaveFoodList()
  {
    SavingSystem.Save(foods, "FoodList");
  }

  public void LoadFoodList()
  {
    foods = (List<string>)SavingSystem.Load("FoodList");
    if (foods == null) foods = new List<string>();
  }

  public void SaveShoppingList()
  {
    SavingSystem.Save(purchases, "ShoppingList");
  }

  public void LoadShoppingList()
  {
    purchases = (Dictionary<string, int>)SavingSystem.Load("ShoppingList");
    if (purchases == null) purchases = new Dictionary<string, int>();
  }

  public void AddNewItemToShoppingCard(string name, int amount)
  {
    if (purchases.ContainsKey(name))
      ListData.instance.purchases[name] += amount;
    else
      purchases.Add(name, amount);
  }


}
