using UnityEngine;
using System;

public class DataTransfer : MonoBehaviour
{
        
    public bool lampOn;
    public bool tvOn = true;
    public bool radioOn = true;
    public bool glassDoorOpen;
    public bool bedroomDoorOpen;
    public bool playerCanMove = true; // PlayerCanMove should be true from the start.
    public bool onTopFloor;
    public bool playerInside = true;
    public bool catFlapClosed = true;
    public bool catIsDead;
    public bool catOutside;
    public bool CatBowlFull;

    public bool playerDied;

    public int numberOfTimesOpenedDoorOrCatFlap;

    public enum Outcome
        {
            Player_alive_cat_alive,
            Player_alive_cat_dead,
            Player_dead_cat_dead,

            stayed_in_bed_all_day,
        }

    public Outcome outcome;
    public bool isPause = false;
    public int playerSortingOrder = 50;
    public int catSortingOrderInside = 50;
    public int CatSortingOrderOutside = -1;
    public int vfxSortingOrder = 5;
    
    
    private void Update()
    {
        if (playerInside)
        {
            playerSortingOrder = 50;
            catSortingOrderInside = 50;
        }
        else if (!playerInside)
        {
            playerSortingOrder = -1;
            catSortingOrderInside = 50;
        }
    }
    public void ToggleLamp()
    {
        lampOn = !lampOn;
    }
    public void ToggleTVOn()
    {
        tvOn = !tvOn;
    }
    public void ToggleRadio()
    {
        radioOn = !radioOn;
    }
    public void OpenOrCloseGlassDoor()
    {
        glassDoorOpen = !glassDoorOpen;
    }

    public void OpenOrCloseBedroomDoor()
    {
       bedroomDoorOpen = !bedroomDoorOpen;
    }
    public void SwitchCanPlayerMove(UserInput userInput)
    {
        if (playerCanMove)
        {
            playerCanMove = false;
            userInput.OnDisable();
        }
        else
        {
            playerCanMove = true;
            userInput.OnEnable();
        }
    }
    public void SwitchFloors()
    {
        if (onTopFloor)
        {
            catSortingOrderInside = 50;
        }
        else
        {
            catSortingOrderInside = -1;
        }
    }



    public void ToggleCatFlap()
    {
        catFlapClosed = !catFlapClosed;
    }
    public void PlayerInsideOrOutside()
    {
        if (playerInside)
        {
            playerSortingOrder = -1;
            vfxSortingOrder = 60;
            playerInside = false;
        }
        else
        {
            playerSortingOrder = 50;
            vfxSortingOrder = 5;
            playerInside = true;
        }
    }
}
