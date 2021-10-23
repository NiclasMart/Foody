using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SavingSystem
{
  public static void Save(object data, string saveFile)
  {
    SaveFile(saveFile, data);
  }

  public static object Load(string saveFile)
  {
    return LoadFile(saveFile);
  }

  public static void DeletePicture(string name)
  {
    if (name == "") return;
    string path = Path.Combine(Application.persistentDataPath, name);
    File.Delete(path);
  }

  public static Sprite LoadImageFromFile(string fileName)
  {
    byte[] bytes;
    Texture2D tex;

    string path = Path.Combine(Application.persistentDataPath, fileName);
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

