using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TagCard : MonoBehaviour
{
  string foodTag;

  public void Initialize(string tag)
  {
    foodTag = tag;
    GetComponentInChildren<TextMeshProUGUI>().text = tag;
  }

  public void SelectTag()
  {
    FindObjectOfType<RecipeAdder>().FillInTag(foodTag);
    transform.parent.GetComponent<TagFinder>().ClearList();
  }

  public string GetTag()
  {
    return foodTag;
  }

  public bool IsSelected()
  {
    return GetComponentInChildren<Toggle>().isOn;
  }

  public void ToggleSelect(bool on)
  {
    GetComponentInChildren<Toggle>().isOn = on;
  }
}
