using UnityEngine;

public class ClickAnywhereHandler : MonoBehaviour
{
    [SerializeField] private GameObject playButton, quitButton;
    [SerializeField] private GameObject pressAnywhereObjects;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           playButton.SetActive(true);
           quitButton.SetActive(true);
           pressAnywhereObjects.SetActive(false);
           
           Destroy(gameObject);

        }
    }
}
