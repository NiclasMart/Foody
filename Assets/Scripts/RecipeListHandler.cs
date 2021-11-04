using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeListHandler : ListHandler
{
  [SerializeField] List<GameObject> sortIcons;
  [SerializeField] GameObject exportBtn;
  Searchbar searchbar;
  [HideInInspector] public List<Recipe> displayedList;
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
    exportModeActive = true;
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

    if (exportList.Count > 0) SavingSystem.ExportData(exportList, "ExportRecipeList");
    ToggleExportMode();
  }

  public void ChangeSortOrder(int state)
  {
    int order = sortState >> 2;
    order ^= 1;
    sortState = 4 * order + state;
    SetSortIcons();
    ShowList(displayedList);

  }
  protected override void CacheDisplayedList(object list)
  {
    displayedList = (List<Recipe>)list;
  }

  private void SortList(List<Recipe> list)
  {
    switch (sortState & 3)
    {
      case 0: //name
        Debug.Log("Sort name");
        if (sortState >> 2 == 1) list.Sort((a, b) => b.name.CompareTo(a.name));
        else list.Sort();
      break;
      case 1: //cook date
        Debug.Log("Sort cook date");
        if (sortState >> 2 == 1) list.Sort((a, b) => b.GetCookDate().CompareTo(a.GetCookDate()));
        else list.Sort((a, b) => a.GetCookDate().CompareTo(b.GetCookDate()));
      break;
      case 2: //creation date
        Debug.Log("Sort creation date");
        if (sortState >> 2 == 1) list.Sort((a, b) => b.GetOriginDate().CompareTo(a.GetOriginDate()));
        else list.Sort((a, b) => a.GetOriginDate().CompareTo(b.GetOriginDate()));
      break;
    }
  }

  private void SetSortIcons()
  {
    foreach (var icon in sortIcons)
    {
      if (icon) icon.SetActive(false);
    }
    sortIcons[sortState].SetActive(true);
  }
}
