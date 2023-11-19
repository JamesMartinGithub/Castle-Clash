using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torcher_Script : MonoBehaviour
{
    public GameObject fireearthPrefab;
    private void Start() {
        InvokeRepeating("SpawnFireearth", 5, 5);
    }
    private void SpawnFireearth() {
        // Each ground fire object is added to the array "spawns"
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Fireearth");
        bool close = false;
        // Each item in the array "spawns" is checked to see if its distance away from this torcher unit is further than 1.5
        foreach (GameObject spawn in spawns) {
            if (Vector3.Distance(spawn.transform.parent.position, transform.position) < 1.5f) {
                close = true;
            }
        }
        if (close == false) {
            // If another ground fire object is further than 1.5 away, a new one can be spawned here
            Instantiate(fireearthPrefab, transform.position, transform.rotation);
            
        }
    }
}