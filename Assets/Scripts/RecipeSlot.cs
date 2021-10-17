using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeSlot : MonoBehaviour
{
  Recipe recipe;
  public void Initialize(Recipe recipe)
  {
    this.recipe = recipe;
    GetComponentInChildren<TextMeshProUGUI>().text = recipe.name;
  }
}
