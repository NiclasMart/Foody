using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplay : MonoBehaviour
{
  [SerializeField] RectTransform showCard, editCard;
  [SerializeField] TextMeshProUGUI nameField, linkField, dateField, tagsField, ingredienceField, descriptionField;
  [SerializeField] InputFunctionality nameIn, linkIn, tagsIn, ingredienceIn, descriptionIn;
  ListHandler listHandler;
  Recipe displayedRecipe;

  private void Awake()
  {
    listHandler = FindObjectOfType<ListHandler>();
  }
  public void Display(Recipe recipe)
  {
    nameField.text = recipe.name;
    linkField.text = BuildHyperlink(recipe.link);
    dateField.text = recipe.date;
    tagsField.text = TagsToString(recipe.tags);
    ingredienceField.text = recipe.ingredients;
    descriptionField.text = recipe.description;
    dateField.text = "Zuletzt gekocht: " + recipe.date;


    displayedRecipe = recipe;
  }

  public void Delete()
  {
    ListData.instance.DeleteRecipe(displayedRecipe);
    ListData.instance.SaveRecipeList();
    listHandler.ShowCompleteList();
    ToggleRecipeDisplay(false);
  }

  public void UpdateCookingDate()
  {
    displayedRecipe.date = DateTime.Now.Date.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"));
    Display(displayedRecipe);
    ListData.instance.SaveRecipeList();
  }

  public void EnableEditingMode()
  {
    GetComponent<Animator>().SetBool("EditActive", true);
    GetComponent<ScrollRect>().content = editCard;
    ToggleEditingMode(true);
    FillContentInInputFields();
  }

  public void ResetEditingMode()
  {
    ToggleEditingMode(false);
    Display(displayedRecipe);
    GetComponent<Animator>().SetBool("EditActive", false);
    GetComponent<ScrollRect>().content = showCard;
  }

  public void SaveEditingChanges()
  {
    displayedRecipe.name = nameIn.GetValue();
    displayedRecipe.link = linkIn.GetValue();
    displayedRecipe.tags = RecipeAdder.StringToList(tagsIn.GetValue());
    displayedRecipe.description = descriptionIn.GetValue();
    displayedRecipe.ingredients = ingredienceIn.GetValue();

    listHandler.ShowCompleteList();
    ListData.instance.SaveRecipeList();

    ResetEditingMode();
  }

  public void ToggleRecipeDisplay(bool on)
  {
    listHandler.gameObject.SetActive(!on);
    transform.GetChild(0).gameObject.SetActive(on);
    if (!on) displayedRecipe = null;
  }

  public void ToggleEditingMode(bool on)
  {
    editCard.gameObject.SetActive(on);
    showCard.gameObject.SetActive(!on);
  }

  public void AddRecipeToFoodList()
  {
    if (ListData.instance.foods.Contains(displayedRecipe)) return;
    ListData.instance.foods.Add(displayedRecipe);
    ListData.instance.SaveFoodList();
  }

  private void FillContentInInputFields()
  {
    nameIn.SetValue(displayedRecipe.name);
    linkIn.SetValue(displayedRecipe.link);
    tagsIn.SetValue(TagsToString(displayedRecipe.tags));
    ingredienceIn.SetValue(displayedRecipe.ingredients);
    descriptionIn.SetValue(displayedRecipe.description);
  }

  private string TagsToString(List<string> tags)
  {
    string tagString = "";
    foreach (var tag in tags)
    {
      tagString += (tag + ", ");
    }
    if (tagString != "") tagString = tagString.Remove(tagString.Length - 2, 2);
    return tagString;
  }

  private string BuildHyperlink(string link)
  {
    return "<link=" + link + "><color=blue>Link zum Rezept</color></link>";
  }


}
