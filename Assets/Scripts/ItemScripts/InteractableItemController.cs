using System.Collections;
using PlayerScripts;
using SceneScripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;

namespace ItemScripts
{
    public class InteractableItemController : MonoBehaviour
    {
        // TODO: Fix the interactable items.

        private EventSystem _eventSystemInteractable;
        
        public ItemType[] itemScrub;
        public TMP_Text itemName;
        public TMP_Text itemText;
        public Image itemImage;
        public AudioSource audioPlayer;
        public PlayableAsset itemTimeline;
        public PlayableDirector playableDirector;
        
        // TODO: Check if the new Timeline code works : )

        public static bool clickedYes;
        public static bool clickedNo;
        private string _sceneToLoad;

        private void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();
            playableDirector.paused += DirectorPaused;
            playableDirector.played += DirectorPlaying;
            playableDirector.stopped += DirectorStopped;
        }
        
        private void Start()
        {
            _eventSystemInteractable = GameObject.FindGameObjectWithTag("EventSystemInteractable").GetComponent<EventSystem>();
            audioPlayer = GetComponent<AudioSource>();
            audioPlayer.clip = itemScrub[ItemObjectScript.currentObjectInt].itemAudio;

            itemName.text = itemScrub[ItemObjectScript.currentObjectInt].itemName;
            itemText.text = itemScrub[ItemObjectScript.currentObjectInt].itemText;
            itemImage.sprite = itemScrub[ItemObjectScript.currentObjectInt].itemImage;
            itemImage.transform.localScale = itemScrub[ItemObjectScript.currentObjectInt].itemSize;
            
            itemTimeline = itemScrub[ItemObjectScript.currentObjectInt].timeline;
            itemTimeline = playableDirector.playableAsset;
            StartCoroutine(SceneLoadAndSetActive());
        }
        
        private IEnumerator SceneLoadAndSetActive()
        {
            var sceneByName = SceneManager.GetSceneByName("Interactable");
            SceneManager.SetActiveScene(sceneByName);
            yield return new WaitUntil(() => SceneManager.GetActiveScene() == sceneByName);
            _eventSystemInteractable.enabled = true;
            StartCoroutine(PlayerInsideItemScene(sceneByName.name));
            ItemObjectScript.currentlyOpeningItem = false;
            ItemObjectScript.inItemScene = true;
        }
        
        private IEnumerator PlayerInsideItemScene(string nameOfScene)
        {
            print("Inside the " + nameOfScene + " Scene, with the item: " + itemScrub[ItemObjectScript.currentObjectInt]);
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName(nameOfScene))
            {
                Debug.LogError(nameOfScene + " isn't the active Scene!");
            }
            
            yield return new WaitUntil(() => clickedYes || clickedNo || UserInput.Escape);
            if (clickedYes)
            {
                // yield return new WaitUntil(() => !clickedYes);
                yield return null;
                StartOnClickYes();
            }
            else if (clickedNo)
            {
                // yield return new WaitUntil(() => !clickedNo);
                yield return null;
                StartOnClickNo();
            }
            else
            {
                yield return null;
                ExitScene(nameOfScene);
            }
        }

        private void ExitScene(string nameOfScene)
        {
            print("Currently exiting " + nameOfScene + ".");
            ItemObjectScript.inItemScene = false;
            _eventSystemInteractable.enabled = false;
            ItemObjectScript.enableEventSystemMain = true;
            SceneManager.UnloadSceneAsync(nameOfScene);
        }

        public void StartOnClickYes()
        {
            StartCoroutine(OnClickYes());
        }
        
        private IEnumerator OnClickYes()
        {
            clickedYes = true;
            if (audioPlayer.clip != null) 
            {
                audioPlayer.clip = itemScrub[ItemObjectScript.currentObjectInt].cutSceneAudio;
                audioPlayer.Play();
                audioPlayer.loop = true;
            }
            if (itemTimeline != null)
            {
                playableDirector.Play(itemTimeline);
                yield return new WaitForSeconds((float)itemTimeline.duration + 0.1f);
                clickedYes = false;
            }
            else
            {
                yield return null;
                clickedYes = false;
            }
            ExitScene(SceneManager.GetActiveScene().name);
        }

        public void StartOnClickNo()
        {
            StartCoroutine(OnClickNo());
        }
        
        private IEnumerator OnClickNo()
        {
            clickedNo = true;
            yield return null; // "yield return null" waits for 1 frame before continuing down.
            clickedNo = false;
            ExitScene(SceneManager.GetActiveScene().name);
        }

        private void DirectorPlaying(PlayableDirector obj)
        {
            // TODO: Add code here for disabling PlayerInput (OnDisable?) and other effects to let timeline play as it should.
            // Maybe a reference to the GameController for a method in there would be good.
        }
        
        private void DirectorPaused(PlayableDirector obj)
        {
            // TODO: Add code here for pauseMenu stuff to be enabled.
            // Maybe a reference to the GameController for a method in there would be good.
        }

        private void DirectorStopped(PlayableDirector obj)
        {
            // TODO: Add code here for pauseMenu stuff to be enabled.
            // Maybe a reference to the GameController for a method in there would be good.
        }
    }
}
