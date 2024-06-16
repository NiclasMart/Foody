using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
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

    foreach (KeyValuePair<string, int> item in list.OrderBy(key => key.Key))
    {
      ShoppingSlot slot = Instantiate(shoppingSlot, listTransform);
      slot.Initialize(item);
    }
  }

  public void ShowOnSubmit(string msg)
  {
    StartCoroutine(UpdateList());
  }

  public void DeleteItemFromList(string name)
  {
    ListData.instance.purchases.Remove(name);
    ListData.instance.SaveShoppingList();
    ShowCompleteList();
  }

  public void DeleteCompleteList()
  {
    ListData.instance.purchases = new Dictionary<string, int>();
    ListData.instance.SaveShoppingList();
    ShowCompleteList();
  }

  IEnumerator UpdateList()
  {
    yield return new WaitForEndOfFrame();
    ShowCompleteList();
  }
}
