using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImportHandler : MonoBehaviour
{
    private void Awake() 
    {
      gameObject.SetActive(CheckForImportData());
    }

    private bool CheckForImportData()
    {
      string path = Path.Combine(Application.persistentDataPath, "ExportData");
      return Directory.Exists(path);
    }

    //TODO: remove
    public void Import()
    {
    //   List<Recipe> newRecipes = SavingSystem.ImportData("ExportRecipeList");

    //   foreach (var recipe in newRecipes)
    //   {
    //     ListData.instance.AddRecipe(recipe);
    //   }
    //   gameObject.SetActive(false);
    }
}
