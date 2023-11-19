using UnityEngine;
using UnityEngine.UI;
public class HeldUnitScript : MonoBehaviour{
    public string heldUnit = "";
    private bool canPickup = true;
    public float resetTime;
    private float circleFloat;
    public Image circle;
    public void changeHeldUnit(string newString) {
        // A unit wants to be picked up
        if (canPickup)
        {
            // The unit is recorded as held and the re-hold timer starts
            heldUnit = newString;
            canPickup = false;
            circleFloat = resetTime;
            circle.gameObject.transform.parent.gameObject.SetActive(true);
            Invoke("ResetPickupState", resetTime);
        }
    }
    private void ResetPickupState() {
        canPickup = true;
        circle.gameObject.transform.parent.gameObject.SetActive(false);
    }
    private void Update() {
        if (canPickup == false) {
            circleFloat -= Time.deltaTime;
            // The circle timer ui is updated to show the re-hold timer's value
            circle.fillAmount = (circleFloat / resetTime);
        }
    }
}