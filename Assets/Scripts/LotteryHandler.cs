using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LotteryHandler : ListHandler
{
    [SerializeField] List<LotterySlot> slots = new List<LotterySlot>();
    [SerializeField] Searchbar dropdown;

    const int holdBackCount = 20; //defines how often you can reroll before the same recipe can show again
    Queue<Recipe> recentlyShownRecipes = new Queue<Recipe>(holdBackCount * 3);

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

            Recipe selectedRecipe;
            int rand;
            do
            {
                rand = Random.Range(0, typeSelection.Count);
                selectedRecipe = typeSelection[rand];
            }
            while (!RecipeCanBeShown(selectedRecipe));

            finalSelection.Add(selectedRecipe);
            typeSelection.RemoveAt(rand);
        }
        foreach (var rec in recentlyShownRecipes)
        {
            Debug.Log(rec.name);
        }
        Debug.Log("------");
        return finalSelection;
    }

    private bool RecipeCanBeShown(Recipe recipe)
    {
        if (ListData.instance.recipes.Count < holdBackCount * 3) return true;
        if (recentlyShownRecipes.Contains(recipe))
        {
            Debug.Log("Hold Back: " + recipe.name);
            return false;
        }
        else
        {
            if (recentlyShownRecipes.Count == holdBackCount * 3)
                recentlyShownRecipes.Dequeue();
            recentlyShownRecipes.Enqueue(recipe);     
        }
        return true;
    }
}
