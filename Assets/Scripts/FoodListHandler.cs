using System.Collections.Generic;
using UnityEngine;

public class FoodListHandler : MonoBehaviour
{
  [SerializeField] RecipeSlot recipeSlot;
  [SerializeField] Transform listTransform;
  [SerializeField] List<GameObject> sortIcons;
  List<Recipe> displayedList;
  int sortState = 0;


  private void Start()
  {
    ShowCompleteList();
  }

  public void ShowList(List<Recipe> list)
  {
    DeleteList();
    SortList(list);

    foreach (var recipe in list)
    {
      RecipeSlot slot = Instantiate(recipeSlot, listTransform);
      slot.Initialize(recipe);
    }
    displayedList = list;
  }

  void SortList(List<Recipe> list)
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

  public void ShowCompleteList()
  {
    ShowList(FoodList.instance.GetRecipes());
  }

  public void ChangeSortOrder(int state)
  {
    sortState ^= 1;
    sortState ^= (-(state >> 1) ^ sortState) & (1 << 1);
    SetSortIcons();
    ShowList(displayedList);

  }

  void SetSortIcons()
  {
    foreach (var icon in sortIcons)
    {
      icon.SetActive(false);
    }
    sortIcons[sortState].SetActive(true);
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
