using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TutorialManager : MonoBehaviour
{
    public int stage = 1; // A = Welcome  B = Buying/Placing & Winning
    //
    public GameObject enemyUnitPrefab;
    public GameObject infirmaryPlacementGrid;
    public GameObject controllerTrigger;
    public GameObject friendlySwordsman;
    public GameObject friendlyDummy;
    public GameObject placementGrid;
    //
    public GameObject A1; // Welcome Text
    public GameObject A2; // What we will be going through
    public GameObject A3; // Bases
    public GameObject A4; // Buy menu
    public GameObject A5; // Unit movement
    public GameObject A6; // Infirmary tent
    //
    public GameObject B1; // Buying intro
    public GameObject B2; // Buy swordsman (Menu visible)
    public GameObject B3; // Place swordsman (Placement grid visible)
    public GameObject B4; // Move unit into infirmary
    public GameObject B5; // Move unit back onto battlefield
    public GameObject B6; // Prepare to win
    //
    public bool waitingForPlace = false;
    private void Start() {
        stage = 1;
        TManager();
    }
    // This method shows new instructions each stage and enables/disables objects in game to facilitate this
    private void TManager() {
        switch (stage) {
            // A
            case 1:
                A1.SetActive(true);
                StartCoroutine(TSwitcher(5, A1, A2));
                break;
            case 2:
                StartCoroutine(TSwitcher(8, A2, A3));
                break;
            case 3:
                StartCoroutine(TSwitcher(8, A3, A4));
                break;
            case 4:
                StartCoroutine(TSwitcher(8, A4, A5));
                break;
            case 5:
                StartCoroutine(TSwitcher(5, A5, A6));
                break;
            case 6:
                StartCoroutine(TSwitcher(8, A6, B1));
                break;
            // B
            case 7:
                StartCoroutine(TSwitcher(4, B1, B2));
                break;
            case 8:
                // Menu just opened
                GameObject.Find("MenuPivot").GetComponent<BuyMenuManager>().TutorialStartCall();
                // Waiting for buy confirmation
                break;
            case 9:
                // Swordsman bought
                B2.SetActive(false);
                B3.SetActive(true);
                // Waiting for placement confirmation
                break;
            case 10:
                // Placed
                // enemy unit placed and the friendly units attacks that
                Instantiate(enemyUnitPrefab, new Vector3(-3.6f, 0, 3.2f), Quaternion.Euler(Vector3.forward));
                B3.SetActive(false);
                StartCoroutine(TSwitcher(13, B3, B4));
                break;
            case 11:
                // Unit frozen, player places into infirmary tent
                Vector3 tempPos = GameObject.Find("Friendly Knight with Sword(Clone)").transform.position;
                Quaternion tempRot = GameObject.Find("Friendly Knight with Sword(Clone)").transform.rotation;
                Destroy(GameObject.Find("Friendly Knight with Sword(Clone)"));
                Instantiate(friendlyDummy, tempPos, tempRot);
                infirmaryPlacementGrid.SetActive(true);
                controllerTrigger.SetActive(true);
                break;
            case 12:
                // Unit in infirmary tent
                // Waiting for taken out and placed confirmation
                infirmaryPlacementGrid.SetActive(false);
                B4.SetActive(false);
                B5.SetActive(true);
                waitingForPlace = true;
                placementGrid.SetActive(true);
                break;
            case 13:
                // Unit placed and moving towards base
                placementGrid.SetActive(false);
                controllerTrigger.SetActive(false);
                waitingForPlace = false;
                Vector3 tempPos2 = GameObject.Find("Friendly Knight with Sword TUTORIAL DUMMY(Clone)").transform.position;
                Quaternion tempRot2 = GameObject.Find("Friendly Knight with Sword TUTORIAL DUMMY(Clone)").transform.rotation;
                Destroy(GameObject.Find("Friendly Knight with Sword TUTORIAL DUMMY(Clone)"));
                Instantiate(friendlySwordsman, tempPos2, tempRot2);
                B5.SetActive(false);
                B6.SetActive(true);
                break;
        }
    }
    // This coroutine switches the state of two gameobjects then moves the stage forward after a specified time
    private IEnumerator TSwitcher(float time, GameObject obj1, GameObject obj2) {
        yield return new WaitForSeconds(time);
        obj1.SetActive(false);
        obj2.SetActive(true);
        stage += 1;
        TManager();
    }
    // This method is for external calls
    public void TIncreaseStage() {
        stage += 1;
        TManager();
    }
}