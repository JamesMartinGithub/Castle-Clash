using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Patch_Script : MonoBehaviour
{
    private void Start() {
        Destroy(transform.parent.gameObject, 8);
    }
    private void OnTriggerEnter(Collider collider) {
        // A trigger has entered this collider
        if (collider.gameObject.name == "Unit_Hitbox" && collider.gameObject.tag != gameObject.tag) {
            // The trigger was part of an enemy unit
            try {
                if (collider.gameObject.transform.parent.gameObject.GetComponentInChildren<On_Fire_Script>() != null) {
                    // If a fire script is found on a unit, its method SetOnFire is called to set the uniy on fire
                    collider.gameObject.transform.parent.gameObject.GetComponentInChildren<On_Fire_Script>().SetOnFire();
                }
            }
            catch (System.Exception) {
                // The try-except prevents an error occuring if no fire script is found
                throw;
            }
        }
    }
}