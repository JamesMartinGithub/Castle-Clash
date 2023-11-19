using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Unit_Script))]
public class Grabbable : MonoBehaviour
{
    private bool touching_Controller = false;
    private bool trigger_Pressed = false;
    private GameObject holding_Controller;
    private Quaternion locked_Rotation;
    private Rigidbody rigid;
    private bool picked_Up = false;
    private Unit_Script unit_Script;
    public bool friendly = true;
    private string identifier;
    private HeldUnitScript heldUnitScript;
    public bool tutorial = false;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Controller_Trigger") {
            // When the controller is over the unit
            touching_Controller = true;
            holding_Controller = collider.gameObject;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Controller_Trigger") {
            // When the controller is no longer over the unit
            touching_Controller = false;
            holding_Controller = null;
        }
    }
    private void Start()
    {
        if (friendly)
        {
            identifier = transform.position.x.ToString() + transform.position.y.ToString() + transform.position.z.ToString();
            heldUnitScript = GameObject.Find("[CameraRig]").GetComponent<HeldUnitScript>();
            // These add functions to be called on the events: "trigger pulled" and "trigger released"
            SteamVR_Actions.courseworkInput2_Trigger.AddOnStateDownListener(TriggerPressed, SteamVR_Input_Sources.LeftHand);
            SteamVR_Actions.courseworkInput2_Trigger.AddOnStateDownListener(TriggerPressed, SteamVR_Input_Sources.RightHand);
            SteamVR_Actions.courseworkInput2_Trigger.AddOnStateUpListener(TriggerLetGo, SteamVR_Input_Sources.LeftHand);
            SteamVR_Actions.courseworkInput2_Trigger.AddOnStateUpListener(TriggerLetGo, SteamVR_Input_Sources.RightHand);
            // The initial rotation is recorded
            locked_Rotation = transform.rotation;
            rigid = GetComponent<Rigidbody>();
            unit_Script = GetComponent<Unit_Script>();
        }
    }
    private void FixedUpdate()
    {
        if (friendly)
        {
            // Units are only pickup-able when friendly
            if (touching_Controller && trigger_Pressed && heldUnitScript.heldUnit == "") {
                if (!tutorial) {
                    // This tells heldUnitScript what unit wants to be held
                    heldUnitScript.changeHeldUnit(identifier);
                }
                else {
                    heldUnitScript.heldUnit = identifier;
                }

            }
            if (trigger_Pressed && heldUnitScript.heldUnit == identifier) {
                // The unit will change position to match the controller's movement, but won't match rotation
                // The rotation is instead set to its known initial rotation
                transform.rotation = locked_Rotation;
                // When picked up, this script performs a one-time method call to Unit_Script to send its picked-up state
                if (!picked_Up) {
                    rigid.isKinematic = true;
                    rigid.useGravity = false;
                    transform.parent = holding_Controller.transform;
                    picked_Up = true;
                    if (!tutorial) {
                        unit_Script.ChangePickupState(true);
                        GameObject.Find("MenuPivot").GetComponent<BuyMenuManager>().ChangeMustShowGrid(true);
                    }
                }
            }
            else {
                // The unit will no longer follow the controller, so is unparented
                transform.parent = null;
                if (transform.position.y > 0) {
                    // If in the air, it should fall under gravity
                    transform.rotation = locked_Rotation;
                    rigid.isKinematic = false;
                    rigid.useGravity = true;
                }
                else if (transform.position.y < 0) {
                    // When it has fallen far enough to the ground, it stops being affected by gravity
                    transform.rotation = locked_Rotation;
                    rigid.isKinematic = true;
                    rigid.useGravity = false;
                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                }
                // When let go, this script performs a one-time method call to Unit_Script to send its picked-up state
                if (picked_Up) {
                    picked_Up = false;
                    if (!tutorial) {
                        unit_Script.ChangePickupState(false);
                    }
                    float largestZ = GameObject.Find("PlaceableEndPos").transform.position.z;
                    float smallestZ = -12f;
                    float largestX = 7.4f;
                    float smallestX = -7.4f;
                    // These conditional statements ensure the unit is in bounds when dropped
                    // If out of bounds, its position is set to the closest location in bounds
                    if (transform.position.z < smallestZ && !(transform.position.z < -13 && transform.position.z > -14.5f && transform.position.x < -1.4f && 
                        transform.position.x > -3.4f && GameObject.Find("InfirmaryTentCollider").GetComponent<InfirmaryTentDisabler>().beingUsed == false)) {
                        transform.position = new Vector3(transform.position.x, transform.position.y, smallestZ);
                    }
                    if (transform.position.z > largestZ) {
                        transform.position = new Vector3(transform.position.x, transform.position.y, largestZ);
                    }
                    if (transform.position.x < smallestX) {
                        transform.position = new Vector3(smallestX, transform.position.y, transform.position.z);
                    }
                    if (transform.position.x > largestX) {
                        transform.position = new Vector3(largestX, transform.position.y, transform.position.z);
                    }
                    heldUnitScript.heldUnit = "";
                    if (!tutorial) {
                        GameObject.Find("MenuPivot").GetComponent<BuyMenuManager>().ChangeMustShowGrid(false);
                    }
                }
            }
        }
    }
    public void TriggerPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        trigger_Pressed = true;
    }
    public void TriggerLetGo(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        trigger_Pressed = false;
    }
}