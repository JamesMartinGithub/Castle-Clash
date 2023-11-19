using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class On_Fire_Script : MonoBehaviour
{
    public GameObject fire_Effect;
    private float time;
    private bool on_Fire;
    private float counter = -1f;
    public Health_Script health_Script;
    private float damage;
    private void Start()
    {
        // The damage the fire should do and the length it should last is found
        damage = GameObject.Find("SetDamages").GetComponent<Set_Damages>().GetDamage("fire");
        time = GameObject.Find("SetDamages").GetComponent<Set_Damages>().GetOther("firetime");
    }
    public void SetOnFire() {
        if (on_Fire == false) {
            // The fire particle system is enabled and the unit is set on fire
            fire_Effect.SetActive(true);
            on_Fire = true;
            counter = 0.5f * 98;
            Invoke("Extinguish", time);
        }
    }
    private void Extinguish() {
        // The unit is set to not on fire after the given time
        // The fire particle system is disabled
        fire_Effect.SetActive(false);
        on_Fire = false;
    }
    private void FixedUpdate()
    {
        if (on_Fire) {
            if (counter > 0)
            {
                counter -= 1;
            }
            else {
                // Health is removed from the unit
                health_Script.Decrease_Health(damage);
                counter = 0.5f * 98;
            }
        } else if (counter != -1f) {
            counter = -1f;
        }
    }
}