using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
