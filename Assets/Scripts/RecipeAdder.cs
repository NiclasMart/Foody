using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class RecipeAdder : MonoBehaviour
{
  [SerializeField] InputFunctionality nameIn, linkIn, tagsIn, ingredienceIn, descriptionIn;
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
    newRecipe.picture = tmpPictureName;
    ClearInputFields();

    ListData.instance.AddRecipe(newRecipe);
    ListData.instance.SaveRecipeList();
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
  }

}
