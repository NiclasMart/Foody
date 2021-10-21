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
  FoodListHandler listHandler;
  Recipe displayedRecipe;

  private void Awake()
  {
    listHandler = FindObjectOfType<FoodListHandler>();
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

  public void Delete()
  {
    FoodList.instance.DeleteRecipe(displayedRecipe);
    SavingSystem.Save("FoodList");
    listHandler.ShowCompleteList();
    ToggleRecipeDisplay(false);
  }

  public void UpdateCookingDate()
  {
    displayedRecipe.date = DateTime.Now.Date.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"));
    Display(displayedRecipe);
    SavingSystem.Save("FoodList");
  }

  public void EnableEditingMode()
  {
    GetComponent<Animator>().SetBool("EditActive", true);
    GetComponent<ScrollRect>().content = editCard;
    ToggleEditingMode(true);
    FillContentInInputFields();
  }

  private void FillContentInInputFields()
  {
    nameIn.SetValue(displayedRecipe.name);
    linkIn.SetValue(displayedRecipe.link);
    tagsIn.SetValue(TagsToString(displayedRecipe.tags));
    ingredienceIn.SetValue(displayedRecipe.ingredients);
    descriptionIn.SetValue(displayedRecipe.description);
  }

  string BuildHyperlink(string link)
  {
    return "<link=" + link + "><color=blue>Link zum Rezept</color></link>";
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
    SavingSystem.Save("FoodList");

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
}
