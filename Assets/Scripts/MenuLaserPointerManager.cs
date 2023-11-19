using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;
public class MenuLaserPointerManager : MonoBehaviour
{
    public LineRenderer line;
    public Transform lineEndPos;
    private bool trigger_Pressed = false;
    public string[] scenes;
    private void Start()
    {
        // Setting both trigger methods to controller trigger events
        SteamVR_Actions.courseworkInput2_Trigger.AddOnStateDownListener(TriggerPressed, SteamVR_Input_Sources.RightHand);
        SteamVR_Actions.courseworkInput2_Trigger.AddOnStateUpListener(TriggerLetGo, SteamVR_Input_Sources.RightHand);
    }
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 80))
        {
            // Hit any collider
            if (hit.collider.gameObject.tag == "UICollider")
            {
                // Hit ui interactable
                if (lineEndPos.gameObject.activeSelf == false)
                {
                    lineEndPos.gameObject.SetActive(true);
                }
                if (!line.gameObject.activeSelf)
                {
                    line.gameObject.SetActive(true);
                }
                if (trigger_Pressed)
                {
                    // Determine what button is hit and load the corresponding scene
                    switch (hit.collider.gameObject.name)
                    {
                        case "Play":
                            SceneManager.LoadScene(scenes[0]);
                            break;
                        case "Learn":
                            SceneManager.LoadScene(scenes[1]);
                            break;
                    }
                }
                // The length of the laser is set to the distance to the point on the menu that was hit
                line.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, hit.point) / 15.57882f));
                lineEndPos.position = transform.TransformPoint(new Vector3(0, 0, Vector3.Distance(transform.position, hit.point) / 15.57882f));
            }
            else
            {
                // No ui interactable hit
                TurnOffLine();
            }
        }
        else
        {
            // No collider hit
            TurnOffLine();
        }
    }
    private void TurnOffLine() {
        // The line is set to its default length of 5 if no collider is hit by the raycast
        if (line.GetPosition(1) != new Vector3(0, 0, 5))
        {
            line.SetPosition(1, new Vector3(0, 0, 5));
        }
        if (lineEndPos.gameObject.activeSelf)
        {
            lineEndPos.gameObject.SetActive(false);
        }
        if (line.gameObject.activeSelf)
        {
            line.gameObject.SetActive(false);
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
