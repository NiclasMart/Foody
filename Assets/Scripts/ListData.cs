using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ListData : MonoBehaviour
{
    public List<Recipe> recipes = new List<Recipe>();
    public List<string> foods = new List<string>();
    public Dictionary<string, int> purchases = new Dictionary<string, int>();
    public static ListData instance = null;

    [System.Serializable]
    public class ListFileData
    {
        public string date;
        public object listData;

        public ListFileData(object list)
        {
            date = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            listData = list;
        }
    }

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

    public void AddRecipe(Recipe newRecipe)
    {
        recipes.Add(newRecipe);
    }

    public void DeleteRecipe(Recipe recipe)
    {
        if (recipe == null) return;
        recipes.Remove(recipe);
    }

    public void SetRecipes(List<Recipe> list)
    {
        recipes = list;
    }

    public Recipe GetRecipe(string name)
    {
        return recipes.Find(x => x.name == name);        
    }

    public void InitializeAllLists()
    {
        Debug.Log("List Initializer");
        FirebaseInit firebase = GetComponent<FirebaseInit>();
        if (!firebase)
        {
            Debug.Log("No firebase handler found!");
            return;
        }

        firebase.OnFirebaseInitialized.AddListener(SynchronizeLists);
    }

    private void SynchronizeLists()
    {
        Debug.Log("Call Synchronize");
        FirebaseInit firebase = GetComponent<FirebaseInit>();
        StartCoroutine(LoadRecipeList(firebase));
        StartCoroutine(LoadFoodList(firebase));
        StartCoroutine(LoadShoppingList(firebase));
    }

    public void SaveRecipeList()
    {
        ListFileData data = new ListFileData(recipes);
        SavingSystem.Save(data, "RecipeList");

        //TODO: upload new file
    }

    public IEnumerator LoadRecipeList(FirebaseInit firebase)
    {
        //get recipe list of file storage
        ListFileData data = (ListFileData)SavingSystem.Load("RecipeList");

        //if no local file is availabe, try to download file
        if (data == null)
        {
            string path = Path.Combine(SavingSystem.DataPath, "RecipeList.eat");
            yield return firebase.DownloadFileCoroutine(path, "RecipeList.eat");

            //get data from downloaded file and if empty, return
            data = (ListFileData)SavingSystem.Load("RecipeList");
            if (data == null)
            {
                recipes = new List<Recipe>();
                yield break;
            }
        }
        recipes = (List<Recipe>)data.listData;
        string localFileLastUpdated = data.date;

        yield return firebase.SynchroniseList("RecipeList.eat", localFileLastUpdated);

    }

    public void SaveFoodList()
    {
        ListFileData data = new ListFileData(foods);
        SavingSystem.Save(data, "FoodList");
        Debug.Log("Saved new food list");
        // SavingSystem.Save(foods, "FoodList");

        //TODO: upload new food list
    }

    public IEnumerator LoadFoodList(FirebaseInit firebase)
    {
        //get recipe list of file storage
        ListFileData data = (ListFileData)SavingSystem.Load("FoodList");

        if (data == null)
        {
            string path = Path.Combine(SavingSystem.DataPath, "FoodList.eat");
            yield return firebase.DownloadFileCoroutine(path, "FoodList.eat");

            //get data from downloaded file and if empty, return
            data = (ListFileData)SavingSystem.Load("FoodList");
            if (data == null)
            {
                foods = new List<string>();
                yield break;
            }
        }
        foods = (List<string>)data.listData;
        string localFileLastUpdated = data.date;

        yield return firebase.SynchroniseList("FoodList.eat", localFileLastUpdated);
    }

    public void SaveShoppingList()
    {
        ListFileData data = new ListFileData(purchases);
        SavingSystem.Save(data, "ShoppingList");
        Debug.Log("Saved new Shopping list");
        // SavingSystem.Save(purchases, "ShoppingList");

        //TODO: implement firebase
    }

    public IEnumerator LoadShoppingList(FirebaseInit firebase)
    {
        //get recipe list of file storage
        ListFileData data = (ListFileData)SavingSystem.Load("ShoppingList");

        if (data == null)
        {
            string path = Path.Combine(SavingSystem.DataPath, "ShoppingList.eat");
            yield return firebase.DownloadFileCoroutine(path, "ShoppingList.eat");

            //get data from downloaded file and if empty, return
            data = (ListFileData)SavingSystem.Load("ShoppingList");
            if (data == null)
            {
                purchases = new Dictionary<string, int>();
                yield break;
            }
        }
        purchases = (Dictionary<string, int>)data.listData;
        string localFileLastUpdated = data.date;

        yield return firebase.SynchroniseList("ShoppingList.eat", localFileLastUpdated);
    }

    public void AddNewItemToShoppingCard(string name, int amount)
    {
        if (purchases.ContainsKey(name))
            ListData.instance.purchases[name] += amount;
        else
            purchases.Add(name, amount);
    }


}
