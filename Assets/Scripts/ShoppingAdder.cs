using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;

public class ShoppingAdder : MonoBehaviour
{
  [SerializeField] InputFunctionality input;
  [SerializeField] ShoppingSlot shoppingSlot;
  [SerializeField] Transform list;

  private void Awake()
  {
    input.gameObject.GetComponent<TMP_InputField>().onSubmit.AddListener(AddNewItem);
  }

  public void AddNewItem(string msg)
  {
    if (Input.GetKeyDown(KeyCode.Escape)) return;

    string newItem = input.GetValue().ToLower();
    if (newItem == "" || newItem == " ") return;

    string potentialAmount = newItem.Split().Last();
    int amount;
    if (int.TryParse(potentialAmount, System.Globalization.NumberStyles.None, null, out amount))
    {
      newItem = Regex.Replace(newItem, "[0-9]", "");
    }
    else amount = 1;

    ListData.instance.AddNewItemToShoppingCard(newItem, amount);
    ListData.instance.SaveShoppingList();

    ClearInput();
  }

  public void SearchItemInShoppingList()
  {
    ClearSearchList();

    string searchInput = input.GetValue().ToLower();
    if (searchInput.Length < 3) return;

    foreach (var item in ListData.instance.purchases)
    {
      if (item.Key.Contains(searchInput)) ShowSearchItem(item);
    }
  }

  public void ClearInput()
  {
    ClearSearchBar();
    ClearSearchList();
  }

  private void ShowSearchItem(KeyValuePair<string, int> item)
  {
    ShoppingSlot listItem = Instantiate(shoppingSlot, list);
    listItem.Initialize(item);
  }

  public void ClearSearchList()
  {
    foreach (Transform item in list)
    {
      if (item == list) continue;
      Destroy(item.gameObject);
    }

  }

  private void ClearSearchBar()
  {
    input.ClearField();
    input.Select();
  }
}
