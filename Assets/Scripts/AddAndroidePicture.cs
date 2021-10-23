using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AddAndroidePicture : MonoBehaviour
{
  [SerializeField] GameObject picturePanel;
  [SerializeField] RawImage liveFeed;
  WebCamTexture camTexture;

  bool isLive = false;

  public Action<string> onTakePicture;

  private void Start()
  {
    camTexture = new WebCamTexture();
    liveFeed.texture = camTexture;
  }

  private void Update()
  {
    if (isLive) liveFeed.texture = camTexture;
  }

  public void OpenPicturePanel()
  {
    TogglePictureMode(true);
    camTexture.Play();
  }

  public void ClosePicturePanel()
  {
    TogglePictureMode(false);
    camTexture.Stop();
  }

  public void TogglePictureMode(bool on)
  {
    isLive = on;
    picturePanel.SetActive(on);
  }

  public void TakePicture()
  {
    StartCoroutine(TakePhoto());
  }

  public IEnumerator TakePhoto()  // Start this Coroutine on some button click
  {
    yield return new WaitForEndOfFrame();

    Texture2D photo = new Texture2D(camTexture.width, camTexture.height);
    photo.SetPixels(camTexture.GetPixels());
    photo.Apply();

    //Encode to a PNG
    byte[] bytes = photo.EncodeToPNG();
    //Write out the PNG. Of course you have to substitute your_path for something sensible
    string photoName = DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + ".png";
    string path = Path.Combine(Application.persistentDataPath, photoName);
    File.WriteAllBytes(path, bytes);

    Texture2D tex = new Texture2D(2, 2);
    tex.LoadImage(bytes);

    Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 100f);
    GetComponent<Image>().sprite = newSprite;

    onTakePicture.Invoke(photoName);

    camTexture.Stop();
    TogglePictureMode(false);
  }
}
