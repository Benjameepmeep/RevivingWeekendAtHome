using System.Collections;
using Pathfinding;
using UnityEngine;
using System;
using UnityEngine.Serialization;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

// Make sure this script runs before StairsTrigger.cs, in Project Settings, in Script Execution Order.
public class FloorManager : MonoBehaviour
    {
        public static readonly Lazy<FloorManager> _instance = new Lazy<FloorManager>(() => FindFirstObjectByType<FloorManager>()); 
        public static FloorManager Instance => _instance.Value;        

        [Header("Player")]
        public GameObject player;

        [Header("Bottom Floor")]
        public GameObject bottomFloor;
        public GameObject stairsLeadingUp;
        [Header("Top Floor")]
        public GameObject topFloor;
        public GameObject stairsLeadingDown;

        
        [Header("Cat")] 
        public GameObject cat;

        public BoxCollider2D catTriggerBox;

        public SpriteRenderer catSprite;

        [Header("PauseScreen")] 
        public GameObject pauseScreen;
        public PauseManager PauseManager;
        [Header("DataTransfer")] 
        public DataTransfer dataTransfer;
        [Header("EventSystem")]
        public EventSystemMain eventSystemMain;


        private float elapsedFadeTime;
        private float startExposure;
        private float endExposure;
        private bool isFadingToBlack = false;
        private float fadeDuration;

        private ColorAdjustments colorAdjustments;
        private Volume globalVolume;

        public bool evaluatingOutcome;



        // private void Awake()
        // {
        //     EnableAllFloorsAndItems();
        // }
        

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

     private void OnDisable()
    {        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player");
    }


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        Invoke(nameof(DisableAllFloorsExceptCurrent), 0.25f);

    }

    private void Update()
    {
        NullChecks();
    }

    private void NullChecks(){

        if (pauseScreen == null)
        {
            pauseScreen = GameObject.FindWithTag("PauseScene");
        }
    }

    public void EnableCatVisuals()
    {
        if (!dataTransfer.catIsDead)
        {
            int layerDefault = LayerMask.NameToLayer("Default");
            if (cat != null) cat.layer = layerDefault;
            if (catSprite != null)catSprite.enabled = true;

        }
    }

    public void DisableCatVisuals()
    {
        if (!dataTransfer.catIsDead)
        {
            int layerCat = LayerMask.NameToLayer("Cat");
            if (cat != null) cat.layer = layerCat;
            if (catSprite != null) catSprite.enabled = false;
        }
    }

    public void EnableAllFloorsAndItems()
    {

        if (catTriggerBox) catTriggerBox.enabled = true;


        if (catSprite && !dataTransfer.catIsDead) catSprite.enabled = true;
        
        if (pauseScreen) pauseScreen.SetActive(true);
        
    }

    public void DisableAllFloorsExceptCurrent()
    {
        if (pauseScreen != null) pauseScreen.SetActive(false);
        if (catTriggerBox != null) catTriggerBox.enabled = true;
        if (catSprite != null && !dataTransfer.catIsDead) catSprite.enabled = true;
        int layerDefault = LayerMask.NameToLayer("Default"); // Sets the cat layer to Default
        if (cat != null) cat.layer = layerDefault;
        // Debug.Log("Current cat layer: Default");

        if (!dataTransfer.catIsDead)
        {
            StartCoroutine(ScanAndContinue());
        }
        else
        {
            DisableCatStuff();
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

    
    public void SetBool(string name, bool value)
    {
        switch (name)
    {
        case "GoToNextDay":
            GoToNextDay();
            break;
        case "CatBowlFull":
            dataTransfer.CatBowlFull = value;
            break;

        case "Evaluate":
            
            EvaluateOutcome();
            break;
        default:
            Debug.LogError("FloorManager: SetBool: " + name + " is not a valid variable.");
            break;
    }
    }

    public void GoToNextDay()
    {
        globalVolume = FindFirstObjectByType<Volume>().GetComponent<Volume>();
        globalVolume.profile.TryGet(out colorAdjustments);

        StartCoroutine(FadeExposureCoroutine(true));

    }

    public void EvaluateOutcome()
{
    switch (true)
    {
        case bool _ when dataTransfer.catIsDead && dataTransfer.playerDied:
            dataTransfer.outcome = DataTransfer.Outcome.Player_dead_cat_dead;
            break;

        case bool _ when dataTransfer.catIsDead && !dataTransfer.playerDied:
            dataTransfer.outcome = DataTransfer.Outcome.Player_alive_cat_dead;
            break;

        case bool _ when !dataTransfer.catIsDead && !dataTransfer.playerDied:
            dataTransfer.outcome = DataTransfer.Outcome.Player_alive_cat_alive;
            break;

        case bool _ when dataTransfer.numberOfTimesOpenedDoorOrCatFlap < 1:
            dataTransfer.outcome = DataTransfer.Outcome.stayed_in_bed_all_day;
            break;

        default:
            break;
    }
}


    private IEnumerator FadeExposureCoroutine(bool toBlack)
    {
               
        fadeDuration = 3f; 
        elapsedFadeTime = 0f;

        if (toBlack)
        {
            startExposure = 0f;
            endExposure = -15f;

            isFadingToBlack = true;
        }
        else
        {
            startExposure = -15f;
            endExposure = 0f;
            isFadingToBlack = false; 
        }
       while (elapsedFadeTime < fadeDuration)
        {
            float newExposure = Mathf.Lerp(startExposure, endExposure, elapsedFadeTime / fadeDuration);
            colorAdjustments.postExposure.Override(newExposure);
    
            elapsedFadeTime += Time.unscaledDeltaTime;
            yield return null;
        }

        colorAdjustments.postExposure.Override(endExposure);

        
        elapsedFadeTime = 0f;
        
        if (toBlack)
        {
            isFadingToBlack = false;
        }

        LoadNextSceneInBuildArray();
    }

    public void LoadNextSceneInBuildArray(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
 
}
