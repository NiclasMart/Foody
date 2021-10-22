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
    ShowList(ListData.instance.foods);
  }

  public void AddNewFood(string msg)
  {
    if (Input.GetKeyDown(KeyCode.Escape)) { return; }

    ListData.instance.foods.Add(new Recipe(foodIn.GetValue()));
    ListData.instance.SaveFoodList();

    foodIn.ClearField();
    ShowCompleteList();
  }

  public void DeleteFoodFromList(Recipe food)
  {
    ListData.instance.foods.Remove(food);
    ListData.instance.SaveFoodList();
    ShowCompleteList();
  }

  protected override void CacheDisplayedList(object list)
  {
    displayedList = (List<Recipe>)list;
  }
}
