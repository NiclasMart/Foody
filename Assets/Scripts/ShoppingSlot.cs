using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingSlot : MonoBehaviour
{
  string itemName;

  public void Initialize(KeyValuePair<string, int> item)
  {
    this.itemName = item.Key;
    ShowText();
  }

  public void AddItemToCard()
  {
    ListData.instance.purchases[itemName] += 1;
    ShowText();

    ShoppingListHandler listHandler = FindObjectOfType<ShoppingListHandler>();
    if (listHandler) listHandler.ShowCompleteList();

    ListData.instance.SaveShoppingList();

  }

  public void RemoveSlot()
  {
    FindObjectOfType<ShoppingListHandler>()?.DeleteItemFromList(itemName);
  }

  private void ShowText()
  {
    string displayedText = itemName + " " + ListData.instance.purchases[itemName] + "x";
    GetComponentInChildren<TextMeshProUGUI>().text = displayedText;
  }
}
