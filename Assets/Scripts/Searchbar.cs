using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Searchbar : MonoBehaviour
{
  FoodListHandler listHandler;
  TMP_InputField input;


  private void Awake()
  {
    listHandler = FindObjectOfType<FoodListHandler>();
    input = GetComponent<TMP_InputField>();
  }

  public void ShowSearchResult()
  {
    if (input.text == null || input.text == "")
    {
      listHandler.ShowCompleteList();
      return;
    }

    string searchString = input.text;
    List<Recipe> showList = new List<Recipe>();

    foreach (var recipe in FoodList.instance.GetRecipes())
    {
      if (recipe.name.Contains(searchString)) showList.Add(recipe);
    }

    listHandler.ShowList(showList);
  }

  public void ResetSearch()
  {
    input.text = "";
    listHandler.ShowCompleteList();
  }
}
