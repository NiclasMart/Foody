using System;
using System.Collections.Generic;

[System.Serializable]
public class Recipe : IComparable<Recipe>
{
  public string name;
  //public Image picture;
  public string link;
  public string description;
  public string ingredients;
  public List<string> tags = new List<string>();
  public string date = "---";

  public int CompareTo(Recipe other)
  {
    return this.name.CompareTo(other.name);
  }

  public DateTime GetDate()
  {
    DateTime returnDate;
    // if (!DateTime.TryParse(this.date, CultureInfo.CreateSpecificCulture("de-DE"), 0,  out returnDate)) 
    // {
    //   returnDate = DateTime.MinValue;
    // }
    if (!DateTime.TryParse(this.date, out returnDate)) returnDate = DateTime.MinValue;
    return returnDate;
  }
}
