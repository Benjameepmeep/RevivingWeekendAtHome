using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


    public class ItemController : MonoBehaviour
    {
        public ItemType[] itemScrub;
        public TMP_Text itemName;
        public TMP_Text itemText;
        public Image itemImage;
        public AudioSource audioPlayer;

        public static bool playerIsInsideItemTrigger;
        private bool _alreadyUsingCoroutine;

        //Sets Scrub To Scene
        private void Start()
        {
            itemName.text = itemScrub[ItemObjectScript.currentObjectInt].itemName;
            itemText.text = itemScrub[ItemObjectScript.currentObjectInt].itemText;
            itemImage.sprite = itemScrub[ItemObjectScript.currentObjectInt].itemImage;
            itemImage.transform.localScale = itemScrub[ItemObjectScript.currentObjectInt].itemSize;
            audioPlayer.PlayOneShot(itemScrub[ItemObjectScript.currentObjectInt].itemAudio);
            StartCoroutine(SceneLoadAndSetActive());
        }

        private IEnumerator SceneLoadAndSetActive()
        {
            var sceneByName = SceneManager.GetSceneByName("Item");
            SceneManager.SetActiveScene(sceneByName);
            yield return new WaitUntil(() => SceneManager.GetActiveScene() == sceneByName);
            StartCoroutine(PlayerInsideItemScene(sceneByName.name));
            ItemObjectScript.currentlyOpeningItem = false;
            ItemObjectScript.inItemScene = true;
        }

        private IEnumerator PlayerInsideItemScene(string nameOfScene)
        {
            Debug.Log("Inside the " + nameOfScene + " Scene, with the item: " + itemScrub[ItemObjectScript.currentObjectInt]);
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName(nameOfScene))
            {
                Debug.LogError(nameOfScene + " isn't the active Scene!");
            }
            yield return new WaitUntil(() => UserInput.Interact || UserInput.Escape || !playerIsInsideItemTrigger);
            
            yield return null; // This line is vital to stop for 1 frame. It avoids reopening a scene immediately if UserInput.Interact.
            
            Debug.Log("Currently exiting " + nameOfScene + ".");
            ItemObjectScript.inItemScene = false;
            SceneManager.UnloadSceneAsync(nameOfScene);
        }
    }

