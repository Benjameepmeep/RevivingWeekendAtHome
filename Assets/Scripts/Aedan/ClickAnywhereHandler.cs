using UnityEngine;

public class ClickAnywhereHandler : MonoBehaviour
{
    public static ClickAnywhereHandler Instance => _instance ??= FindFirstObjectByType<ClickAnywhereHandler>();
    private static ClickAnywhereHandler _instance;

    [SerializeField] private GameObject playButton, quitButton;
    [SerializeField] private GameObject pressAnywhereObjects;
    
    // Flag to track if the buttons have already been shown
    private bool _buttonsShown = false;

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //        ShowPlayButtons();
    //     }
    // }

    private void OnDestroy()
    {
        // Clear the static instance when destroyed
        if (_instance == this)
            _instance = null;
    }

    public void ShowPlayButtons()
    {
        // Prevent multiple calls or calls after destruction
        if (_buttonsShown) 
            return;
            
        // Check if the references are still valid before using them
        if (playButton != null)
            playButton.SetActive(true);
            
        if (quitButton != null)
            quitButton.SetActive(true);
            
        if (pressAnywhereObjects != null)
            pressAnywhereObjects.SetActive(false);
        
        _buttonsShown = true;
        Destroy(gameObject);
    }
}