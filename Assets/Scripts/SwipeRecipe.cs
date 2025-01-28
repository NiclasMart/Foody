using UnityEngine;

public class SwipeRecipe : MonoBehaviour
{
    [SerializeField] float minSwipeDistance = 50f; // Minimum distance for a valid swipe
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 swipeDelta;

    [SerializeField] private RecipeListHandler listHandler;

    void Update()
    {
        // Ensure there are touches to process
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    swipeDelta = endTouchPosition - startTouchPosition;

                    HandleSwipe(swipeDelta);
                    break;
            }
        }
#if UNITY_EDITOR
        else if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            swipeDelta = endTouchPosition - startTouchPosition;
            HandleSwipe(swipeDelta);
        }
#endif
    }

    private void HandleSwipe(Vector2 swipeDelta)
    {
        // Check if the swipe distance meets the minimum threshold
        if (swipeDelta.magnitude >= minSwipeDistance)
        {
            // Determine swipe direction
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                Recipe nextRecipe;
                if (x > 0)
                {
                    nextRecipe = listHandler.GetNextRecipeInList(-1);
                }
                else
                {
                    nextRecipe = listHandler.GetNextRecipeInList(1);
                }

                if (nextRecipe != null)
                {
                    listHandler.DisplayRecipe(nextRecipe);
                }
            }
        }
    }
}
