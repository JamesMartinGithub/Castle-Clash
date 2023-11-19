using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DummyScript : MonoBehaviour
{
    private bool changedUI = false;
    public Image img;
    private void FixedUpdate() {
        if (GameObject.Find("Tutorial Text").GetComponent<TutorialManager>().waitingForPlace) {
            // The tutorial manager is waiting for this unit to be placed on the ground outside the infirmary
            if (changedUI == false) {
                changedUI = true;
                img.fillAmount = 1;
            }
            if (transform.position.y == 0 && transform.position.z >= -10) {
                // The unit has been placed on the ground outside the infirmary
                // The tutorial manager is notified so the next stage can run
                GameObject.Find("Tutorial Text").GetComponent<TutorialManager>().waitingForPlace = false;
                GameObject.Find("Tutorial Text").GetComponent<TutorialManager>().TIncreaseStage();
            }
        }
    }
}