using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


    public class UsedInteractableController : MonoBehaviour
    {
        public ItemType[] itemScrub;
        public TMP_Text itemName;
        public TMP_Text itemText;
        public Image itemImage;
        public AudioSource audioPlayer;

        //Sets Scrub To Scene
        private void Start()
        {
            itemName.text = itemScrub[ItemObjectScript.currentUsedInteractableInt].itemName;
            itemText.text = itemScrub[ItemObjectScript.currentUsedInteractableInt].itemText;
            itemImage.sprite = itemScrub[ItemObjectScript.currentUsedInteractableInt].itemImage;
            itemImage.transform.localScale = itemScrub[ItemObjectScript.currentUsedInteractableInt].itemSize;
            audioPlayer.PlayOneShot(itemScrub[ItemObjectScript.currentUsedInteractableInt].itemAudio);
            StartCoroutine(SceneLoadAndSetActive());
        }

        private IEnumerator SceneLoadAndSetActive()
        {
            var sceneByName = SceneManager.GetSceneByName("UsedInteractable");
            SceneManager.SetActiveScene(sceneByName);
            yield return new WaitUntil(() => SceneManager.GetActiveScene() == sceneByName);
            StartCoroutine(PlayerInsideItemScene(sceneByName.name));
            ItemObjectScript.currentlyOpeningItem = false;
            ItemObjectScript.inItemScene = true;
        }
        
        private IEnumerator PlayerInsideItemScene(string nameOfScene)
        {
            Debug.Log("Inside the " + nameOfScene + " Scene, with the item: " + itemScrub[ItemObjectScript.currentUsedInteractableInt]);
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName(nameOfScene))
            {
                Debug.LogError(nameOfScene + " isn't the active Scene!");
            }
            yield return new WaitUntil(() => UserInput.Interact || UserInput.Escape || !ItemController.playerIsInsideItemTrigger);
            
            yield return null; // This line is vital to stop for 1 frame. It avoids reopening a scene immediately if UserInput.Interact.
            
            Debug.Log("Currently exiting " + nameOfScene + ".");
            ItemObjectScript.inItemScene = false;
            SceneManager.UnloadSceneAsync(nameOfScene);
        }
    }

