using PlayerScripts;
using UnityEngine;

public class DataTransfer : MonoBehaviour
{
    // Static instance stored
    public static DataTransfer Instance;
    
    public bool lampOn;
    public static bool tvOn = true;
    public static bool radioOn = true;
    public static bool glassDoorOpen;
    public static bool bedroomDoorOpen;
    public static bool playerCanMove = true; // PlayerCanMove should be true from the start.
    public static bool onTopFloor;
    public static bool insideBathroom;
    public static bool playerInside = true;
    public static bool catFlapClosed = true;
    public static bool catIsDead;
    public static bool catOutside;
    public static bool isPause = false;
    public static int playerSortingOrder = 50;
    public static int catSortingOrderInside = 50;
    public const int CatSortingOrderOutside = -1;
    public static int vfxSortingOrder = 5;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);  // Ensures only one instance is available
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Update()
    {
        if (playerInside && !onTopFloor)
        {
            playerSortingOrder = 50;
            catSortingOrderInside = 50;
        }
        else if (playerInside && onTopFloor)
        {
            catSortingOrderInside = -1;
            
        }
        else if (!playerInside)
        {
            playerSortingOrder = -1;
            catSortingOrderInside = 50;
        }
    }
    public void TurnLampOnOrOff()
    {
        if (lampOn)
        {
           lampOn = false;
        }
        else if (!lampOn)
        {
            lampOn = true;
        }
    }
    public static void TurnTVOnOrOff()
    {
        if (tvOn)
        {
            tvOn = false;
        }
        else if (!tvOn)
        {
            tvOn = true;
        }
    }
    public static void TurnRadioOnOrOff()
    {
        if (radioOn)
        {
            radioOn = false;
        }
        else if (!radioOn)
        {
            radioOn = true;
        }
    }
    public static void OpenOrCloseGlassDoor()
    {
        if (glassDoorOpen)
        {
            glassDoorOpen = false;
        }
        else if (!glassDoorOpen)
        {
            glassDoorOpen = true;
        }
    }

    public static void OpenOrCloseBedroomDoor()
    {
        if (bedroomDoorOpen)
        {
            bedroomDoorOpen = false;
        }
        else if (!bedroomDoorOpen)
        {
            bedroomDoorOpen = true;
        }
    }
    public static void SwitchCanPlayerMove(UserInput userInput)
    {
        if (playerCanMove)
        {
            playerCanMove = false;
            userInput.OnDisable();
        }
        else if (!playerCanMove)
        {
            playerCanMove = true;
            userInput.OnEnable();
        }
    }
    public static void SwitchFloors()
    {
        if (onTopFloor)
        {
            onTopFloor = false;
            catSortingOrderInside = 50;
        }
        else if (!onTopFloor)
        {
            onTopFloor = true;
            catSortingOrderInside = -1;
        }
    }

    public static void EnterOrExitBathroom()
    {
        if (insideBathroom)
        {
            insideBathroom = false;
        }
        else
        {
            insideBathroom = true;
        }
    }

    public static void OpenOrCloseCatFlapDoor()
    {
        if (catFlapClosed)
        {
            catFlapClosed = false;
        }
        else
        {
            catFlapClosed = true;
        }
    }
    public static void PlayerInsideOrOutside()
    {
        if (playerInside)
        {
            playerSortingOrder = -1;
            vfxSortingOrder = 60;
            playerInside = false;
        }
        else if (!playerInside)
        {
            playerSortingOrder = 50;
            vfxSortingOrder = 5;
            playerInside = true;
        }
    }
}
