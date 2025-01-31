using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LinkHandler : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI textMeshProUGUI;

    void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check for link index at the pointer location
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshProUGUI, eventData.position, null);
        
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = textMeshProUGUI.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkID();
            HandleLinkClick(linkID);
        }
    }

    private void HandleLinkClick(string linkID)
    {
        Debug.Log("Link clicked: " + linkID);
        RecipeDisplay display = FindObjectOfType<RecipeDisplay>();
        if (display != null)
        {
            display.OpenLinkedRecipe(linkID);
        }

    }
}
