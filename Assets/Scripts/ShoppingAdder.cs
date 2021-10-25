using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    if (ListData.instance.purchases.ContainsKey(newItem))
      ListData.instance.purchases[newItem] += 1;
    else
      ListData.instance.purchases.Add(newItem, 1);

    ListData.instance.SaveShoppingList();

    ClearInput();
  }

  public void SearchItemInShoppingList()
  {
    ClearSearchList();

    string searchInput = input.GetValue().ToLower();
    if (searchInput.Length < 3) return;

    foreach (var item in ListData.instance.purchases.Keys)
    {
      if (item.Contains(searchInput)) ShowSearchItem(item);
    }
  }

  public void ClearInput()
  {
    ClearSearchBar();
    ClearSearchList();
  }

  private void ShowSearchItem(string item)
  {
    ShoppingSlot listItem = Instantiate(shoppingSlot, list);
    listItem.Initialize(item);
  }

  private void ClearSearchList()
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
