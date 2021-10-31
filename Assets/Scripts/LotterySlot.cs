using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LotterySlot : RecipeSlot
{
  [SerializeField] Image slotImage;
  [SerializeField] Sprite defaultImage;


  public override void Initialize(Recipe recipe)
  {

    if (recipe == null)
    {
      gameObject.SetActive(false);
      return;
    }
    else gameObject.SetActive(true);

    base.Initialize(recipe);
    Sprite image = recipe.GetImage();
    if (image != null) slotImage.sprite = image;
    else slotImage.sprite = defaultImage;
  }
}
