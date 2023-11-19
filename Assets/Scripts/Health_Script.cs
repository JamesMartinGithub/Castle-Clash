using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health_Script : MonoBehaviour
{
    public float starting_Health;
    protected float health;
    public GameObject ragdoll;
    public Image health_Bar;
    public virtual void Start() {
        health = starting_Health;
    }
    public void Increase_Health(float amount) {
        // The unit's health is only increased to the maximum starting value
        if ((health + amount) <= starting_Health)
        {
            health += amount;
        }
        else {
            health = starting_Health;
        }
        // The healthbar ui is updated to show the new health
        health_Bar.fillAmount = health / starting_Health;
    }
    // This method handles the loss of health
    public void Decrease_Health(float amount) {
        health -= amount;
        Check_Health();
        // The healthbar ui is updated to show the new health
        health_Bar.fillAmount = health / starting_Health;
    }
    // This method checks if the unit is dead
    virtual public void Check_Health() {
        if (health <= 0) {
            // The unit is removed and a ragdoll appears in its place
            Instantiate(ragdoll, transform.parent.position, transform.parent.rotation);
            Destroy(transform.parent.gameObject);
        }
    }
}