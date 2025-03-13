using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
    {
        public static Scene currentGameScene;
        
        public UserInput userInput;
        private PlayerControls _controls;
        public GameObject transferBetweenLoad;
        public Animator transition;
        public bool noFadeOut;
        public float transitionTime = 2f;
        [SerializeField] private Scene[] scenesToLoad;
        private int currentSceneArrayIndex = 0;
        
        [Header("Transition Speed plays at default speed at 1")]
        [SerializeField] private float transitionSpeed = 0.5f;
        private static readonly int StartFade = Animator.StringToHash("StartFade");

        private void Awake() => _controls = new PlayerControls();
        private void OnEnable() => _controls.Enable();
        private void OnDisable() => _controls.Disable();

        private void CheckForPlayer(){

            if (userInput == null)
            {
                userInput = GameObject.FindGameObjectWithTag("Player").GetComponent<UserInput>();
            }

        }
        
        public void Start()
        {
            CheckForPlayer();

            currentSceneArrayIndex = 0;

            transition.speed = transitionSpeed;
            //Play a no-fade animation if there is no fade out requested
            transition.Play(noFadeOut ? "Crossfade_Idle" : "Crossfade_End");

            currentGameScene = SceneManager.GetActiveScene();
            Debug.Log("This is the " + currentGameScene.name + " with the index: " + currentGameScene.buildIndex);
            if (currentGameScene.buildIndex == 0 || !_controls.TitleScreen.enabled)
            {
                userInput.SwitchInputToTitleScreen();
            }
            else
            {
                userInput.SwitchInputToGameScene();
            }
        }

        private void Update()
        {
            CheckForPlayer();

            if (userInput.titleScreenControlsActive)
            {
                if (UserInput.QuitGame)
                {
                    // TODO: Run a new scene window to ask if they really want to quit the game. Do the same for pauseScreen, but let pauseScreen go to TitleScreen.
                    BeforeQuittingTheGame();
                }
                else if (UserInput.AnyKeyOrStart)
                {
                    LoadNextLevelByIndex();
                }
            }
        }

        private void BeforeQuittingTheGame()
        {
            CheckForPlayer();

            if (UserInput.QuitGame)
            {
                QuitTheGame();
            }
            else
            {
                UnloadQuitWarningScene();
            }
        }

        public void QuitTheGame()
        {
            Debug.Log("Quitting the Game");
            Application.Quit();
        }

        public void UnloadQuitWarningScene()
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("QuitWarning Scene"));
        }

        public void LoadNextLevelByIndex()
        {
            //Start a coroutine to wait until the fade is done
            StartCoroutine(LoadLevel(SceneManager.GetSceneByBuildIndex(currentGameScene.buildIndex).buildIndex + 1));
        }

        public void LoadSceneByName(string sceneName)
        {
            //Start a coroutine to wait until the fade is done
            StartCoroutine(LoadLevel(SceneManager.GetSceneByName(sceneName).buildIndex));
        }

        public void LoadSceneByNextInArray(){

            if (currentSceneArrayIndex < scenesToLoad.Length)
            {
                StartCoroutine(LoadLevel(scenesToLoad[currentSceneArrayIndex].buildIndex));
                currentSceneArrayIndex++;
            }
            else
            {
                Debug.Log("No more scenes to load.");
            }


        }
        

        public void LoadTitleScreen()
        {
            SceneManager.LoadScene("Scenes/Scene 0 - Title Screen");
            
            // TODO: Something tells me this Destroy below needs to be put elsewhere outside the gameObject this script is on...
            Destroy(transferBetweenLoad);
        }

        private IEnumerator LoadLevel(int levelIndex)
        {
            // Start the fade animation and then wait until the transition time expires
            transition.SetTrigger(StartFade);
            yield return new WaitForSeconds(transitionTime + 0.1f);
            SceneManager.LoadScene(levelIndex);
        }
}
