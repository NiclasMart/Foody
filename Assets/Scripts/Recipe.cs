using System;
using System.Collections.Generic;

public enum RecipeType
{
  cook,
  bake,
  other
}

[System.Serializable]
public class Recipe : IComparable<Recipe>
{
  public string name;
  public string picture;
  public RecipeType type;
  public string link;
  public string description;
  public string ingredients;
  public List<string> tags = new List<string>();
  public string date = "---";

  public Recipe(string name)
  {
    this.name = name;
  }

  public int CompareTo(Recipe other)
  {
    return this.name.CompareTo(other.name);
  }

  public DateTime GetDate()
  {
    DateTime returnDate;
    if (!DateTime.TryParse(this.date, out returnDate)) returnDate = DateTime.MinValue;
    return returnDate;
  }
}
