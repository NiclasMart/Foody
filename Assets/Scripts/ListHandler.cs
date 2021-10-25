using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListHandler : MonoBehaviour
{
  [SerializeField] protected RecipeSlot recipeSlot;
  [SerializeField] protected Transform listTransform;

  public virtual void Start()
  {
    ShowCompleteList();
  }

  public virtual void ShowCompleteList() { }
  protected virtual void CacheDisplayedList(object list) { }

  public virtual void ShowList(List<Recipe> list)
  {
    DeleteList();

    foreach (var recipe in list)
    {
      RecipeSlot slot = Instantiate(recipeSlot, listTransform);
      slot.Initialize(recipe);
    }
    CacheDisplayedList(list);

  }

  protected void DeleteList()
  {
    foreach (Transform item in listTransform.transform)
    {
      if (item == listTransform) continue;
      Destroy(item.gameObject);
    }
  }
}
