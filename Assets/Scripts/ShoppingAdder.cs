using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class ShoppingAdder : MonoBehaviour
{
    [SerializeField] InputFunctionality input;
    [SerializeField] Image groupSelector;
    [SerializeField] ShoppingSlot shoppingSlot;
    [SerializeField] Transform list;
    private int itemGroup = 0;
    

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

        ListData.instance.AddNewItemToShoppingCard(newItem, amount, itemGroup);
        ListData.instance.SaveShoppingList();

        ClearInput();
    }

    public void SwitchGroup()
    {
        itemGroup++;
        if (itemGroup > 3) itemGroup = 0;
        groupSelector.color = ShoppingListHandler.groupColor[itemGroup];
    }

    public void SearchItemInShoppingList()
    {
        ClearSearchList();

        string searchInput = input.GetValue().ToLower();
        if (searchInput.Length < 2) return;

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

    public void ClearSearchList()
    {
        foreach (Transform item in list)
        {
            if (item == list) continue;
            Destroy(item.gameObject);
        }

    }

    private void ShowSearchItem(KeyValuePair<string, int[]> item)
    {
        ShoppingSlot listItem = Instantiate(shoppingSlot, list);
        listItem.Initialize(item);
    }

    private void ClearSearchBar()
    {
        input.ClearField();
        input.Select();
    }
}
