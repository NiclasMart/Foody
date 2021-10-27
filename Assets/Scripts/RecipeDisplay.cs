using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplay : MonoBehaviour
{
  [SerializeField] RectTransform showCard, editCard;
  [SerializeField] ShoppingAdder shoppingAdder;
  [SerializeField] TextMeshProUGUI nameField, linkField, dateField, tagHeader, tagsField, ingredienceField, descriptionHeader, descriptionField;
  [SerializeField] Image pictureField, pictureIn, foodListButton;
  [SerializeField] TMP_Dropdown dropdown;
  [SerializeField] AddAndroidePicture addAndroidePicture;
  [SerializeField] InputFunctionality nameIn, linkIn, tagsIn, ingredienceIn, descriptionIn;
  string tmpPictureName = "";
  ListHandler listHandler;
  Recipe displayedRecipe;

  private void Awake()
  {
    listHandler = FindObjectOfType<ListHandler>();
    ToggleShoppingAdder(null);
    if (addAndroidePicture) addAndroidePicture.onTakePicture += AddPicture;
  }
  public void Display(Recipe recipe)
  {
    nameField.text = recipe.name;

    linkField.gameObject.SetActive(recipe.link != "");
    linkField.text = BuildHyperlink(recipe.link);

    string tags = TagsToString(recipe.tags);
    tagHeader.gameObject.SetActive(tags != "");
    tagsField.text = tags;

    ingredienceField.text = recipe.ingredients;

    descriptionHeader.gameObject.SetActive(recipe.description != "");
    descriptionField.text = recipe.description;

    dateField.text = "Zuletzt gekocht: " + recipe.date;

    pictureField.sprite = SavingSystem.LoadImageFromFile(recipe.picture);
    pictureField.gameObject.SetActive(pictureField.sprite != null);

    SetButtonColor(ListData.instance.foods.Find(x => x.name == recipe.name) != null);
    displayedRecipe = recipe;
  }

  public void Delete()
  {
    SavingSystem.DeletePicture(displayedRecipe.picture);
    ListData.instance.DeleteRecipe(displayedRecipe);
    ListData.instance.SaveRecipeList();
    listHandler.ShowCompleteList();
    ToggleRecipeDisplay(false);
  }

  public void UpdateCookingDate()
  {
    displayedRecipe.date = DateTime.Now.Date.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"));
    Display(displayedRecipe);
    ListData.instance.SaveFoodList();

    Recipe recipe = ListData.instance.recipes.Find(x => x.name == displayedRecipe.name);
    if (recipe != null) recipe.date = displayedRecipe.date;
    ListData.instance.SaveRecipeList();
  }

  public void EnableEditingMode()
  {
    GetComponent<Animator>().SetBool("EditActive", true);
    GetComponent<ScrollRect>().content = editCard;
    shoppingAdder.gameObject.SetActive(false);
    ToggleEditingMode(true);
    StartCoroutine(FillContentInInputFields());
  }

  public void ResetEditingMode()
  {
    ToggleEditingMode(false);
    Display(displayedRecipe);
    //delete picture if one was made
    if (tmpPictureName != displayedRecipe.picture) SavingSystem.DeletePicture(tmpPictureName);
    tmpPictureName = "";
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
    displayedRecipe.SetRecipeType(dropdown.captionText.text);

    if (tmpPictureName != "")
    {
      SavingSystem.DeletePicture(displayedRecipe.picture);
      displayedRecipe.picture = tmpPictureName;
    }

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
    if (ListData.instance.foods.Find(x => x.name == displayedRecipe.name) != null)
    {
      ListData.instance.foods.RemoveAll(x => x.name == displayedRecipe.name);
      SetButtonColor(false);
    }
    else
    {
      ListData.instance.foods.Add(displayedRecipe);
      SetButtonColor(true);
    }
    ListData.instance.SaveFoodList();
  }

  private void SetButtonColor(bool foodInList)
  {
    if (!foodListButton) return;
    if (foodInList) foodListButton.color = new Color32(43, 137, 35, 255);
    else foodListButton.color = Color.white;
  }

  public void AddPicture(string name)
  {
    if (tmpPictureName != "") SavingSystem.DeletePicture(tmpPictureName);
    tmpPictureName = name;
  }

  public void ToggleShoppingAdder(Image btn)
  {
    if (!shoppingAdder) return;
    bool isActive = shoppingAdder.gameObject.activeSelf;
    shoppingAdder.gameObject.SetActive(!isActive);
    if (btn) btn.color = !isActive ? new Color32(43, 137, 35, 255) : new Color32(255, 255, 255, 255);
    if (!isActive) shoppingAdder.gameObject.GetComponentInChildren<TMP_InputField>().Select();

  }

  private IEnumerator FillContentInInputFields()
  {
    yield return new WaitForEndOfFrame();

    nameIn.SetValue(displayedRecipe.name);
    linkIn.SetValue(displayedRecipe.link);
    tagsIn.SetValue(TagsToString(displayedRecipe.tags));
    ingredienceIn.SetValue(displayedRecipe.ingredients);
    descriptionIn.SetValue(displayedRecipe.description);
    pictureIn.sprite = SavingSystem.LoadImageFromFile(displayedRecipe.picture);
    dropdown.value = (int)displayedRecipe.type;
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
