using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

  public static class SavingSystem
  {
    public static void Save(string saveFile)
    {
      List<Recipe> data = FoodList.instance.GetRecipes();
      SaveFile(saveFile, data);
    }

    public static void Load(string saveFile)
    {
      object data = LoadFile(saveFile);
      if (data != null) FoodList.instance.SetRecipes((List<Recipe>)data);
      else FoodList.instance.SetRecipes(new List<Recipe>());
    }

    private static string GetPathFromSaveFile(string saveFile)
    {
      return Path.Combine(Application.persistentDataPath, saveFile + ".eat");
    }

    private static void SaveFile(string saveFile, object data)
    {
      string path = GetPathFromSaveFile(saveFile);
      Debug.Log("Saving to " + path);

      using (FileStream stream = File.Open(path, FileMode.Create))
      {
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, data);
      }
    }

    private static object LoadFile(string saveFile)
    {
      string path = GetPathFromSaveFile(saveFile);
      Debug.Log("Loading from " + path);
      if (!File.Exists(path))
      {
        return null;
      }
      using (FileStream stream = File.Open(path, FileMode.Open))
      {
        BinaryFormatter formatter = new BinaryFormatter();
        return formatter.Deserialize(stream);
      }
    }
  }

