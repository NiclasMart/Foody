using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeAdder : MonoBehaviour
{
  [SerializeField] TMP_InputField nameIn, linkIn, tagsIn, ingredienceIn, descriptionIn;

  public void AddNewRecipe()
  {
    Recipe newRecipe = new Recipe();
    newRecipe.name = nameIn.text;
    newRecipe.link = linkIn.text;
    newRecipe.ingredience = ingredienceIn.text;
    newRecipe.description = descriptionIn.text;

    Debug.Log(newRecipe.name);
    ClearInputFields();

    FoodList.instance.AddRecipe(newRecipe);
    SavingSystem.Save("FoodList");
  }

  void ClearInputFields()
  {
    nameIn.text = "";
    linkIn.text = "";
    tagsIn.text = "";
    ingredienceIn.text = "";
    descriptionIn.text = "";
  }

}