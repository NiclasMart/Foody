using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodList : MonoBehaviour
{
    List<Recipe> recipes = new List<Recipe>();
    public FoodList instance = null;

    private void Awake() 
    {
    
      if (instance != null)
      {
        Destroy(this);
        return;
      }
      instance = this;
      DontDestroyOnLoad(this);
    }
}
