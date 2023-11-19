using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// x: (-7.4 -> +7.4)    z: (12.1 -> furthestEnemyPos    excluding -0.5 -> +0.5)
// W.M exclusion x: -4.3 -> +4.3   z: <8.2  

// Warmachines spawn in enemy half only, units spawn anywhere behind the furthest forward unit
// Units spawn to protect the base if friendly units are close to enemy base
// Units spawn close to friendly base when enemy units are close to it to rush
public class EnemyAI : MonoBehaviour
{
    private int targetGold = 99;
    private int targetIndex;
    public GameObject[] unitPrefabs;
    private Vector3 targetPosition;
    void Start() {
        ChooseNextUnit();
    }
    private void FixedUpdate() {
        // If the enemy has enough gold for the selected unit, it spawns the unit at the selected location
        // It then chooses another unit and location
        if (GameObject.Find("MenuPivot").GetComponent<GoldManager>().enemyGold >= targetGold) {
            Instantiate(unitPrefabs[targetIndex], targetPosition, transform.rotation);
            GameObject.Find("MenuPivot").GetComponent<GoldManager>().ReduceEnemyGold(targetGold);
            targetGold = 99;
            ChooseNextUnit();
        }
    }
    private void ChooseNextUnit() {
        int randInt1 = Random.Range(1,4);
        switch (randInt1) {
            case 1:
            case 2:
                // Footsoldier Chosen
                ChooseFootsoldier();
                ChoosePosition("Footsoldier");
                break;
            case 3:
                // Warmachine Chosen
                ChooseWarMachine();
                ChoosePosition("Warmachine");
                break;
        }
    }
    private void ChooseFootsoldier() {
        int randInt2 = Random.Range(1, 7);
        switch (randInt2) {
            case 1:
                // Swordsman Chosen
                targetGold = 3;
                targetIndex = 0;
                break;
            case 2:
                // Axeman Chosen
                targetGold = 3;
                targetIndex = 1;
                break;
            case 3:
                // Longbowman Chosen
                targetGold = 4;
                targetIndex = 2;
                break;
            case 4:
                // Pikeman Chosen
                targetGold = 3;
                targetIndex = 4;
                break;
            case 5:
                // Torcher Chosen
                targetGold = 2;
                targetIndex = 5;
                break;
            case 6:
                // Crossbowman Chosen
                targetGold = 4;
                targetIndex = 6;
                break;
        }
    }
    private void ChooseWarMachine() {
        int randInt2 = Random.Range(1, 3);
        switch (randInt2) {
            case 1:
                // Catapult Chosen
                targetGold = 12;
                targetIndex = 3;
                break;
            case 2:
                // Ballista Chosen
                targetGold = 12;
                targetIndex = 7;
                break;
        }
    }
    private void ChoosePosition(string unitType) {
        float furthestDistance = 12.1f;
        // Gets position of furthest forward enemy unit
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("EnemyMarker"))
        {
            if (((unit.transform.parent.position.z) < furthestDistance) && unit.transform.parent.position.y == 0)
            {
                furthestDistance = unit.transform.parent.position.z + 1.5f;
            }
        }
        float randX = new float();
        float randZ = new float();
        // Positions are chosen based on the unit type
        switch (unitType) {
            case "Footsoldier":
                randX = Random.Range(-7.4f, 7.4f);
                randZ = 0f;
                if (furthestDistance <= -4f) {
                    // Enemy rush mode - units should attack friendly base
                    randZ = furthestDistance;
                }
                else {
                    // Standard unit spawning
                    randZ = Random.Range(furthestDistance, 12.1f);
                }
                if (randZ > -0.5f && randZ < 0.5f) {
                    // Chosen position ontop of the fence, so a safe position is set
                    randZ = 0.5f;
                }
                float furthestEnemyPosZ = GameObject.Find("PlaceableEndPos").transform.position.z;
                if (furthestEnemyPosZ > 4f) { 
                    // Friendly rush mode - units should protect enemy base
                    // This takes priority over rushing the friendly player
                    float tempFurthestFriendlyZ = 13;
                    float tempFurthestFriendlyX = -7.4f;
                    foreach (GameObject unit in GameObject.FindGameObjectsWithTag("FriendlyMarker")) {
                        if (((unit.transform.parent.position.z) > tempFurthestFriendlyZ) && unit.transform.parent.position.y == 0) {
                            tempFurthestFriendlyZ = unit.transform.parent.position.z;
                            tempFurthestFriendlyX = unit.transform.parent.position.x;
                        }
                    }
                    // This sets the next unit spawn so that it attacks the closest friendly attacking unit
                    if (tempFurthestFriendlyX > 0f) {
                        randX = 1f;
                    }
                    if (tempFurthestFriendlyX == 0f) {
                        randX = 0f;
                    }
                    if (tempFurthestFriendlyX < 0f)
                    {
                        randX = -1f;
                    }
                    randZ = 12.1f;
                }
                targetPosition = new Vector3(randX, 0, randZ);
                break;
            case "Warmachine":
                randX = Random.Range(-7.4f, 7.4f);
                randZ = Random.Range(2f, 12.1f);
                // This check prevents enemy warmachines spawning in the corners where they would do nothing
                if (randZ > 8.2) {
                    randX = Random.Range(-4.3f, 4.3f);
                }
                targetPosition = new Vector3(randX, 0, randZ);
                break;
        }
    }
}