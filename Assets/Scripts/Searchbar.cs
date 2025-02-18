using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Linq;

public class Searchbar : MonoBehaviour
{
    [SerializeField] public TMP_Dropdown dropdown;
    TMP_InputField input;


    private void Awake()
    {
        input = GetComponent<TMP_InputField>();
    }

    public List<Recipe> GetSearchResult()
    {
        //handle recipe type
        if (input.text == null || input.text == "") return FilterRecipeType();


        //handle search input
        List<Recipe> showList = new List<Recipe>();
        string searchString = input.text.ToLower();
        if (searchString.Contains("#"))
        {
            string[] searchTerms = searchString.Split('#');

            foreach (var term in searchTerms)
            {
                if (term == "") continue;
                if (term.Contains("tag")) showList = SearchTag(term.Split(' '), showList);
                else if (term.Contains("zutat")) showList = SearchIngredience(Regex.Split(term, @"zutat\s|,\s|,").Where(part => !string.IsNullOrEmpty(part)).ToArray(), showList);
                else SearchName(searchString, showList);
            }
        }
        else
        {
            showList = FilterRecipeType();
            showList = SearchName(searchString, showList);
        }

        return showList;
    }

    public void Fill(string term)
    {
        input.text = term;
        GetSearchResult();
    }

    public List<Recipe> FilterRecipeType()
    {
        if (dropdown.captionText.text == "Alle") return ListData.instance.recipes;

        List<Recipe> showList = new List<Recipe>();
        foreach (var recipe in ListData.instance.recipes)
        {
            if (dropdown.captionText.text == "Markiert" && recipe.marked
              || recipe.type.ToString() == dropdown.captionText.text) showList.Add(recipe);
        }
        return showList;
    }

    private List<Recipe> SearchIngredience(string[] ingredients, List<Recipe> showList)
    {
        Debug.Log("Zutat gefunden");
        if (ingredients.Length == 0 || ingredients[0] == "") return showList;

        //iterate over each recipe
        List<Recipe> newShowList = new List<Recipe>();
        List<Recipe> recipeSelection = showList.Count == 0 ? FilterRecipeType() : showList;
        foreach (var recipe in recipeSelection)
        {
            if (recipe.ingredients == "") continue;
            bool containsIngredients = true;
            //iterate over each tag
            for (int i = 0; i < ingredients.Length; i++)
            {
                if (ingredients[i] == "") continue;
                string[] ingredientWords = Regex.Split(recipe.ingredients.ToLower(), @"\r?\n|[-*]|[/]").Where(part => !string.IsNullOrEmpty(part)).Select(part => part.Trim()).ToArray();
                if (!ingredientWords.Contains(ingredients[i].ToLower()))
                {
                    containsIngredients = false;
                    break;
                }
            }
            if (containsIngredients) newShowList.Add(recipe);
        }
        return newShowList;
    }

    private List<Recipe> SearchName(string searchString, List<Recipe> showList)
    {
        List<Recipe> newShowList = new List<Recipe>();
        foreach (var recipe in showList)
        {
            if (recipe.name.ToLower().Contains(searchString)) newShowList.Add(recipe);
        }
        return newShowList;
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
        GetSearchResult();
    }
}
