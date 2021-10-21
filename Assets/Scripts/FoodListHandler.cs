using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodListHandler : ListHandler
{
  List<Recipe> displayedList;

  public override void ShowCompleteList()
  {
    ShowList(ListData.instance.foods);
  }

  protected override void CacheDisplayedList(object list)
  {
    displayedList = (List<Recipe>)list;
  }
}
