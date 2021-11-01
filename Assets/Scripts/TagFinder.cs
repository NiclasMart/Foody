using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagFinder : MonoBehaviour
{
  [SerializeField] TagCard card;
    public void DisplayTags(List<string> tagList)
    {
      int tagAmount = 0;
      foreach (string tag in tagList)
      {
        TagCard newCard = Instantiate(card, transform);
        newCard.Initialize(tag);
        tagAmount++;
        if (tagAmount == 5) break;
      }
    }

    public void ClearList()
    {
      foreach (Transform child in transform)
      {
        if (child == transform) continue;
        Destroy(child.gameObject);
      }
    }
}
