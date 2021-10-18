using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodListHandler : MonoBehaviour
{
  [SerializeField] RecipeSlot recipeSlot;
  [SerializeField] Transform listTransform;

  private void Start()
  {
    ShowCompleteList();
  }

  public void ShowList(List<Recipe> list)
  {
    DeleteList();

    foreach (var recipe in list)
    {
      RecipeSlot slot = Instantiate(recipeSlot, listTransform);
      slot.Initialize(recipe);
    }
  }

  public void ShowCompleteList()
  {
    ShowList(FoodList.instance.GetRecipes());
  }


  private void DeleteList()
  {
    foreach (Transform item in listTransform.transform)
    {
      if (item == listTransform) continue;
      Destroy(item.gameObject);
    }
  }


}
