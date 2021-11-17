using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class TagSearchbar : MonoBehaviour
{
  [SerializeField] Searchbar searchbar;
  [SerializeField] TagCard tagCard;
  [SerializeField] Transform contentTransform;
  [SerializeField] RecipeListHandler listHandler;

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
    List<string> tags;
    tags = (listHandler != null) ? FindAllTagsInListHandler() : FindAllAvailableTags();
    tags.Sort();

    foreach (var tag in tags)
    {
      TagCard card = Instantiate(tagCard, contentTransform);
      card.Initialize(tag);
    }
  }

  private List<string> FindAllAvailableTags()
  {
    List<string> tags = new List<string>();
    foreach (var recipe in ListData.instance.recipes)
    {
      foreach (var tag in recipe.tags)
      {
        if (tags.Contains(tag)) continue;
        tags.Add(tag);
      }
    }
    return tags;
  }

  public void FillTagsInSearchbar()
  {
    string searchTerm = "#tag ";
    foreach (var tagSlot in contentTransform.GetComponentsInChildren<TagCard>())
    {
      if (tagSlot.IsSelected()) searchTerm = searchTerm + tagSlot.GetTag() + " ";
    }
    searchbar.Fill(searchTerm);
  }

  public List<string> FindAllTagsInListHandler()
  {
    List<string> uniqueTags = new List<string>();
    foreach (var recipe in listHandler.displayedList)
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

  public void FindTagsInInputField(TMP_InputField field)
  {
    List<string> tagsInField = RecipeAdder.StringToList(field.text);
    List<TagCard> cards = contentTransform.GetComponentsInChildren<TagCard>().ToList();
    foreach (var card in cards)
    {
      card.ToggleSelect(tagsInField.Contains(card.GetTag()));
    }
  }

  public void FillTagsInInputField(TMP_InputField field)
  {
    string newTagTerm = "";
    List<string> existingTags = new List<string>();
    foreach (var tagSlot in contentTransform.GetComponentsInChildren<TagCard>())
    {
      if (tagSlot.IsSelected()) newTagTerm = newTagTerm + tagSlot.GetTag() + ", ";
      existingTags.Add(tagSlot.GetTag());
    }

    foreach (var tag in RecipeAdder.StringToList(field.text))
    {
      if (newTagTerm.Contains(tag) || existingTags.Contains(tag)) continue;
      newTagTerm = newTagTerm + tag + ", ";
    }
    field.text = newTagTerm;
  }

  public void RefreshTagList()
  {
    DeleteList();
    DisplayAllTags();
  }
}
