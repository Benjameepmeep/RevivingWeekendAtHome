using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    private PlayerControls _controls;
    private float _interactCooldownEnd = 0f; // added cooldown timer

    public bool titleScreenControlsActive;
    public bool pauseScreenControlsActive;
    public bool gameScreenControlsActive;
    public bool cutsceneControlsActive;

    private void Awake() => _controls = new PlayerControls();

    // Use this after cutscenes!
    public void OnEnable()
    {
        _controls.Enable(); 
    }

    // Use this during cutscenes!
    public void OnDisable()
    { 
        _controls.Disable();
    }

    private void Start()
    {
        if(FloorManager.Instance.PauseManager != null)
        {        
            FloorManager.Instance.PauseManager.userInput = this;
        }
        else
        {
            FindFirstObjectByType<PauseManager>().userInput = this;
        }
        InputSystem.settings.maxEventBytesPerUpdate = 0;
    }

    public void SwitchInputToPauseScreen()
    {
        _controls.Player.Disable();
        gameScreenControlsActive = false;
        
        _controls.TitleScreen.Enable();
        titleScreenControlsActive = true;
        
        _controls.Cutscene.Disable();
        cutsceneControlsActive = false;
        
        _controls.PauseScreen.Enable();
        pauseScreenControlsActive = true;
    }
    
    public void SwitchInputToGameScene()
    {
        _controls.PauseScreen.Disable();
        pauseScreenControlsActive = false;
        
        _controls.TitleScreen.Disable();
        titleScreenControlsActive = false;
        
        _controls.Cutscene.Disable();
        cutsceneControlsActive = false;
        
        _controls.Player.Enable();
        gameScreenControlsActive = true;
    }
    
    public void SwitchInputToTitleScreen()
    {
        _controls.PauseScreen.Enable();
        pauseScreenControlsActive = true;
        
        _controls.Player.Disable();
        gameScreenControlsActive = false;
        
        _controls.Cutscene.Disable();
        cutsceneControlsActive = false;
        
        _controls.TitleScreen.Enable();
        titleScreenControlsActive = true;
    }

    public void SwitchInputToCutscene()
    {
        _controls.PauseScreen.Disable();
        pauseScreenControlsActive = false;
        
        _controls.Player.Disable();
        gameScreenControlsActive = false;
        
        _controls.TitleScreen.Disable();
        titleScreenControlsActive = false;
        
        _controls.Cutscene.Enable();
        cutsceneControlsActive = true;
    }
    
    // Player ActionMap Controls:
    public static Vector2 Movement { get; private set; }
    public static bool Interact { get; private set; }
    public static bool Escape { get; private set; }
    
    // Title Screen ActionMap Controls:
    public static bool AnyKeyOrStart { get; private set; }
    public static bool QuitGame { get; private set; }

    // PauseScreen ActionMap Controls:
    public Vector2 Move { get; private set; }
    public static bool Unpause { get; private set; }
    public static bool Select { get; private set; }
    
    // Cutscene ActionMap Controls:
    public static bool PauseDuringCutscene { get; private set; }
    
    public void Update()
    {
        Movement = _controls.Player.Movement.ReadValue<Vector2>();

        // Add 1 second cooldown to interaction
        if (Time.time > _interactCooldownEnd && _controls.Player.Interact.triggered)
        {
            Interact = true;
            _interactCooldownEnd = Time.time + 1f;
        }
        else if (_controls.Player.Interact.triggered)
        {
            Debug.LogWarning("Interact cooldown active");
            Interact = false;
        }
        else
        {
            Interact = false;
        }

        Escape = _controls.Player.OpenPauseMenu.triggered;
        AnyKeyOrStart = _controls.TitleScreen.AnyKeyorStart.triggered;
        QuitGame = _controls.TitleScreen.QuitGame.triggered;

        Move = _controls.PauseScreen.Move.ReadValue<Vector2>();
        Unpause = _controls.PauseScreen.Unpause.triggered;
        Select = _controls.PauseScreen.Select.triggered;

        PauseDuringCutscene = _controls.Cutscene.OpenPauseMenu.triggered;
    }
}
