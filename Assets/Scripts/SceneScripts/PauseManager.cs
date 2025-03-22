using System.Collections;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    public UserInput userInput;

    [SerializeField] private bool pausedDuringCutscene;


    private void Start()
    {

        CheckForFloorManager();

        CheckForPlayer();
        
    }

    private void CheckForFloorManager()
    {
        if (FloorManager.Instance == null)
        {
            Invoke("CheckForFloorManager", 0.3f);
            return;
        }
        pauseCanvas = FloorManager.Instance.pauseScreen;;

        if (pauseCanvas == null)
            {
                pauseCanvas = FloorManager.Instance.pauseScreen;
            }
            if (pauseCanvas == null)
            {
                pauseCanvas = GameObject.FindGameObjectWithTag("PauseScene");
            }

        if (pauseCanvas.activeSelf)
        {
            if (pauseCanvas == null)
            {
                pauseCanvas = FloorManager.Instance.pauseScreen;
            }
            if (pauseCanvas == null)
            {
                pauseCanvas = GameObject.FindGameObjectWithTag("PauseScene");
            }

            FloorManager.Instance.dataTransfer.isPause = false;
            SetPauseScreenInactive();
        }
    }

    private void Update()
    {
        if (userInput == null)
        {
            CheckForPlayer();
            return;
        } 

        if (UserInput.Escape)
        {
            StartCoroutine(AreWePausing(false));
        }
        if (UserInput.PauseDuringCutscene)
        {
            StartCoroutine(AreWePausing(true));
        }
        if (UserInput.Unpause)
        {
            SetPauseScreenInactive();
        }
    }

    private IEnumerator AreWePausing(bool duringCutscene)
    {
        if (ItemObjectScript.inItemScene) yield break;
        pausedDuringCutscene = duringCutscene;
        Debug.Log("Setting pauseScreen Active.");
        SetPauseScreenActive();
    }

    private void SetPauseScreenActive()
    {
        Time.timeScale = 0f;
        CheckForPlayer();
        userInput.SwitchInputToPauseScreen();
        pauseCanvas.SetActive(true);
        FloorManager.Instance.dataTransfer.isPause = true;
    }

    public void SetPauseScreenInactive()
    {
        Time.timeScale = 1f;
        CheckForPlayer();
        if (pausedDuringCutscene)
        {
            userInput.SwitchInputToCutscene();
        }
        else
        {
            userInput.SwitchInputToGameScene();
        }
        pausedDuringCutscene = false;
        pauseCanvas.SetActive(false);
        FloorManager.Instance.dataTransfer.isPause = false; 
    }

    private void CheckForPlayer(){

        if (userInput == null)
        {
            userInput = GameObject.FindGameObjectWithTag("Player").GetComponent<UserInput>();
        }

    }

    public void GoBackToMainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        CheckForPlayer();

        pausedDuringCutscene = false;
        pauseCanvas.SetActive(false);

        userInput.SwitchInputToTitleScreen();
        FloorManager.Instance.dataTransfer.isPause = false;

        FindFirstObjectByType<BackToMainMenu>().BackToMainMenuScene(sceneName);
    }
}

