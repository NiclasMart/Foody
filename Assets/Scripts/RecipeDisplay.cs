using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeDisplay : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI nameField;
  [SerializeField] TMP_InputField linkIn, tagsIn, ingredienceIn, descriptionIn;
  FoodListHandler listHandler;

  private void Awake()
  {
    listHandler = FindObjectOfType<FoodListHandler>();
  }
  public void Display(Recipe recipe)
  {
    nameField.text = recipe.name;
    linkIn.text = recipe.link;
    //tagsIn.text = recipe.tags;
    Debug.Log(recipe.ingredience);
    ingredienceIn.text = recipe.ingredience;
    descriptionIn.text = recipe.description;
  }

  public void ToggleRecipeDisplay(bool on)
  {
    listHandler.gameObject.SetActive(!on);
    transform.GetChild(0).gameObject.SetActive(on);
  }
}
