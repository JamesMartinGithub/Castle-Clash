using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Attack_Script : MonoBehaviour
{
    private GameObject target_Unit;
    private GameObject target_Unit_2;
    private float counter = -1;
    public float time_To_Attack;
    public float time_Between_Attacks;
    private float damage;
    private bool attacking = false;
    private int multiplying_Constant = 97;
    public string weapon_Type;
    public Animation anim;
    private void Start() {
        // The damage this weapon should do is retrieved
        damage = GameObject.Find("SetDamages").GetComponent<Set_Damages>().GetDamage(weapon_Type);
    }
    private void OnTriggerEnter(Collider collider) {
        // Any trigger is entered
        if (collider.gameObject.name == "Unit_Hitbox" && collider.gameObject.tag != gameObject.tag && target_Unit == null) {
            // An enemy hitbox was entered so the unit begins to attack
            target_Unit = collider.gameObject;
            counter = time_To_Attack * multiplying_Constant;
            attacking = true;
            if (weapon_Type == "catapultturret") {
                anim.Play("Catapult_Attack");
            } else if (weapon_Type == "bowturret") {
                anim.Play("Bowturret_Attack");
            }
        }
    }
    private void OnTriggerExit(Collider collider) {
        // A trigger has left this collider
        if (target_Unit == collider.gameObject) {
            // The target has left or died
            counter = -1;
            target_Unit = null;
            // The unit stops attacking
            attacking = false;
            // The animation is set to idle
            if (weapon_Type == "catapultturret") {
                anim.Play("Catapult_Idle");
            }
            else if (weapon_Type == "bowturret") {
                anim.Play("Bowturret_Idle");
            }
            CheckForOtherEnemies();
        }
        if (target_Unit_2 == collider.gameObject) {
            target_Unit_2 = null;
        }
    }
    private void OnTriggerStay(Collider collider) {
        if (collider.gameObject.name == "Unit_Hitbox" && collider.gameObject.tag != gameObject.tag && target_Unit != collider.gameObject) {
            // A 2nd enemy hitbox is within attack range
            target_Unit_2 = collider.gameObject;
        }
        else {
            target_Unit_2 = null;
        }
    }
    private void FixedUpdate() {
        if (target_Unit != null) {
            // The unit has an attack target so attacks at a given rate and decreases the health of the enemy unit
            transform.parent.transform.LookAt(target_Unit.transform.parent.transform);
            transform.parent.transform.eulerAngles = new Vector3(0, transform.parent.transform.eulerAngles.y, 0);
            if (counter <= 0) {
                counter = time_Between_Attacks * multiplying_Constant;
                // Do damage to the enemy object or base
                if (target_Unit.GetComponent<Health_Script>() != null) {
                    target_Unit.GetComponent<Health_Script>().Decrease_Health(damage);
                }
                else {
                    if (target_Unit.GetComponent<Base_Health_Script>() != null) {
                        target_Unit.GetComponent<Base_Health_Script>().Decrease_Health(damage);
                    }
                }
            }
            else {
                counter -= 1;
            }
        }
        else if (attacking) {
            // This is a check to ensure the character stops attacking if the enemy dies, since OnTriggerExit() is not called when a collider is destroyed
            counter = -1;
            target_Unit = null;
            attacking = false;
            if (weapon_Type == "catapultturret") {
                anim.Play("Catapult_Idle");
            }
            else if (weapon_Type == "bowturret") {
                anim.Play("Bowturret_Idle");
            }
            CheckForOtherEnemies();
        }
    }
    private void CheckForOtherEnemies() {
        if (target_Unit_2 != null) {
            // This switches the attack target to the 2nd enemy unit within attack range
            target_Unit = target_Unit_2;
            counter = time_To_Attack * multiplying_Constant;
            attacking = true;
            if (weapon_Type == "catapultturret") {
                anim.Play("Catapult_Attack");
            }
            else if (weapon_Type == "bowturret") {
                anim.Play("Bowturret_Attack");
            }
        }
    }
}