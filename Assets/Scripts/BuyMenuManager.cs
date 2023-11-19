using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BuyMenuManager : MonoBehaviour
{
    public GameObject menuObject;
    public Transform cameraObject;
    public LaserPointerManager laserManager;
    public GameObject laser;
    public GoldManager goldManager;
    private string status = "choosing"; // choosing or placing
    Dictionary<string, int> unitPrices = new Dictionary<string, int>();
    public GameObject placementGrid;
    private bool mustShowGrid = false;
    public bool tutorial = false;
    private void Start()
    {
        // These add functions to be called on the events: "menu button pressed" and "menu button released"
        SteamVR_Actions.courseworkInput2_MenuButton.AddOnStateDownListener(ButtonPressed, SteamVR_Input_Sources.LeftHand);
        SteamVR_Actions.courseworkInput2_MenuButton.AddOnStateDownListener(ButtonPressed, SteamVR_Input_Sources.RightHand);
        SteamVR_Actions.courseworkInput2_MenuButton.AddOnStateUpListener(ButtonLetGo, SteamVR_Input_Sources.LeftHand);
        SteamVR_Actions.courseworkInput2_MenuButton.AddOnStateUpListener(ButtonLetGo, SteamVR_Input_Sources.RightHand);
        // The prices of each unit are set in a dictionary
        unitPrices.Add("swordsman", 3);
        unitPrices.Add("axeman", 3);
        unitPrices.Add("longbowman", 4);
        unitPrices.Add("catapult", 12);
        unitPrices.Add("pikeman", 3);
        unitPrices.Add("torcher", 2);
        unitPrices.Add("crossbowman", 4);
        unitPrices.Add("bowturret", 12);
        placementGrid.SetActive(false);
        menuObject.SetActive(false);
    }
    public void TutorialStartCall() {
        if (tutorial) {
            // Starts the choosing process in the tutorial
            laserManager.laserMode = "choosing";
            laser.SetActive(true);
            laserManager.canBuy = true;
        }
    }
    public void ButtonPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (status == "choosing" && !tutorial)
        {
            // The menu button was pressed so the menu is shown
            menuObject.SetActive(true);
            transform.eulerAngles = new Vector3(0, cameraObject.eulerAngles.y, 0);
            transform.position = cameraObject.position;
            laserManager.laserMode = "choosing";
            laser.SetActive(true);
            laserManager.canBuy = true;
        }
    }
    public void ButtonLetGo(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (status == "choosing" && !tutorial)
        {
            // The menu button was released so the menu is hidden
            // Does not apply to Placing mode which can't be exited until the bought unit is placed
            laser.SetActive(false);
            menuObject.SetActive(false);
        }
    }
    public bool buyUnit(string chosenUnit) {
        // Check if the player has enough gold
        if (goldManager.playerGold >= unitPrices[chosenUnit])
        {
            laser.SetActive(true);
            menuObject.SetActive(false);
            // The laser is set to Placing mode so the unit can be placed on placeable ground
            status = "placing";
            goldManager.ReducePlayerGold(unitPrices[chosenUnit]);
            laserManager.laserMode = "placing";
            // The placement grid is shown
            placementGrid.SetActive(true);
            if (tutorial) {
                GameObject.Find("Tutorial Text").GetComponent<TutorialManager>().TIncreaseStage();
            }
            return true;
        }
        else {
            return false;
        }
    }
    public void placed() {
        // The unit has been placed on placeable ground
        laser.SetActive(false);
        // mustShowGrid acts as an override to stop this script hiding the placement grid if it needs to be shown for another purpose
        if (mustShowGrid == false) {
            // The placement grid is hidden
            placementGrid.SetActive(false);
        }
        // The laser is reset to Choosing mode so more units can be bought
        status = "choosing";
        if (tutorial) {
            GameObject.Find("Tutorial Text").GetComponent<TutorialManager>().TIncreaseStage();
        }
    }
    // This method chooses whether to show the placementGrid or not depending on the laser status and the mustShowGrid override
    public void ChangeMustShowGrid(bool value) {
        mustShowGrid = value;
        if (value == false && status == "choosing") {
            placementGrid.SetActive(false);
        }
        if (value == true)
        {
            placementGrid.SetActive(true);
        }
    }
}
