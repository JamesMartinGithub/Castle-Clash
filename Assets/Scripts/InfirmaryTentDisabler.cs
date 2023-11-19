using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfirmaryTentDisabler : MonoBehaviour
{
    public bool beingUsed = false;
    public GameObject placeableMarker;
    public bool tutorial = false;
    private void FixedUpdate() {
        // This ensures when a unit is healing in the infirmary, another unit can't be placed inside
        if (beingUsed) {
            if (placeableMarker.activeSelf == true) {
                if (tutorial) {
                    GameObject.Find("Tutorial Text").GetComponent<TutorialManager>().TIncreaseStage();
                    tutorial = false;
                }
                // The collider that allows units to be placed is disabled
                placeableMarker.SetActive(false);
            }
        }
        else {
            if (placeableMarker.activeSelf == false) {
                // The collider that allows units to be placed is enabled
                placeableMarker.SetActive(true);
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        beingUsed = true;
    }
    private void OnTriggerExit(Collider other) {
        beingUsed = false;
    }
}