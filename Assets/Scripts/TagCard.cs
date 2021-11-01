using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
}
