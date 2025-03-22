using UnityEngine;
using UnityEngine.EventSystems;

public class FocusUIButton : MonoBehaviour
{
    void Update()
    {
        if (UserInput.AnyKeyOrStart || UserInput.Movement != Vector2.zero)
        {
            HandleAnyButtonPress();
        }
    }

    private void HandleAnyButtonPress()
    {
        // If MainUIBtn is enabled and no UI element is currently selected, select this GameObject.
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
