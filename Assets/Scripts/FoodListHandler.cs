using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodListHandler : MonoBehaviour
{
    [SerializeField] RecipeSlot recipeSlot;
    [SerializeField] Transform list;

    private void Start() 
    {
      foreach(var recipe in FoodList.instance.GetRecipes())
      {
        RecipeSlot slot = Instantiate(recipeSlot, list);
        slot.Initialize(recipe);
      }
    }
}
