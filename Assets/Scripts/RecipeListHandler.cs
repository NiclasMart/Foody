using System.Collections.Generic;
using UnityEngine;

public class RecipeListHandler : ListHandler
{

  [SerializeField] List<GameObject> sortIcons;
  List<Recipe> displayedList;
  int sortState = 0;

  public override void ShowList(List<Recipe> list)
  {
    SortList(list);
    base.ShowList(list);
  }

  public override void ShowCompleteList()
  {
    ShowList(ListData.instance.recipes);
  }

  public void ChangeSortOrder(int state)
  {
    ListData.instance.LoadRecipeList();
    sortState ^= 1;
    sortState ^= (-(state >> 1) ^ sortState) & (1 << 1);
    SetSortIcons();
    ShowList(displayedList);

  }
  protected override void CacheDisplayedList(object list)
  {
    displayedList = (List<Recipe>)list;
  }

  private void SortList(List<Recipe> list)
  {
    //sort name
    if (((sortState >> 1) & 1) == 0)
    {
      if ((sortState & 1) == 1) list.Sort((a, b) => b.name.CompareTo(a.name));
      else list.Sort();
    }
    else
    {
      if ((sortState & 1) == 1) list.Sort((a, b) => b.GetDate().CompareTo(a.GetDate()));
      else list.Sort((a, b) => a.GetDate().CompareTo(b.GetDate()));
    }
  }

  private void SetSortIcons()
  {
    foreach (var icon in sortIcons)
    {
      icon.SetActive(false);
    }
    sortIcons[sortState].SetActive(true);
  }
}
