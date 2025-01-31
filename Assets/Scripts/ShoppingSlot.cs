using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingSlot : MonoBehaviour
{
  string itemName;

  public void Initialize(KeyValuePair<string, int[]> item)
  {
    itemName = item.Key;
    DisplayItemInfo();
  }

  public void AddItemToCard()
  {
    ListData.instance.purchases[itemName][0] += 1;
    UpdateData();
  }

  public void RemoveItemFromCard()
  {
    ListData.instance.purchases[itemName][0] -= 1;
    if (ListData.instance.purchases[itemName][0] <= 0)
    {
      ListData.instance.purchases.Remove(itemName);
      ListData.instance.SaveShoppingList();
      ShoppingListHandler listHandler = FindObjectOfType<ShoppingListHandler>();
      if (listHandler) listHandler.ShowCompleteList();
      Destroy(gameObject);
      return;
    }

    UpdateData();
  }

  public void SwitchGroup()
  {
    ListData.instance.purchases[itemName][1]++;
    if (ListData.instance.purchases[itemName][1] > 3) ListData.instance.purchases[itemName][1] = 0;
    UpdateData();
  }

  private void UpdateData()
  {
    DisplayItemInfo();

    ShoppingListHandler listHandler = FindObjectOfType<ShoppingListHandler>();
    if (listHandler) listHandler.ShowCompleteList();

    ListData.instance.SaveShoppingList();
  }

  public void RemoveSlot()
  {
    FindObjectOfType<ShoppingListHandler>()?.DeleteItemFromList(itemName);
  }

  private void DisplayItemInfo()
  {
    string displayedText = itemName + " " + ListData.instance.purchases[itemName][0] + "x";
    GetComponentInChildren<TextMeshProUGUI>().text = displayedText;
    GetComponent<Image>().color = ShoppingListHandler.groupColor[ListData.instance.purchases[itemName][1]];
  }
}
