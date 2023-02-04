using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Firebase;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseInit : MonoBehaviour
{

    public UnityEvent OnFirebaseInitialized = new UnityEvent();
    async void Start()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            Debug.Log("initialized Firebase");
            
            OnFirebaseInitialized.Invoke();
            SavingSystem.firebase = this;
            Debug.Log("Test1");
            UploadAllData();
        }
    }

    public void UploadAllData()
    {
        // foreach(Recipe recipe in ListData.instance.recipes)
        // {
        //     StartCoroutine(UploadImageCoroutine(recipe));
        //     recipe.lastUpdated = DateTime.Now.Date.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"));
        // }

        // StartCoroutine(UploadImageCoroutine(ListData.instance.recipes[2]));
        // var recipeListDataPath = Path.Combine(SavingSystem.DataPath, "RecipeList.eat");
        // Debug.Log(recipeListDataPath);
        // StartCoroutine(UploadFileCoroutine(recipeListDataPath, "RecipeList.eat"));

    }

    public IEnumerator SynchroniseList(string fileName, string local_metaDataLastUpdated)
    {
        Debug.Log("Synchronize file");
        string localFilePath = Path.Combine(SavingSystem.DataPath, fileName);

        // Get a reference to the binary file in Firebase Storage
        var storage = FirebaseStorage.DefaultInstance;
        var fileRef = storage.GetReference($"/{fileName}");

        // Retrieve the metadata for the file
        var metadataTask = fileRef.GetMetadataAsync();
        yield return new WaitUntil(() => metadataTask.IsCompleted);

        

        //if file does not exist in the cloud, upload it
        if (metadataTask.Exception != null)
        {
            //if (metadataTask.Exception.Message == "Object does not exist at location.")
                Debug.Log($"{fileName} or its meta data not exists in the cloud");
                yield return UploadFileCoroutine(localFilePath, fileName, local_metaDataLastUpdated);
                yield break;
        }
        

        //get and compare meta data
        string cloud_metaDataLastUpdated = metadataTask.Result.GetCustomMetadata("Date");

        long localUnixSeconds = long.Parse(local_metaDataLastUpdated);
        long cloudUnixSeconds = long.Parse(cloud_metaDataLastUpdated);
        //DateTimeOffset localDate = DateTimeOffset.Parse(local_metaDataLastUpdated);
        //DateTimeOffset cloudDate = DateTimeOffset.Parse(cloud_metaDataLastUpdated);

        Debug.Log($"Local date {local_metaDataLastUpdated}; Cloud date {cloud_metaDataLastUpdated}");
        if (localUnixSeconds == cloudUnixSeconds)
        {
            yield break;
        }
        else if (localUnixSeconds < cloudUnixSeconds)
        {
            //download content
            yield return DownloadFileCoroutine(localFilePath, fileName);
            ListData.ListFileData data = (ListData.ListFileData)SavingSystem.Load("RecipeList");
            if (data == null) yield break;

            ListData.instance.recipes = (List<Recipe>)data.listData;
        }
        else if (localUnixSeconds > cloudUnixSeconds)
        {
            //upload content
            yield return UploadFileCoroutine(localFilePath, fileName, local_metaDataLastUpdated);
        }
    }

    private IEnumerator UploadImageCoroutine(Recipe recipe)
    {
        Debug.Log("Test3");
        if (recipe.picture == "")
        {
            Debug.Log("No picture");
            yield break;
        }
        //get storage ref for picture
        var storage = FirebaseStorage.DefaultInstance;
        var pictureRef = storage.GetReference($"/pictures/{recipe.picture}");

        Debug.Log("Test 3.5");

        //upload foto and save path to recipe
        var bytes = recipe.GetImage().texture.EncodeToPNG();
        Debug.Log("Test 3.6");
        var uploadTask = pictureRef.PutBytesAsync(bytes);
        yield return new WaitUntil(() => uploadTask.IsCompleted);

        Debug.Log("Test4");

        if (uploadTask.Exception != null)
        {
            Debug.LogError($"Failed to upload because {uploadTask.Exception}");
            yield break;
        }
        yield return null;
    }

    private IEnumerator DownloadImageCoroutine(Recipe recipe)
    {
        // TODO: test if image has picture

        var storage = FirebaseStorage.DefaultInstance;
        var pictureRef = storage.GetReference($"/pictures/{recipe.picture}");

        // Download the picture
        var downloadTask = pictureRef.GetBytesAsync(4096 * 4096);
        yield return new WaitUntil(() => downloadTask.IsCompleted);

        if (downloadTask.Exception != null)
        {
            Debug.LogError($"Failed to download picture because {downloadTask.Exception}");
            yield break;
        }

        // Create a Texture2D from the downloaded bytes
        // var texture = new Texture2D(1, 1);
        // texture.LoadImage(downloadTask.Result);


    }

    private IEnumerator UploadFileCoroutine(string filePath, string name, string lastUpdatedMetadata)
    {
        Debug.Log("Upload File");
        // Get a reference to the binary file in Firebase Storage
        var storage = FirebaseStorage.DefaultInstance;
        var binaryRef = storage.GetReference($"/{name}");

        //create custom metadata
        var metadateChange = new MetadataChange()
        {
            CustomMetadata = new Dictionary<string, string>()
            {
                {"Date",  lastUpdatedMetadata}
            }
        };

        // Read the binary file from disk
        byte[] binaryData = File.ReadAllBytes(filePath);

        // Upload the binary file to Firebase Storage
        var uploadTask = binaryRef.PutBytesAsync(binaryData, metadateChange);
        yield return new WaitUntil(() => uploadTask.IsCompleted);

        if (uploadTask.Exception != null)
        {
            Debug.LogError($"Failed to upload binary because {uploadTask.Exception}");
            yield break;
        }
    }

    public IEnumerator DownloadFileCoroutine(string filePath, string name)
    {
        // Get a reference to the binary file in Firebase Storage
        var storage = FirebaseStorage.DefaultInstance;
        var fileRef = storage.GetReference($"/{name}");

        // Download the picture
        var downloadTask = fileRef.GetBytesAsync(4096 * 4096);
        yield return new WaitUntil(() => downloadTask.IsCompleted);

        if (downloadTask.Exception != null)
        {
            Debug.LogError($"Failed to download picture because {downloadTask.Exception}");
            yield break;
        }

        //create file from data
        byte[] binaryData = downloadTask.Result;
        File.WriteAllBytes(filePath, binaryData);
    }

}
