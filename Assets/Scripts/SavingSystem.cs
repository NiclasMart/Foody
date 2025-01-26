using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SavingSystem
{
  public static string DataPath => Path.Combine(Application.persistentDataPath);
  public static void Save(object data, string saveFile)
  {
    SaveFile(GetPathFromSaveFile(saveFile), data);
  }

  public static object Load(string saveFile)
  {
    return LoadFile(GetPathFromSaveFile(saveFile));
  }

  public static void DeletePicture(string name)
  {
    if (name == "") return;
    string path = Path.Combine(DataPath, name);
    File.Delete(path);
  }

  public static void ExportData(List<Recipe> list, string fileName)
  {
    string path = Path.Combine(Application.persistentDataPath, "ExportData");
    if (Directory.Exists(path)) Directory.Delete(path, true);
    
    Directory.CreateDirectory(path);

    foreach (var recipe in list)
    {
      if (recipe.picture != "")
      {
        string imagePath = Path.Combine(DataPath, recipe.picture);
        string copyPath = Path.Combine(path, recipe.picture);
        File.Copy(imagePath, copyPath);
      }
    }

    string exportDataPath = Path.Combine(path, fileName + ".eat");
    SaveFile(exportDataPath, list);
    
  }

  public static List<Recipe> ImportData(string fileName)
  {
    string path = Path.Combine(Application.persistentDataPath, "ExportData");
    string importDataPath = Path.Combine(path, fileName + ".eat");
    List<Recipe> importData = (List<Recipe>)LoadFile(importDataPath);

    int nameCounter = 0;
    foreach (var recipe in importData)
    {
      //change name
      recipe.name = recipe.name + " (neu)";
      if (ListData.instance.recipes.Find(x => x.name == recipe.name) != null) recipe.name =recipe.name + " -d"; 

      //copy image
      if (recipe.picture != "")
      {
        string imagePath = Path.Combine(path, recipe.picture);
        string newImageName = DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + nameCounter.ToString() + ".png";
        string newImagePath = Path.Combine(DataPath, newImageName);
        File.Copy(imagePath, newImagePath);
        recipe.picture = newImageName;
      }
      nameCounter++;
    }
    
    Directory.Delete(path, true);
    return importData;
  }

  public static Sprite LoadImageFromFile(string fileName)
  {
    byte[] bytes;
    Texture2D tex;

    string path = Path.Combine(DataPath, fileName);
    if (File.Exists(path))
    {
      bytes = File.ReadAllBytes(path);

      tex = new Texture2D(2, 2);
      tex.LoadImage(bytes);

      return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 100f);
    }
    return null;
  }

  public static string GetPathFromSaveFile(string saveFile)
  {
    return Path.Combine(DataPath, saveFile + ".eat");
  }

  private static void SaveFile(string path, object data)
  {
    Debug.Log("Saving to " + path);

    using (FileStream stream = File.Open(path, FileMode.Create))
    {
      BinaryFormatter formatter = new BinaryFormatter();
      formatter.Serialize(stream, data);
    }
  }

  private static object LoadFile(string path)
  {
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

