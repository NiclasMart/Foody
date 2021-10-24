using System;
using System.Collections.Generic;

public enum RecipeType
{
  Kochen,
  Backen,
  Sonstiges
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
}
