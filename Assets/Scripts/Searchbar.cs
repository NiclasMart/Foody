using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Searchbar : MonoBehaviour
{
  [SerializeField] TMP_Dropdown dropdown;
  RecipeListHandler listHandler;
  TMP_InputField input;


  private void Awake()
  {
    listHandler = FindObjectOfType<RecipeListHandler>();
    input = GetComponent<TMP_InputField>();
  }

  public void ShowSearchResult()
  {
    //handle recipe type
    if (input.text == null || input.text == "")
    {
      listHandler.ShowList(FilterRecipeType());
      return;
    }

    //handle search input
    List<Recipe> showList = new List<Recipe>();
    string searchString = input.text;
    if (searchString.Contains("#"))
    {
      string[] searchTerms = searchString.Split('#');

      foreach (var term in searchTerms)
      {
        if (term == "") continue;
        string lowCaseTerm = term.ToLower();
        string[] words = lowCaseTerm.Split(' ');
        if (words[0] == "tag") showList = SearchTag(words, showList);
        else if (words[0] == "zutat") showList = SearchIngredience(words, showList);
        else SearchName(searchString, showList);
      }
    }
    else SearchName(searchString, showList);

    listHandler.ShowList(showList);
  }

  public List<Recipe> FilterRecipeType()
  {
    if (dropdown.captionText.text == "Alle") return ListData.instance.recipes;

    List<Recipe> showList = new List<Recipe>();
    foreach (var recipe in ListData.instance.recipes)
    {
      if (recipe.type.ToString() == dropdown.captionText.text) showList.Add(recipe);
    }
    return showList;
  }

  private List<Recipe> SearchIngredience(string[] ingredients, List<Recipe> showList)
  {
    Debug.Log("Zutat gefunden");
    if (ingredients.Length == 1 || ingredients[1] == "") return showList;

    //iterate over each recipe
    List<Recipe> newShowList = new List<Recipe>();
    List<Recipe> recipeSelection = showList.Count == 0 ? FilterRecipeType() : showList;
    foreach (var recipe in recipeSelection)
    {
      if (recipe.ingredients == "") continue;
      bool containsIngredients = true;
      //iterate over each tag
      for (int i = 1; i < ingredients.Length; i++)
      {
        if (ingredients[i] == "") continue;
        if (!recipe.ingredients.Contains(ingredients[i]))
        {
          containsIngredients = false;
          break;
        }
      }
      if (containsIngredients) newShowList.Add(recipe);
    }
    return newShowList;
  }

  private void SearchName(string searchString, List<Recipe> showList)
  {
    foreach (var recipe in ListData.instance.recipes)
    {
      if (recipe.name.Contains(searchString)) showList.Add(recipe);
    }
  }

  private List<Recipe> SearchTag(string[] tags, List<Recipe> showList)
  {
    Debug.Log("Tag gefunden");
    if (tags.Length == 1 || tags[1] == "") return showList;

    //iterate over each recipe
    List<Recipe> newShowList = new List<Recipe>();
    List<Recipe> recipeSelection = showList.Count == 0 ? FilterRecipeType() : showList;
    foreach (var recipe in recipeSelection)
    {
      if (recipe.tags.Count == 0) continue;
      bool tagsAreValid = true;
      //iterate over each tag
      for (int i = 1; i < tags.Length; i++)
      {
        if (tags[i] == "") continue;
        if (!recipe.tags.Contains(tags[i]))
        {
          tagsAreValid = false;
          break;
        }
      }
      if (tagsAreValid) newShowList.Add(recipe);
    }
    return newShowList;
  }

  public void ResetSearch()
  {
    input.text = "";
    ShowSearchResult();
  }
}
