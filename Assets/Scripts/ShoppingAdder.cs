using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShoppingAdder : MonoBehaviour
{
  [SerializeField] InputFunctionality input;

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

    Debug.Log(ListData.instance.purchases[newItem]);
    input.ClearField();
    input.Select();
  }
}
