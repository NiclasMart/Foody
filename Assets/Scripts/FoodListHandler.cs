using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodListHandler : MonoBehaviour
{
  [SerializeField] RecipeSlot recipeSlot;
  [SerializeField] Transform list;

  private void Start()
  {
    ShowList();
  }

  public void RefreshList()
  {
    DeleteList();
    ShowList();
  }

  private void ShowList()
  {
    foreach (var recipe in FoodList.instance.GetRecipes())
    {
      RecipeSlot slot = Instantiate(recipeSlot, list);
      slot.Initialize(recipe);
    }
  }

  private void DeleteList()
  {
    foreach (Transform item in list.transform)
    {
      if (item == list) continue;
      Destroy(item.gameObject);
    }
  }


}
