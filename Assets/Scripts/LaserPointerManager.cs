using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class LaserPointerManager : MonoBehaviour
{
    public LineRenderer line;
    public Transform lineEndPos;
    public BuyMenuManager buyMenuManager;
    private bool trigger_Pressed = false;
    public bool canBuy = true;
    public string laserMode = "choosing"; // Choosing or placing or off
    private int choosingBitMask;
    public Transform lineEndDiamond;
    private int lastChosen;
    public GameObject[] unitPrefabs;
    private void Start()
    {
        // These add functions to be called on the events: "trigger pulled" and "trigger released"
        SteamVR_Actions.courseworkInput2_Trigger.AddOnStateDownListener(TriggerPressed, SteamVR_Input_Sources.RightHand);
        SteamVR_Actions.courseworkInput2_Trigger.AddOnStateUpListener(TriggerLetGo, SteamVR_Input_Sources.RightHand);
        // This integer represents both the two layers
        choosingBitMask = (1 << LayerMask.NameToLayer("UnitPlaceable")) | (1 << LayerMask.NameToLayer("UnitNonPlaceable"));
    }
    private void Update()
    {
        if (laserMode == "choosing")
        { // Choosing
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 80))
            {
                // Any collider is hit
                if (hit.collider.gameObject.tag == "UICollider")
                {
                    // Specifically a ui collider is hit
                    if (hit.collider.gameObject.name != "Back")
                    {
                        // Hit registered on buyable item
                        if (lineEndPos.gameObject.activeSelf == false)
                        {
                            lineEndPos.gameObject.SetActive(true);
                        }
                        if (trigger_Pressed && canBuy)
                        {
                            // The hit item is attempted to be bought
                            string chosenUnit = "";
                            switch (hit.collider.gameObject.name)
                            {
                                case "Button1":
                                    chosenUnit = "swordsman";
                                    lastChosen = 1;
                                    break;
                                case "Button2":
                                    chosenUnit = "axeman";
                                    lastChosen = 2;
                                    break;
                                case "Button3":
                                    chosenUnit = "longbowman";
                                    lastChosen = 3;
                                    break;
                                case "Button4":
                                    chosenUnit = "catapult";
                                    lastChosen = 4;
                                    break;
                                case "Button5":
                                    chosenUnit = "pikeman";
                                    lastChosen = 5;
                                    break;
                                case "Button6":
                                    chosenUnit = "torcher";
                                    lastChosen = 6;
                                    break;
                                case "Button7":
                                    chosenUnit = "crossbowman";
                                    lastChosen = 7;
                                    break;
                                case "Button8":
                                    chosenUnit = "bowturret";
                                    lastChosen = 8;
                                    break;
                            }
                            canBuy = false;
                            // The items price is sent to BuyMenuManager to be checked and bought if the gold is owned by the player
                            // If the item cannot be afforded, then the laser is still active to choose another
                            if (buyMenuManager.buyUnit(chosenUnit) == false)
                            {
                                canBuy = true;
                            }
                        }
                    }
                    else
                    {
                        // The back of the buy menu is hit
                        // The cursor is hidden
                        if (lineEndPos.gameObject.activeSelf)
                        {
                            lineEndPos.gameObject.SetActive(false);
                        }
                    }
                    // The length of the laser is set to the distance to the point on the menu that was hit
                    line.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, hit.point) / 15.57882f));
                    lineEndPos.position = transform.TransformPoint(new Vector3(0, 0, Vector3.Distance(transform.position, hit.point) / 15.57882f));
                }
                else
                {
                    // An unknown collider was hit
                    // The line length defaults to 5
                    if (line.GetPosition(1) != new Vector3(0, 0, 5))
                    {
                        line.SetPosition(1, new Vector3(0, 0, 5));
                    }
                    if (lineEndPos.gameObject.activeSelf)
                    {
                        lineEndPos.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                // No collider was hit
                // The line length defaults to 5
                if (line.GetPosition(1) != new Vector3(0, 0, 5))
                {
                    line.SetPosition(1, new Vector3(0, 0, 5));
                }
                if (lineEndPos.gameObject.activeSelf)
                {
                    lineEndPos.gameObject.SetActive(false);
                }
            }
        }
        else if (laserMode == "placing"){ // placing
            lineEndPos.gameObject.SetActive(false);
            RaycastHit hit;
            // This raycast is only in the specific two layers of "UnitPlaceable" and "UnitNonPlaceable" hence the bitmask
            if (Physics.Raycast(transform.position, transform.forward, out hit, 80, choosingBitMask))
            {
                // Any collider hit
                if (hit.collider.gameObject.name == "UnitPlaceable")
                {
                    // Pointing at placeable ground
                    if (lineEndDiamond.gameObject.activeSelf == false)
                    {
                        // The placeable ground indicator is shown
                        lineEndDiamond.gameObject.SetActive(true);
                    }
                    line.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, hit.point) / 15.57882f));
                    lineEndDiamond.position = transform.TransformPoint(new Vector3(0, 0, Vector3.Distance(transform.position, hit.point) / 15.57882f));
                    if (trigger_Pressed && laserMode != "off") {
                        // The item is placed on placeable ground
                        laserMode = "off";
                        lineEndDiamond.gameObject.SetActive(false);
                        Instantiate(unitPrefabs[--lastChosen], new Vector3(hit.point.x, 0, hit.point.z), Quaternion.Euler(new Vector3(0, 0, 0)));
                        buyMenuManager.placed();
                    }
                }
                else {
                    // Pointing at UI Mask
                    if (line.GetPosition(1) != new Vector3(0, 0, 0.4f))
                    {
                        line.SetPosition(1, new Vector3(0, 0, 0.4f));
                    }
                    if (lineEndDiamond.gameObject.activeSelf)
                    {
                        lineEndDiamond.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                // Not pointing at any ground element
                if (line.GetPosition(1) != new Vector3(0, 0, 0.4f))
                {
                    line.SetPosition(1, new Vector3(0, 0, 0.4f));
                }
                if (lineEndDiamond.gameObject.activeSelf)
                {
                    lineEndDiamond.gameObject.SetActive(false);
                }
            }
        }
        
    }
    public void TriggerPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        trigger_Pressed = true;
    }
    public void TriggerLetGo(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        trigger_Pressed = false;
    }
}
