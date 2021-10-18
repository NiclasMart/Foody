using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class RecipeAdder : MonoBehaviour
{
  [SerializeField] InputFunctionality nameIn, linkIn, tagsIn, ingredienceIn, descriptionIn;

  public void AddNewRecipe()
  {
    Recipe newRecipe = new Recipe();
    newRecipe.name = nameIn.GetValue();
    newRecipe.link = linkIn.GetValue();
    newRecipe.tags = StringToList(tagsIn.GetValue());
    newRecipe.ingredience = ingredienceIn.GetValue();
    newRecipe.description = descriptionIn.GetValue();

    ClearInputFields();

    FoodList.instance.AddRecipe(newRecipe);
    SavingSystem.Save("FoodList");
  }

  public static List<string> StringToList(string stringValue)
  {
    stringValue.ToLower();
    stringValue.Replace(" ", "");
    return stringValue.Split(',').ToList();
  }

  void ClearInputFields()
  {
    nameIn.ClearField();
    linkIn.ClearField();
    tagsIn.ClearField();
    ingredienceIn.ClearField();
    descriptionIn.ClearField();
  }

}
