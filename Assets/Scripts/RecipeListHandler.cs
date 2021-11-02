using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeListHandler : ListHandler
{
  [SerializeField] List<GameObject> sortIcons;
  [SerializeField] GameObject exportBtn;
  Searchbar searchbar;
  List<Recipe> displayedList;
  int sortState = 0;

  private void Awake()
  {
    searchbar = FindObjectOfType<Searchbar>();
  }

  public override void Start()
  {
    ShowList(searchbar.ShowSearchResult());
  }
  public override void ShowList(List<Recipe> list)
  {
    exportModeActive =true;
    ToggleExportMode();
    
    SortList(list);
    base.ShowList(list);
  }

  public override void ShowCompleteList()
  {
    ShowList(searchbar.ShowSearchResult());
  }

  bool exportModeActive = false;
  public void ToggleExportMode()
  {
    exportModeActive = !exportModeActive;
    exportBtn.SetActive(exportModeActive);
    foreach (Transform slot in listTransform)
    {
      slot.GetComponentInChildren<RecipeSlot>().ToggleExportOption(exportModeActive);
    }
  }

  public void ExportRecieps()
  {
    List<Recipe> exportList = new List<Recipe>();
    foreach (Transform slot in listTransform)
    {
      RecipeSlot recipeSlot = slot.GetComponentInChildren<RecipeSlot>();
      if (recipeSlot.ExportRecipe()) exportList.Add(recipeSlot.GetRecipe());
    }

    if (exportList.Count > 0) SavingSystem.ExportData(exportList, "ExportData");
    ToggleExportMode();
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
