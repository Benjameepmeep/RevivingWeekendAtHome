using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using UnityEngine.Playables;
using System.Collections.Generic;

public class DebugMenu : EditorWindow
{
    private string[] aedanScenes = new string[]
    {
        "Assets/Scenes/AedanScenes/AScene 0 - Title Screen.unity",
        "Assets/Scenes/AedanScenes/AScene 1 - Day 1 Evening.unity",
        "Assets/Scenes/AedanScenes/AScene 2 - Day 2 Morning.unity",
        "Assets/Scenes/AedanScenes/AScene 3 - Day 2 Evening.unity",
        "Assets/Scenes/AedanScenes/AScene 4 - Day 3 Morning 1.unity",
        "Assets/Scenes/AedanScenes/AScene 5 - Day 3 Morning 2.unity",
        "Assets/Scenes/AedanScenes/AScene 6 - Day 3 Evening.unity"
    };

    private string[] scenes = new string[]
    {
        "Assets/Scenes/Scene 0 - Title Screen.unity",
        "Assets/Scenes/Scene 1 - Day 1 Evening.unity",
        "Assets/Scenes/Scene 2 - Day 2 Morning.unity",
        "Assets/Scenes/Scene 3 - Day 2 Evening.unity",
        "Assets/Scenes/Scene 4 - Day 3 Morning 1.unity",
        "Assets/Scenes/Scene 5 - Day 3 Morning 2.unity",
        "Assets/Scenes/Scene 6 - Day 3 Evening.unity"
    };

    // Dictionary to track boolean values from DataTransfer
    private Dictionary<string, bool> dataTransferBools = new Dictionary<string, bool>()
    {
        { "lampOn", false },
        { "tvOn", true },
        { "radioOn", true },
        { "glassDoorOpen", false },
        { "bedroomDoorOpen", false },
        { "playerCanMove", true },
        { "onTopFloor", false },
        { "insideBathroom", false },
        { "playerInside", true },
        { "catFlapClosed", true },
        { "catIsDead", false },
        { "catOutside", false },
        { "CatBowlFull", false },
        { "isPause", false },
        { "playerDied", false }
    };
    
    // Track integer and enum values
    private int numberOfTimesOpenedDoorOrCatFlap = 0;
    private string selectedOutcome = "Player_alive_cat_alive"; // Default value
    
    private bool foldoutPublicBools = false;

    // Track the event system state
    private bool isEventSystemEnabled = true;

    [MenuItem("Tools/aedan.games/DebugMenu")]
    public static void ShowWindow()
    {
        GetWindow<DebugMenu>("Debug Menu");
    }

    // Register global keyboard shortcut for T key (works outside game view)
    [MenuItem("Tools/aedan.games/Skip Timeline %t", false, 100)]
    public static void SkipTimelineShortcut()
    {
        // Get the window instance or create it if it doesn't exist
        DebugMenu window = GetWindow<DebugMenu>("Debug Menu");
        window.SkipToEndOfTimeline();
    }

    private void OnEnable()
    {
        // Register for editor update events to handle key presses
        EditorApplication.update += OnEditorUpdate;
        
        // Register for play mode events
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        
        // For keyboard handling in game view during play mode
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        // Unregister from all events when window is closed
        EditorApplication.update -= OnEditorUpdate;
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        // Make sure the event handlers are registered/unregistered correctly when play mode changes
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            // Add additional event handlers for play mode
            EditorApplication.update += CheckKeyPressInPlayMode;
        }
        else if (state == PlayModeStateChange.ExitingPlayMode)
        {
            // Remove play mode specific handlers
            EditorApplication.update -= CheckKeyPressInPlayMode;
        }
    }

    private void CheckKeyPressInPlayMode()
    {
        // Check for T key press specifically for play mode
        if (UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            SkipToEndOfTimeline();
        }
        
        // Check and update DataTransfer boolean values
        UpdateDataTransferBooleanValues();
    }

    private void UpdateDataTransferBooleanValues()
    {
        if (EditorApplication.isPlaying && FloorManager.Instance != null)
        {
            DataTransfer dataTransfer = FloorManager.Instance.dataTransfer;
            if (dataTransfer != null)
            {
                // Update all boolean values from DataTransfer
                UpdateBoolValue("lampOn", dataTransfer.lampOn);
                UpdateBoolValue("tvOn", dataTransfer.tvOn);
                UpdateBoolValue("radioOn", dataTransfer.radioOn);
                UpdateBoolValue("glassDoorOpen", dataTransfer.glassDoorOpen);
                UpdateBoolValue("bedroomDoorOpen", dataTransfer.bedroomDoorOpen);
                UpdateBoolValue("playerCanMove", dataTransfer.playerCanMove);
                UpdateBoolValue("onTopFloor", dataTransfer.onTopFloor);
                UpdateBoolValue("playerInside", dataTransfer.playerInside);
                UpdateBoolValue("catFlapClosed", dataTransfer.catFlapClosed);
                UpdateBoolValue("catIsDead", dataTransfer.catIsDead);
                UpdateBoolValue("catOutside", dataTransfer.catOutside);
                UpdateBoolValue("CatBowlFull", dataTransfer.CatBowlFull);
                UpdateBoolValue("isPause", dataTransfer.isPause);
                UpdateBoolValue("playerDied", dataTransfer.playerDied);
                
                // Update integer and enum values
                numberOfTimesOpenedDoorOrCatFlap = dataTransfer.numberOfTimesOpenedDoorOrCatFlap;
                selectedOutcome = dataTransfer.outcome.ToString();
            }
        }
    }

    private void UpdateBoolValue(string key, bool currentValue)
    {
        // Update local dictionary if value changed
        if (dataTransferBools.ContainsKey(key) && dataTransferBools[key] != currentValue)
        {
            dataTransferBools[key] = currentValue;
            Repaint(); // Refresh UI when values change
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        // Handle keyboard input in the scene view 
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.T)
        {
            SkipToEndOfTimeline();
            e.Use();
        }
    }

    private void OnEditorUpdate()
    {
        // Check for T key press globally in editor
        if (Event.current != null && Event.current.isKey && 
            Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.T)
        {
            SkipToEndOfTimeline();
            Event.current.Use();
        }
        
        // Additional check using Input system for when Game view has focus
        if (EditorApplication.isPlaying && UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            SkipToEndOfTimeline();
        }
        
        // Keep checking DataTransfer boolean values
        UpdateDataTransferBooleanValues();
    }

    private void OnGUI()
    {
        // Check for T key press in this window
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.T)
        {
            SkipToEndOfTimeline();
            e.Use(); // Mark event as used to prevent it from being processed further
        }

        GUILayout.Space(10);
        GUILayout.Label("Event System", EditorStyles.boldLabel);

        // Single toggle button for event system
        string buttonText = isEventSystemEnabled ? "Disable Event System" : "Enable Event System";
        if (GUILayout.Button(buttonText))
        {
            isEventSystemEnabled = !isEventSystemEnabled;
            if (isEventSystemEnabled)
            {
                FloorManager.Instance.eventSystemMain.EnableEventSystem();
            }
            else
            {
                FloorManager.Instance.eventSystemMain.DisableEventSystem();
            }
        }

        EditorGUILayout.Space(10);
        GUILayout.Label("Public Bools", EditorStyles.boldLabel);

        // Dropdown for public bools
        foldoutPublicBools = EditorGUILayout.Foldout(foldoutPublicBools, "List of DataTransfer Bools", true);
        if (foldoutPublicBools)
        {
            EditorGUI.indentLevel++;
            
            // Display and allow editing of all DataTransfer boolean values
            DisplayBooleanToggles();
            
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(10);
        GUILayout.Label("Scene Management", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("← Previous Scene"))
        {
            LoadScene(false);
        }
        if (GUILayout.Button("Next Scene →"))
        {
            LoadScene(true);
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(10);
        GUILayout.Label("Timeline Controls", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Skip to End of Current Timeline"))
        {
            SkipToEndOfTimeline();
        }
        
        EditorGUILayout.Space(10);
        GUILayout.Label("Editor Utilities", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Reload Domain"))
        {
            ReloadEditorDomain();
        }
    }

    private void DisplayBooleanToggles()
    {
        if (!EditorApplication.isPlaying || FloorManager.Instance == null || FloorManager.Instance.dataTransfer == null)
        {
            EditorGUILayout.HelpBox("Enter Play Mode to view and change DataTransfer values.", MessageType.Info);
            return;
        }

        DataTransfer dataTransfer = FloorManager.Instance.dataTransfer;
        
        // Format property names for better readability in the UI
        EditorGUI.BeginChangeCheck();
        bool newLampOn = EditorGUILayout.Toggle("Lamp On", dataTransferBools["lampOn"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.lampOn != newLampOn)
        {
            dataTransfer.lampOn = newLampOn;
            dataTransferBools["lampOn"] = newLampOn;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newTvOn = EditorGUILayout.Toggle("TV On", dataTransferBools["tvOn"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.tvOn != newTvOn)
        {
            dataTransfer.tvOn = newTvOn;
            dataTransferBools["tvOn"] = newTvOn;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newRadioOn = EditorGUILayout.Toggle("Radio On", dataTransferBools["radioOn"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.radioOn != newRadioOn)
        {
            dataTransfer.radioOn = newRadioOn;
            dataTransferBools["radioOn"] = newRadioOn;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newGlassDoorOpen = EditorGUILayout.Toggle("Glass Door Open", dataTransferBools["glassDoorOpen"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.glassDoorOpen != newGlassDoorOpen)
        {
            dataTransfer.glassDoorOpen = newGlassDoorOpen;
            dataTransferBools["glassDoorOpen"] = newGlassDoorOpen;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newBedroomDoorOpen = EditorGUILayout.Toggle("Bedroom Door Open", dataTransferBools["bedroomDoorOpen"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.bedroomDoorOpen != newBedroomDoorOpen)
        {
            dataTransfer.bedroomDoorOpen = newBedroomDoorOpen;
            dataTransferBools["bedroomDoorOpen"] = newBedroomDoorOpen;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newPlayerCanMove = EditorGUILayout.Toggle("Player Can Move", dataTransferBools["playerCanMove"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.playerCanMove != newPlayerCanMove)
        {
            dataTransfer.playerCanMove = newPlayerCanMove;
            dataTransferBools["playerCanMove"] = newPlayerCanMove;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newOnTopFloor = EditorGUILayout.Toggle("On Top Floor", dataTransferBools["onTopFloor"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.onTopFloor != newOnTopFloor)
        {
            dataTransfer.onTopFloor = newOnTopFloor;
            dataTransferBools["onTopFloor"] = newOnTopFloor;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newPlayerInside = EditorGUILayout.Toggle("Player Inside", dataTransferBools["playerInside"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.playerInside != newPlayerInside)
        {
            dataTransfer.playerInside = newPlayerInside;
            dataTransferBools["playerInside"] = newPlayerInside;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newCatFlapClosed = EditorGUILayout.Toggle("Cat Flap Closed", dataTransferBools["catFlapClosed"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.catFlapClosed != newCatFlapClosed)
        {
            dataTransfer.catFlapClosed = newCatFlapClosed;
            dataTransferBools["catFlapClosed"] = newCatFlapClosed;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newCatIsDead = EditorGUILayout.Toggle("Cat Is Dead", dataTransferBools["catIsDead"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.catIsDead != newCatIsDead)
        {
            dataTransfer.catIsDead = newCatIsDead;
            dataTransferBools["catIsDead"] = newCatIsDead;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newCatOutside = EditorGUILayout.Toggle("Cat Outside", dataTransferBools["catOutside"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.catOutside != newCatOutside)
        {
            dataTransfer.catOutside = newCatOutside;
            dataTransferBools["catOutside"] = newCatOutside;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newCatBowlFull = EditorGUILayout.Toggle("Cat Bowl Full", dataTransferBools["CatBowlFull"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.CatBowlFull != newCatBowlFull)
        {
            dataTransfer.CatBowlFull = newCatBowlFull;
            dataTransferBools["CatBowlFull"] = newCatBowlFull;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newIsPause = EditorGUILayout.Toggle("Is Pause", dataTransferBools["isPause"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.isPause != newIsPause)
        {
            dataTransfer.isPause = newIsPause;
            dataTransferBools["isPause"] = newIsPause;
        }
        
        EditorGUI.BeginChangeCheck();
        bool newPlayerDied = EditorGUILayout.Toggle("Player Died", dataTransferBools["playerDied"]);
        if (EditorGUI.EndChangeCheck() && dataTransfer.playerDied != newPlayerDied)
        {
            dataTransfer.playerDied = newPlayerDied;
            dataTransferBools["playerDied"] = newPlayerDied;
        }
        
        EditorGUILayout.Space(10);
        GUILayout.Label("Integer Values", EditorStyles.boldLabel);
        
        EditorGUI.BeginChangeCheck();
        int newDoorOpenCount = EditorGUILayout.IntField("Times Opened Door/Cat Flap", numberOfTimesOpenedDoorOrCatFlap);
        if (EditorGUI.EndChangeCheck() && dataTransfer.numberOfTimesOpenedDoorOrCatFlap != newDoorOpenCount)
        {
            dataTransfer.numberOfTimesOpenedDoorOrCatFlap = newDoorOpenCount;
            numberOfTimesOpenedDoorOrCatFlap = newDoorOpenCount;
        }
        
        EditorGUILayout.Space(10);
        GUILayout.Label("Enum Values", EditorStyles.boldLabel);
        
        EditorGUI.BeginChangeCheck();
        string[] outcomeOptions = System.Enum.GetNames(typeof(DataTransfer.Outcome));
        int selectedIndex = Array.IndexOf(outcomeOptions, selectedOutcome);
        selectedIndex = EditorGUILayout.Popup("Game Outcome", selectedIndex, outcomeOptions);
        if (EditorGUI.EndChangeCheck() && selectedIndex >= 0)
        {
            selectedOutcome = outcomeOptions[selectedIndex];
            dataTransfer.outcome = (DataTransfer.Outcome)System.Enum.Parse(typeof(DataTransfer.Outcome), selectedOutcome);
        }
    }

    private void LoadScene(bool next)
    {
        string currentScenePath = EditorSceneManager.GetActiveScene().path;
        string[] sceneList = null;
        int index = Array.IndexOf(aedanScenes, currentScenePath); // Checks if current scene is in aedanScenes
        if (index != -1)
        {
            sceneList = aedanScenes;
        }
        else
        {
            index = Array.IndexOf(scenes, currentScenePath);
            if (index != -1)
            {
                sceneList = scenes;
            }
        }
        if (sceneList == null)
        {
            //Debug.LogError("Current scene is not in any defined scene list.");

            EditorSceneManager.OpenScene(aedanScenes[0]);
            return;
        }
        int totalScenes = sceneList.Length;
        // Calculate next or previous index using modulo arithmetic
        int targetIndex = next ? (index + 1) % totalScenes : (index + totalScenes - 1) % totalScenes;
        EditorSceneManager.OpenScene(sceneList[targetIndex]);
    }
    
    private void SkipToEndOfTimeline()
    {
        PlayableDirector[] directors = FindObjectsByType<PlayableDirector>(FindObjectsSortMode.None);
        bool foundPlayingTimeline = false;
        
        foreach (PlayableDirector director in directors)
        {
            if (director.state == PlayState.Playing)
            {
                // Get the duration of the timeline
                double duration = director.duration;
                
                // Set the time to the end of the timeline
                director.time = duration;
                director.Evaluate();
                
                Debug.Log($"Skipped to the end of timeline on {director.gameObject.name} (Duration: {duration})");
                foundPlayingTimeline = true;
            }
        }
        
        if (!foundPlayingTimeline)
        {
            Debug.Log("No playing timeline found in the current scene.");
        }
    }
    
    private void ReloadEditorDomain()
    {
        Debug.Log("Reloading editor domain...");
        EditorUtility.RequestScriptReload();
    }
}