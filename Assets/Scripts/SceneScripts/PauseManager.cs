using System.Collections;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    public UserInput userInput;

    [SerializeField] private bool pausedDuringCutscene;


    private void Start()
    {

        pauseCanvas = FloorManager.Instance.pauseScreen;;

        CheckForPlayer();


        if (pauseCanvas.activeSelf)
        {
            DataTransfer.isPause = false;
            SetPauseScreenInactive();
        }

        
    }

    private void Update()
    {
        if (userInput == null) return;

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
        DataTransfer.isPause = true;
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
        DataTransfer.isPause = false; 
    }

    private void CheckForPlayer(){

        if (userInput == null)
        {
            userInput = GameObject.FindGameObjectWithTag("Player").GetComponent<UserInput>();
        }

    }
}

