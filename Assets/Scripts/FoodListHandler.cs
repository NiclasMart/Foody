using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodListHandler : ListHandler
{
  [SerializeField] InputFunctionality foodIn;
  List<Recipe> displayedList;

  private void Awake()
  {
    foodIn.GetComponent<TMP_InputField>().onSubmit.AddListener(AddNewFood);
  }

  public override void ShowCompleteList()
  {
    List<Recipe> displayFoodList = new List<Recipe>();
    foreach (var foodIdentifier in ListData.instance.foods)
    {
      Recipe matchingRecipe = ListData.instance.recipes.Find(recipe => recipe.ID == foodIdentifier);
      if (matchingRecipe != null) displayFoodList.Add(matchingRecipe);
      else displayFoodList.Add(new Recipe(foodIdentifier, false));
    }
    ShowList(displayFoodList);
  }

  public void AddNewFood(string msg)
  {
    if (Input.GetKeyDown(KeyCode.Escape)) return;

    ListData.instance.foods.Add(foodIn.GetValue());
    ListData.instance.SaveFoodList();

    foodIn.ClearField();
    ShowCompleteList();
  }

  public void DeleteFoodFromList(Recipe food)
  {
    if (food.ID == null) ListData.instance.foods.Remove(food.name);
    else ListData.instance.foods.Remove(food.ID);

    ListData.instance.SaveFoodList();
    ShowCompleteList();
  }

  protected override void CacheDisplayedList(object list)
  {
    displayedList = (List<Recipe>)list;
  }
}
