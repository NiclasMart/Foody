using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Recipe
{
  public string name;
  //public Image picture;
  public string link;
  public string description;
  public string ingredience;
  public List<SearchTags> tags = new List<SearchTags>();



}
