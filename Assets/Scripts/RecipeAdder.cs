using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;

public class RecipeAdder : MonoBehaviour
{
  [SerializeField] InputFunctionality nameIn, linkIn, tagsIn, ingredienceIn, descriptionIn;
  [SerializeField] TMP_Dropdown dropdown;
  [SerializeField] Image picture;
  string tmpPictureName = "";

  private void Awake()
  {
    GetComponentInChildren<AddAndroidePicture>().onTakePicture += AddPicture;
  }

  public void AddNewRecipe()
  {
    if (nameIn.GetValue() == "" || nameIn.GetValue() == " ") return;

    Recipe newRecipe = new Recipe(nameIn.GetValue());
    newRecipe.link = linkIn.GetValue();
    newRecipe.tags = StringToList(tagsIn.GetValue());
    newRecipe.ingredients = ingredienceIn.GetValue();
    newRecipe.description = descriptionIn.GetValue();
    newRecipe.type = EvaluateRecipeType();
    newRecipe.picture = tmpPictureName;
    ClearInputFields();

    ListData.instance.AddRecipe(newRecipe);
    ListData.instance.SaveRecipeList();
  }

  private RecipeType EvaluateRecipeType()
  {
    switch (dropdown.captionText.text)
    {
      case "Kochen": return RecipeType.cook;
      case "Backen": return RecipeType.bake;
      default: return RecipeType.other;
    }
  }

  public static List<string> StringToList(string stringValue)
  {
    if (stringValue == "" || stringValue == " ") return new List<string>();
    stringValue = stringValue.ToLower();
    stringValue = stringValue.Replace(" ", "");
    return stringValue.Split(',').ToList();
  }

  public void AddPicture(string name)
  {
    if (tmpPictureName != "") SavingSystem.DeletePicture(tmpPictureName);
    tmpPictureName = name;
  }

  public void ClearUnusedData()
  {
    if (tmpPictureName != "") SavingSystem.DeletePicture(tmpPictureName);
    ClearInputFields();
  }

  void ClearInputFields()
  {
    nameIn.ClearField();
    linkIn.ClearField();
    tagsIn.ClearField();
    ingredienceIn.ClearField();
    descriptionIn.ClearField();
    tmpPictureName = "";
    picture.sprite = null;
    dropdown.captionText.text = "Kochen";
  }

}
