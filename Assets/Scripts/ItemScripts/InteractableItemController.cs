using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;


public class InteractableItemController : MonoBehaviour
{
    // TODO: Fix the interactable items.

    [SerializeField] private EventSystem _eventSystemInteractable;
    
    public ItemType[] itemScrub;
    public TMP_Text itemName;
    public TMP_Text itemText;
    public Image itemImage;
    public AudioSource audioPlayer;
    public PlayableAsset itemTimeline;
    public PlayableDirector playableDirector;
    
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
        if (_eventSystemInteractable == null) _eventSystemInteractable = GameObject.FindGameObjectWithTag("EventSystemInteractable").GetComponent<EventSystem>();
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

    void Update()
    {
        // Check if the current triggered object has walked away state set
        if (ItemObjectScript.currentTriggeredObject != null)
        {
            ItemObjectScript currentObject = ItemObjectScript.currentTriggeredObject.GetComponent<ItemObjectScript>();
            if (currentObject != null)
            {
                // Logic for handling walked away state is now managed in the ItemObjectScript
                // This Update method can be simplified
            }
        }
    }
    

    private IEnumerator SceneLoadAndSetActive()
    {
        if (_eventSystemInteractable == null) _eventSystemInteractable = GameObject.FindGameObjectWithTag("EventSystemInteractable").GetComponent<EventSystem>();

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
        Debug.Log("Inside the " + nameOfScene + " Scene, with the item: " + itemScrub[ItemObjectScript.currentObjectInt]);
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName(nameOfScene))
        {
            Debug.LogError(nameOfScene + " isn't the active Scene!");
        }
        
        // Wait for user input and handle it directly on the triggered object
        yield return new WaitUntil(() => 
            UserInput.Escape || 
            (UserInput.Movement.magnitude > 0.1f && 
                ItemObjectScript.currentTriggeredObject != null && 
                !ItemObjectScript.currentTriggeredObject.GetComponent<ItemObjectScript>().interactableWithChoice));
        
        // Handle escape key press
        if (UserInput.Escape)
        {
            yield return null;
            if (ItemObjectScript.currentTriggeredObject != null)
            {
                ItemObjectScript.currentTriggeredObject.GetComponent<ItemObjectScript>().SetWalkedAway();
            }
            ExitScene(nameOfScene);
        }
        // Handle walked away case (movement input)
        else if (UserInput.Movement.magnitude > 0.1f && 
                ItemObjectScript.currentTriggeredObject != null && 
                !ItemObjectScript.currentTriggeredObject.GetComponent<ItemObjectScript>().interactableWithChoice)
        {
            yield return null;
            ItemObjectScript.currentTriggeredObject.GetComponent<ItemObjectScript>().SetWalkedAway();
            ExitScene(nameOfScene);
        }
    }

    public void ExitScene(string nameOfScene)
    {
        Debug.Log("Currently exiting " + nameOfScene + ".");
        ItemObjectScript.inItemScene = false;
        _eventSystemInteractable.enabled = false;
        FloorManager.Instance.eventSystemMain.EnableEventSystem();
        SceneManager.UnloadSceneAsync(nameOfScene);

        
        FloorManager.Instance.player.GetComponent<PlayerMovement>().permaLockMovement = false;
        
    }

    public void StartOnClickYes()
    {
        StartCoroutine(OnClickYes());
    }
    
    private IEnumerator OnClickYes()
    {
        // Set the flag on the current object instead of using a static variable
        if (ItemObjectScript.currentTriggeredObject != null)
        {
            ItemObjectScript itemScript = ItemObjectScript.currentTriggeredObject.GetComponent<ItemObjectScript>();
            if (itemScript != null)
            {
                itemScript.SetClickedYes();
            }
        }

        if (itemScrub[ItemObjectScript.currentObjectInt].PublicBoolToChange != null)
            {
                if (itemScrub[ItemObjectScript.currentObjectInt].SetBoolToTrue)
                {
                    Debug.LogWarning("Floor Manager bool " + itemScrub[ItemObjectScript.currentObjectInt].PublicBoolToChange + " is set to true.");
                    FloorManager.Instance.SetBool(itemScrub[ItemObjectScript.currentObjectInt].PublicBoolToChange, true);
                }
                else
                {
                    //Debug.LogWarning("Floor Manager bool " + itemScrub[ItemObjectScript.currentObjectInt].PublicBoolToChange + " is set to false.");

                }
            }
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
            // Reset the clickedYes state if needed
        }
        
        else
        {
            yield return null;
            // No need to reset static clickedYes
        }
        ExitScene(SceneManager.GetActiveScene().name);
    }

    public void StartOnClickNo()
    {
        StartCoroutine(OnClickNo());
    }
    
    private IEnumerator OnClickNo()
    {
        // Set the flag on the current object instead of using a static variable
        if (ItemObjectScript.currentTriggeredObject != null)
        {
            ItemObjectScript itemScript = ItemObjectScript.currentTriggeredObject.GetComponent<ItemObjectScript>();
            if (itemScript != null)
            {
                itemScript.SetClickedNo();
            }
        }
        
        yield return null; // "yield return null" waits for 1 frame before continuing down.
        ExitScene(SceneManager.GetActiveScene().name);
    }

    // Handle "Yes" button click from UI
    public void OnYesButtonClick()
    {
        StartOnClickYes();
    }
    
    // Handle "No" button click from UI
    public void OnNoButtonClick()
    {
        StartOnClickNo();
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

