using System.Collections;
using Pathfinding;
using UnityEngine;
using System;
using UnityEngine.Serialization;

// Make sure this script runs before StairsTrigger.cs, in Project Settings, in Script Execution Order.
public class FloorManager : MonoBehaviour
    {
        public static readonly Lazy<FloorManager> _instance = new Lazy<FloorManager>(() => FindFirstObjectByType<FloorManager>()); 
        public static FloorManager Instance => _instance.Value;        


        [Header("Bottom Floor")]
        public GameObject bottomFloor;
        public GameObject stairsBottomFloor;
        [FormerlySerializedAs("toesTriggerBox")] public BoxCollider2D catTriggerBox;
        
        [Header("Top Floor")]
        public GameObject topFloor;
        public GameObject stairsTopFloor;
        public GameObject bathroomTrigger;

        [Header("Cat")] 
        public GameObject cat;
        public SpriteRenderer catSprite;

        [Header("PauseScreen")] 
        public GameObject pauseScreen;
        public PauseManager PauseManager;

        // Awake is called before any Start functions, across scripts.
        // private void Awake()
        // {
        //     EnableAllFloorsAndItems();
        // }
        
        // Start is called before the first frame update
        private void Start()
        {
            Invoke(nameof(DisableAllFloorsExceptCurrent), 0.25f);
        }

    private void Update()
    {
        NullChecks();
    }

    private void NullChecks(){

        // if (bottomFloor == null)
        // {
        //     bottomFloor = GameObject.FindWithTag("BottomFloor");
        // }

        // if (stairsBottomFloor == null)
        // {
        //     stairsBottomFloor = GameObject.FindWithTag("StairsBottomFloor");
        // }

        // if (catTriggerBox == null)
        // {
        //     catTriggerBox = GameObject.FindWithTag("CatPNG").GetComponent<BoxCollider2D>();
        // }

        // if (topFloor == null)
        // {
        //     topFloor = GameObject.FindWithTag("TopFloor");
        // }

        // if (stairsTopFloor == null)
        // {
        //     stairsTopFloor = GameObject.FindWithTag("StairsTopFloor");
        // }

        // if (bathroomTrigger == null)
        // {
        //     bathroomTrigger = GameObject.FindWithTag("BathroomTrigger");
        // }

        // if (cat == null)
        // {
        //     cat = GameObject.FindWithTag("Cat");
        // }

        // if (catSprite == null)
        // {
        //     catSprite = GameObject.FindWithTag("CatSprite").GetComponent<SpriteRenderer>();
        // }

        if (pauseScreen == null)
        {
            pauseScreen = GameObject.FindWithTag("PauseScreen");
        }
    }


    public void EnableAllFloorsAndItems()
    {
        if (bottomFloor) bottomFloor.SetActive(true);
        if (stairsBottomFloor) stairsBottomFloor.SetActive(true);
        if (catTriggerBox) catTriggerBox.enabled = true;

        if (topFloor) topFloor.SetActive(true);
        if (stairsTopFloor) stairsTopFloor.SetActive(true);
        if (bathroomTrigger) bathroomTrigger.SetActive(true);

        if (catSprite) catSprite.enabled = true;
        
        if (pauseScreen) pauseScreen.SetActive(true);
        
    }

    public void DisableAllFloorsExceptCurrent()
    {
        if (topFloor != null) topFloor.SetActive(false);
        if (stairsTopFloor != null) stairsTopFloor.SetActive(false);
        if (bathroomTrigger != null) bathroomTrigger.SetActive(false);
        if (pauseScreen != null) pauseScreen.SetActive(false);
        if (catTriggerBox != null) catTriggerBox.enabled = true;
        if (catSprite != null) catSprite.enabled = true;
        int layerDefault = LayerMask.NameToLayer("Default"); // Sets the cat layer to Default
        if (cat != null) cat.layer = layerDefault;
        // Debug.Log("Current cat layer: Default");

        if (!DataTransfer.catIsDead)
        {
            StartCoroutine(ScanAndContinue());
        }
        else
        {
            DisableCatStuff();
        }
        
        if (DataTransfer.onTopFloor)
        {
            ApplyTopFloorSettings();
        }
    }

    // Coroutine that waits until scanning is complete.
    private IEnumerator ScanAndContinue()
    {
        AstarPath.active.ScanAsync();
        yield return new WaitUntil(() => !AstarPath.active.isScanning);
        // Continue with any post-scan logic if needed.
        yield break;
    }

    // Disables the "CatStuff" GameObject.
    private void DisableCatStuff()
    {
        GameObject catStuff = GameObject.FindWithTag("CatStuff");
        if (catStuff != null)
        {
            catStuff.SetActive(false);
        }
    }

    // Applies settings specific to when DataTransfer.onTopFloor is true.
    private void ApplyTopFloorSettings()
    {
        if (bottomFloor != null) bottomFloor.SetActive(false);
        if (stairsBottomFloor != null) stairsBottomFloor.SetActive(false);
        if (topFloor != null) topFloor.SetActive(false);
        if (catTriggerBox != null) catTriggerBox.enabled = false;
        if (catSprite != null) catSprite.enabled = false;
        if (pauseScreen != null) pauseScreen.SetActive(false);

        int layerCat = LayerMask.NameToLayer("Cat");
        cat.layer = layerCat;
        Debug.Log("Current cat layer: Cat");
    }
}
