using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LotteryHandler : ListHandler
{
  [SerializeField] List<LotterySlot> slots = new List<LotterySlot>();
  [SerializeField] Searchbar dropdown;

  private void Awake()
  {
    RollNewRecipes();
  }

  public void RollNewRecipes()
  {
    List<Recipe> selection = GetSelection();
    ShowSelection(selection);
  }

  private void ShowSelection(List<Recipe> selection)
  {
    for (int i = 0; i < 3; i++)
    {
      if (selection.Count > i) slots[i].Initialize(selection[i]);
      else slots[i].Initialize(null);
    }
  }

  private List<Recipe> GetSelection()
  {
    List<Recipe> typeSelection = dropdown.FilterRecipeType();

    List<Recipe> finalSelection = new List<Recipe>();
    for (int i = 0; i < 3; i++)
    {
      if (typeSelection.Count == 0) continue;
      int rand = Random.Range(0, typeSelection.Count);
      finalSelection.Add(typeSelection[rand]);
      typeSelection.RemoveAt(rand);
    }
    return finalSelection;
  }
}
