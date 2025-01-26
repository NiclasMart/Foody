using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingListHandler : ListHandler
{
    public static readonly Color[] groupColor = new Color[] { new Color(1f, 1f, 1f), new Color(0.88f, 0.73f, 0.33f), new Color(0.88f, 0.33f, 0.41f), new Color(0.33f, 0.63f, 0.88f) };
    [SerializeField] ShoppingSlot shoppingSlot;
    [SerializeField] InputFunctionality purchaseIn;
    [SerializeField] Transform displayGroupSwitchIcon;

    private void Awake()
    {
        purchaseIn.GetComponent<TMP_InputField>().onSubmit.AddListener(ShowOnSubmit);
    }

    public override void ShowCompleteList()
    {
        int currentDisplayGroup = ListData.instance.activePurchaseGroup;
        if (currentDisplayGroup == -1) ShowDictionary(ListData.instance.purchases);
        else
        {
            var purchaseDisplayList = ListData.instance.purchases.Where(x =>
                x.Value[1] == currentDisplayGroup || currentDisplayGroup == -1)
                .ToDictionary(x => x.Key, x => x.Value);
            ShowDictionary(purchaseDisplayList);
        }
        SetDisplayGroupSwitchIconColor();
    }

    public void ShowDictionary(Dictionary<string, int[]> list)
    {
        base.DeleteList();

        foreach (var item in list.OrderBy(key => key.Key))
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
        ListData.instance.purchases = new Dictionary<string, int[]>();
        ListData.instance.SaveShoppingList();
        ShowCompleteList();
    }

    public void SwitchDisplayGroup()
    {
        ListData.instance.activePurchaseGroup++;
        if (ListData.instance.activePurchaseGroup > 3) ListData.instance.activePurchaseGroup = -1;
        ShowCompleteList();
    }

    private void SetDisplayGroupSwitchIconColor()
    {
        Color displayColor = ListData.instance.activePurchaseGroup == -1 ? Color.black : groupColor[ListData.instance.activePurchaseGroup];
        foreach (var image in displayGroupSwitchIcon.GetComponentsInChildren<Image>())
        {
            Debug.Log(displayColor);
            image.color = displayColor;
        }
    }

    IEnumerator UpdateList()
    {
        yield return new WaitForEndOfFrame();
        ShowCompleteList();
    }
}
