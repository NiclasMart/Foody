using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingListHandler : ListHandler
{
  [SerializeField] ShoppingSlot shoppingSlot;
  [SerializeField] InputFunctionality purchaseIn;

  private void Awake()
  {
    purchaseIn.GetComponent<TMP_InputField>().onSubmit.AddListener(ShowOnSubmit);
  }

  public override void ShowCompleteList()
  {
    ShowDictionary(ListData.instance.purchases);
  }

  public void ShowDictionary(Dictionary<string, int> list)
  {
    base.DeleteList();

    foreach (var item in list)
    {
      ShoppingSlot slot = Instantiate(shoppingSlot, listTransform);
      slot.Initialize(item);
    }
  }

  public void ShowOnSubmit(string msg)
  {
    ShowCompleteList();
  }

  public void DeleteItemFromList(string name)
  {
    ListData.instance.purchases.Remove(name);
    ListData.instance.SaveShoppingList();
    ShowCompleteList();
  }
}
