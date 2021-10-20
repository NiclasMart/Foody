using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplay : MonoBehaviour
{
  [SerializeField] RectTransform showCard, editCard;
  [SerializeField] TextMeshProUGUI nameField, linkField, tagsField, ingredienceField, descriptionField;
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
    linkField.text = recipe.link;
    tagsField.text = TagsToString(recipe.tags);
    ingredienceField.text = recipe.ingredience;
    descriptionField.text = recipe.description;


    displayedRecipe = recipe;
  }

  private string TagsToString(List<string> tags)
  {
    string tagString = "";
    foreach (var tag in tags)
    {
      tagString += (tag + ", ");
    }
    tagString = tagString.Remove(tagString.Length - 2, 2);
    return tagString;
  }

  public void Delete()
  {
    FoodList.instance.DeleteRecipe(displayedRecipe);
    SavingSystem.Save("FoodList");
    listHandler.ShowCompleteList();
    ToggleRecipeDisplay(false);
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
    ingredienceIn.SetValue(displayedRecipe.ingredience);
    descriptionIn.SetValue(displayedRecipe.description);
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
    displayedRecipe.ingredience = ingredienceIn.GetValue();

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
