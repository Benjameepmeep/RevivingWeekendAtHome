using UnityEngine;

public class ClickAnywhereHandler : MonoBehaviour
{
    public static ClickAnywhereHandler Instance => _instance ??= FindFirstObjectByType<ClickAnywhereHandler>();
    private static ClickAnywhereHandler _instance;

    [SerializeField] private GameObject playButton, quitButton;
    [SerializeField] private GameObject pressAnywhereObjects;

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //        ShowPlayButtons();
    //     }
    // }



    public void ShowPlayButtons()
    {
        playButton.SetActive(true);
        quitButton.SetActive(true);
        pressAnywhereObjects.SetActive(false);
    
        Destroy(gameObject);
    }
}