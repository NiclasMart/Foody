using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Recipe
{
    string name;
    Image picture;
    string link;
    string description;
    List<SearchTags> tags = new List<SearchTags>();

}
