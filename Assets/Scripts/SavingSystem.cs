using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

  public class SavingSystem : MonoBehaviour
  {
    public void Save(string saveFile)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      SaveFile(saveFile, data);
    }

    public void Load(string saveFile)
    {
      Dictionary<string, object> data = LoadFile(saveFile);
    }

    private string GetPathFromSaveFile(string saveFile)
    {
      return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }

    private void SaveFile(string saveFile, object data)
    {
      string path = GetPathFromSaveFile(saveFile);
      Debug.Log("Saving to " + path);

      using (FileStream stream = File.Open(path, FileMode.Create))
      {
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, data);
      }
    }

    private Dictionary<string, object> LoadFile(string saveFile)
    {
      string path = GetPathFromSaveFile(saveFile);
      Debug.Log("Loading from " + path);
      if (!File.Exists(path))
      {
        return new Dictionary<string, object>();
      }
      using (FileStream stream = File.Open(path, FileMode.Open))
      {
        BinaryFormatter formatter = new BinaryFormatter();
        return (Dictionary<string, object>)formatter.Deserialize(stream);
      }
    }
  }

