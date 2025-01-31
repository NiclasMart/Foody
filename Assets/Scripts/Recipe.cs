using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum RecipeType
{
  Kochen,
  Backen,
  Sonstiges
}

[System.Serializable]
public class Recipe : IComparable<Recipe>
{
  public string ID;
  public string name;
  public string picture;
  public RecipeType type;
  public string link;
  public string description;
  public string ingredients;
  public List<string> tags = new List<string>();
  public string date = "---";
  public string creationDate = "---";
  public string note;
  public bool marked;

  public Recipe(string name, bool createID)
  {
    if (createID) ID = Guid.NewGuid().ToString();
    this.name = name;
    picture = "";
    link = "";
    description = "";
    ingredients = "";
    creationDate = DateTime.Now.Date.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"));
    note = "";
    marked = false;
  }

  public int CompareTo(Recipe other)
  {
    return ID.CompareTo(other.ID);
  }

  public DateTime GetCookDate()
  {
    DateTime returnDate;
    if (!DateTime.TryParse(this.date, out returnDate)) returnDate = DateTime.MinValue;
    return returnDate;
  }

  public DateTime GetOriginDate()
  {
    DateTime returnDate;
    if (!DateTime.TryParse(this.creationDate, out returnDate)) returnDate = DateTime.MinValue;
    return returnDate;
  }

  public void SetRecipeType(string type)
  {
    switch (type)
    {
      case "Kochen":
        this.type = RecipeType.Kochen;
        break;
      case "Backen":
        this.type = RecipeType.Backen;
        break;
      default:
        this.type = RecipeType.Sonstiges;
        break;
    }
  }

  public string GetTagsAsString()
  {
    string tagString = "";
    foreach (var tag in tags)
    {
      tagString += (tag + ", ");
    }
    if (tagString != "") tagString = tagString.Remove(tagString.Length - 2, 2);
    return tagString;
  }

  public Sprite GetImage()
  {
    return SavingSystem.LoadImageFromFile(picture);
  }
}
