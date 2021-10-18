using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeDisplay : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI nameField;
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
    linkIn.SetValue(recipe.link);
    tagsIn.SetValue(TagsToString(recipe.tags));
    ingredienceIn.SetValue(recipe.ingredience);
    descriptionIn.SetValue(recipe.description);

    displayedRecipe = recipe;
  }

  private string TagsToString(List<string> tags)
  {
    string tagString = "";
    foreach (var tag in tags)
    {
      tagString += tag + " ";
    }
    return tagString;
  }

  public void Delete()
  {
    FoodList.instance.DeleteRecipe(displayedRecipe);
    SavingSystem.Save("FoodList");
    listHandler.RefreshList();
    ToggleRecipeDisplay(false);
  }

  public void EnableEditingMode()
  {
    GetComponent<Animator>().SetBool("EditActive", true);
    nameIn.SetValue(displayedRecipe.name);
    ToggleEditingMode(true);
  }

  public void ResetEditingMode()
  {
    ToggleEditingMode(false);
    Display(displayedRecipe);
    GetComponent<Animator>().SetBool("EditActive", false);
  }

  public void SaveEditingChanges()
  {
    displayedRecipe.name = nameIn.GetValue();
    displayedRecipe.link = linkIn.GetValue();
    displayedRecipe.tags = RecipeAdder.StringToList(tagsIn.GetValue());
    displayedRecipe.description = descriptionIn.GetValue();
    displayedRecipe.ingredience = ingredienceIn.GetValue();

    listHandler.RefreshList();
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
    linkIn.ToggleInteractionState(on);
    tagsIn.ToggleInteractionState(on);
    ingredienceIn.ToggleInteractionState(on);
    descriptionIn.ToggleInteractionState(on);
  }
}
