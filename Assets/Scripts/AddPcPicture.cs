using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


public class AddPcPicture : PictureAdder
{
  public void RetrieveImageFromClipboard()
  {
    string photoName = DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + ".png";
    string storagePath = Path.Combine(SavingSystem.DataPath, photoName);

    string pathToScript = Path.Combine(UnityEngine.Application.persistentDataPath, "ImageDumper.ps1");
    string command = "powershell -ExecutionPolicy Bypass -File " + pathToScript + " " + storagePath;

    System.Diagnostics.Process process = System.Diagnostics.Process.Start("powershell.exe", command);
    process.WaitForExit();
    process.Close();

    Sprite sprite = SavingSystem.LoadImageFromFile(storagePath);
    GetComponent<Image>().sprite = sprite;

    onTakePicture.Invoke(photoName);
  }
}
