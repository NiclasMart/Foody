using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;

public class RecipeAdder : MonoBehaviour
{
  [SerializeField] InputFunctionality nameIn, linkIn, tagsIn, ingredienceIn, descriptionIn, noteIn;
  [SerializeField] TMP_Dropdown dropdown;
  [SerializeField] Image picture, errorAlert;
  [SerializeField] PictureAdder pictureAdder;
  string tmpPictureName = "";

  private void Awake()
  {
    if (pictureAdder != null) pictureAdder.onTakePicture += AddPicture;
  }

  public void AddNewRecipe()
  {
    if (nameIn.GetValue() == ""
      || nameIn.GetValue() == " "
      || ListData.instance.recipes.Find(x => x.name == nameIn.GetValue()) != null)
    {
      StartCoroutine(ShowingAlert());
      return;
    }

    Recipe newRecipe = new Recipe(nameIn.GetValue(), true);
    newRecipe.link = linkIn.GetValue();
    newRecipe.tags = StringToList(tagsIn.GetValue());
    newRecipe.ingredients = ingredienceIn.GetValue();
    newRecipe.description = descriptionIn.GetValue();
    newRecipe.SetRecipeType(dropdown.captionText.text);
    newRecipe.picture = tmpPictureName;
    newRecipe.note = noteIn.GetValue();
    ClearInputFields();

    ListData.instance.AddRecipe(newRecipe);
    ListData.instance.SaveRecipeList();
  }

  public static List<string> StringToList(string stringValue)
  {
    if (stringValue == "" || stringValue == " ") return new List<string>();
    stringValue = stringValue.ToLower();
    stringValue = stringValue.Replace(" ", "");
    List<string> tagList = stringValue.Split(',').ToList();
    tagList.Remove("");
    return tagList;
  }

  public void FillInTag(string newTag)
  {
    List<string> tagList = StringToList(tagsIn.GetValue());
    tagList[tagList.Count - 1] = newTag;

    string tagString = "";
    foreach (var tag in tagList)
    {
      tagString += (tag + ", ");
    }
    tagsIn.SetValue(tagString);
    tagsIn.GetComponent<TMP_InputField>().caretPosition = tagString.Length;
    tagsIn.Select();
  }

  public void SavePicture(Recipe recipe)
  {
    if (tmpPictureName != "")
    {
      SavingSystem.DeletePicture(recipe.picture);
      recipe.picture = tmpPictureName;
      tmpPictureName = "";
    }
  }

  public void ResetPicture()
  {
    AddPicture("");
  }

  public void AddPicture(string name)
  {
    if (tmpPictureName != "") SavingSystem.DeletePicture(tmpPictureName);
    tmpPictureName = name;
  }

  public void ClearUnusedData()
  {
    AddPicture("");
    ClearInputFields();
  }

  void ClearInputFields()
  {
    nameIn.ClearField();
    linkIn.ClearField();
    tagsIn.ClearField();
    ingredienceIn.ClearField();
    descriptionIn.ClearField();
    picture.sprite = null;
    tmpPictureName = "";
    dropdown.value = 0;
    noteIn.ClearField();
  }

  IEnumerator ShowingAlert()
  {
    errorAlert.gameObject.SetActive(true);
    yield return new WaitForSeconds(3f);
    errorAlert.gameObject.SetActive(false);
  }

}
