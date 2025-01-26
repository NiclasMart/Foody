using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplay : MonoBehaviour
{
    [SerializeField] RectTransform showCard, editCard;
    [SerializeField] ShoppingAdder shoppingAdder;
    [SerializeField] TextMeshProUGUI nameField, linkField, dateField, date2Field, tagHeader, tagsField, ingredienceField, descriptionHeader, descriptionField, noteHeader, noteField;
    [SerializeField] Image pictureField, pictureIn, foodListButton, markerButton;
    [SerializeField] TagSearchbar tagSearchbar;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] InputFunctionality nameIn, linkIn, tagsIn, ingredienceIn, descriptionIn, noteIn;
    ListHandler listHandler;
    public Recipe displayedRecipe;
    List<string> linkpath = new List<string>();

    private void Awake()
    {
        listHandler = FindObjectOfType<ListHandler>();
        ToggleShoppingAdder(null);
    }

    public void Display(Recipe recipe)
    {
        nameField.text = recipe.name;

        linkField.gameObject.SetActive(recipe.link != "");
        linkField.text = BuildHyperlink(recipe.link);

        string tags = recipe.GetTagsAsString();
        tagHeader.gameObject.SetActive(tags != "");
        tagsField.text = tags;

        ingredienceField.text = recipe.ingredients;

        descriptionHeader.gameObject.SetActive(recipe.description != "");
        descriptionField.text = recipe.description;

        noteHeader.gameObject.SetActive(recipe.note != "");
        noteField.text = recipe.note;

        dateField.text = "Zuletzt gekocht am: " + recipe.date;
        date2Field.text = "Hinzugefügt am: " + recipe.creationDate;

        pictureField.sprite = SavingSystem.LoadImageFromFile(recipe.picture);
        pictureField.transform.parent.gameObject.SetActive(pictureField.sprite != null);

        SetButtonColor(foodListButton, ListData.instance.foods.Find(x => x == recipe.ID) != null);
        SetButtonColor(markerButton, recipe.marked);
        displayedRecipe = recipe;
    }

    public void Delete()
    {
        SavingSystem.DeletePicture(displayedRecipe.picture);
        ListData.instance.DeleteRecipe(displayedRecipe);
        ListData.instance.SaveRecipeList();
        listHandler.ShowCompleteList();
        ToggleRecipeDisplay(false);
    }

    public void UpdateCookingDate()
    {
        displayedRecipe.date = DateTime.Now.Date.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"));
        Display(displayedRecipe);
        ListData.instance.SaveFoodList();

        Recipe recipe = ListData.instance.recipes.Find(x => x.name == displayedRecipe.name);
        if (recipe != null) recipe.date = displayedRecipe.date;
        ListData.instance.SaveRecipeList();
    }

    public void EnableEditingMode()
    {
        GetComponent<Animator>().SetBool("EditActive", true);
        GetComponent<ScrollRect>().content = editCard;
        shoppingAdder.gameObject.SetActive(false);
        ToggleEditingMode(true);
        StartCoroutine(FillContentInInputFields());
    }

    public void ResetEditingMode()
    {
        ToggleEditingMode(false);
        Display(displayedRecipe);
        GetComponent<Animator>().SetBool("EditActive", false);
        GetComponent<ScrollRect>().content = showCard;
    }

    public void SaveEditingChanges()
    {
        displayedRecipe.name = nameIn.GetValue();
        displayedRecipe.link = linkIn.GetValue();
        displayedRecipe.tags = RecipeAdder.StringToList(tagsIn.GetValue());
        displayedRecipe.description = RecipeAdder.RichTextAdder(descriptionIn.GetValue());
        displayedRecipe.ingredients = RecipeAdder.RichTextAdder(ingredienceIn.GetValue());
        displayedRecipe.SetRecipeType(dropdown.captionText.text);
        displayedRecipe.note = RecipeAdder.RichTextAdder(noteIn.GetValue());

        RecipeAdder recipeAdder = FindObjectOfType<RecipeAdder>();
        if (recipeAdder) recipeAdder.SavePicture(displayedRecipe);

        listHandler.ShowCompleteList();
        ListData.instance.SaveRecipeList();

        if (tagSearchbar) tagSearchbar.RefreshTagList();

        ResetEditingMode();
    }

    public void ToggleRecipeDisplay(bool on)
    {
        listHandler.gameObject.SetActive(!on);
        transform.GetChild(0).gameObject.SetActive(on);
        if (!on) displayedRecipe = null;
    }

    public void ToggleEditingMode(bool on)
    {
        editCard.gameObject.SetActive(on);
        showCard.gameObject.SetActive(!on);
    }

    public void AddRecipeToFoodList(Image btn)
    {
        if (ListData.instance.foods.Find(x => x == displayedRecipe.ID) != null)
        {
            ListData.instance.foods.RemoveAll(x => x == displayedRecipe.ID);
            SetButtonColor(btn, false);
        }
        else
        {
            ListData.instance.foods.Add(displayedRecipe.ID);
            SetButtonColor(btn, true);
        }
        ListData.instance.SaveFoodList();
    }

    public void MarkRecipe(Image btn)
    {
        displayedRecipe.marked = !displayedRecipe.marked;
        SetButtonColor(btn, displayedRecipe.marked);
        ListData.instance.SaveRecipeList();
    }

    //jumps to the last displayed recipe from linkpath or back to overview
    public void JumpBackInLinkpath()
    {
        if (linkpath.Count == 0)
        {
            ToggleRecipeDisplay(false);
            listHandler.ShowCompleteList();
        }
    }

    public void CopyIDToClipboard()
    {
        if (displayedRecipe.ID != null) GUIUtility.systemCopyBuffer = displayedRecipe.ID;
    }

    private void SetButtonColor(Image btn, bool active)
    {
        if (!btn) return;
        if (active) btn.color = new Color32(43, 137, 35, 255);
        else btn.color = Color.white;
    }

    public void ToggleShoppingAdder(Image btn)
    {
        if (!shoppingAdder) return;
        bool isActive = shoppingAdder.gameObject.activeSelf;
        shoppingAdder.gameObject.SetActive(!isActive);
        if (btn) btn.color = !isActive ? new Color32(43, 137, 35, 255) : new Color32(255, 255, 255, 255);
        if (!isActive) shoppingAdder.gameObject.GetComponentInChildren<TMP_InputField>().Select();

    }

    private IEnumerator FillContentInInputFields()
    {
        yield return new WaitForEndOfFrame();

        nameIn.SetValue(displayedRecipe.name);
        linkIn.SetValue(displayedRecipe.link);
        tagsIn.SetValue(displayedRecipe.GetTagsAsString());
        ingredienceIn.SetValue(displayedRecipe.ingredients);
        descriptionIn.SetValue(displayedRecipe.description);
        noteIn.SetValue(displayedRecipe.note);
        pictureIn.sprite = SavingSystem.LoadImageFromFile(displayedRecipe.picture);
        dropdown.value = (int)displayedRecipe.type;
    }

    private string BuildHyperlink(string link)
    {
        return "<link=" + link + "><color=blue>Link zum Rezept</color></link>";
    }
}
