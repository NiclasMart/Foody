using UnityEngine;
using TMPro;

public class MenuHandler : MonoBehaviour
{
    Animator animator;
    [SerializeField] TextMeshProUGUI pathDisplay;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ListData.instance.LoadRecipeList();
        ListData.instance.LoadFoodList();
        ListData.instance.LoadShoppingList();

        pathDisplay.text = Application.persistentDataPath;

        //application settings to reduce energy consumption
        Application.targetFrameRate = 30;
        Time.fixedDeltaTime = 1.0f / 2.0f;
        Application.runInBackground = false;
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void ToggleAddCard(bool on)
    {
        animator.SetBool("addCardActive", on);
    }

    public void ShowExplorer()
    {
        string itemPath = Application.persistentDataPath.Replace(@"/", @"\");
        System.Diagnostics.Process.Start("explorer.exe", "/root," + itemPath);
    }
}
