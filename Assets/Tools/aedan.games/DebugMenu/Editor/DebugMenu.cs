using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

public class DebugMenu : EditorWindow
{
    private string[] aedanScenes = new string[]
    {
        "Assets/Scenes/AedanScenes/Scene 0 - Title Screen.unity",
        "Assets/Scenes/AedanScenes/Scene 1 - Day 1 Evening.unity",
        "Assets/Scenes/AedanScenes/Scene 2 - Day 2 Morning.unity",
        "Assets/Scenes/AedanScenes/Scene 3 - Day 2 Evening.unity",
        "Assets/Scenes/AedanScenes/Scene 4 - Day 3 Morning 1.unity",
        "Assets/Scenes/AedanScenes/Scene 5 - Day 3 Morning 2.unity",
        "Assets/Scenes/AedanScenes/Scene 6 - Day 3 Evening.unity"
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

    [MenuItem("Tools/aedan.games/DebugMenu")]
    public static void ShowWindow()
    {
        GetWindow<DebugMenu>("Debug Menu");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Event System", EditorStyles.boldLabel);

        if (GUILayout.Button("Enable Event System"))
        {
            EventSystemMain.Instance.EnableEventSystem();
        }

        if (GUILayout.Button("Disable Event System"))
        {
            EventSystemMain.Instance.DisableEventSystem();
        }

        EditorGUILayout.Space(10);
        GUILayout.Label("Scene Management", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Previous Scene"))
        {
            LoadScene(false);
        }
        if (GUILayout.Button("Next Scene"))
        {
            LoadScene(true);
        }
        EditorGUILayout.EndHorizontal();
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
            Debug.LogError("Current scene is not in any defined scene list.");
            return;
        }
        int totalScenes = sceneList.Length;
        // Calculate next or previous index using modulo arithmetic
        int targetIndex = next ? (index + 1) % totalScenes : (index + totalScenes - 1) % totalScenes;
        EditorSceneManager.OpenScene(sceneList[targetIndex]);
    }
}