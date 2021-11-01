using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagSearchbar : MonoBehaviour
{
  [SerializeField] Searchbar searchbar;
  [SerializeField] TagCard tagCard;
  [SerializeField] Transform contentTransform;

  private void Awake()
  {
    DisplayAllTags();
  }

  public void Toggle()
  {
    gameObject.SetActive(!gameObject.activeSelf);
  }

  public void DisplayAllTags()
  {
    List<string> tags = FindAllTags();

    foreach (var tag in tags)
    {
      TagCard card = Instantiate(tagCard, contentTransform);
      card.Initialize(tag);
    }
  }

  public List<string> FindAllTags()
  {
    List<string> uniqueTags = new List<string>();
    foreach (var recipe in ListData.instance.recipes)
    {
      foreach (string tag in recipe.tags)
      {
        if (!uniqueTags.Contains(tag)) uniqueTags.Add(tag);
      }
    }
    uniqueTags.Sort();
    return uniqueTags;
  }

  public void DeleteList()
  {
    foreach (Transform child in contentTransform)
    {
      if (child == contentTransform) continue;
      Destroy(child.gameObject);
    }
  }

  internal void RefreshTagList()
  {
    DeleteList();
    DisplayAllTags();
  }
}
