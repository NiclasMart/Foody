using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingSlot : MonoBehaviour
{
  string itemName;

  public void Initialize(string name)
  {
    itemName = name;
    ShowText(name);
  }

  public void AddItemToCard()
  {
    ListData.instance.purchases[itemName] += 1;
    ShowText(itemName);
  }

  private void ShowText(string name)
  {
    string displayedText = name + " " + ListData.instance.purchases[name] + "x";
    GetComponentInChildren<TextMeshProUGUI>().text = displayedText;
  }
}
