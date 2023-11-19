using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Script : MonoBehaviour
{
    public Unit_Script unit_Script;
    protected GameObject target_Unit;
    protected GameObject target_Unit_2;
    protected float counter = -1;
    public float time_To_Attack;
    public float time_Between_Attacks;
    protected float damage;
    protected bool attacking = false;
    protected int multiplying_Constant = 98;
    public string weapon_Type;
    protected void Start() {
        // The damage this weapon should do is retrieved
        damage = GameObject.Find("SetDamages").GetComponent<Set_Damages>().GetDamage(weapon_Type);
    }
    protected void OnTriggerEnter(Collider collider) {
        // Any trigger is entered
        if (collider.gameObject.name == "Unit_Hitbox" && collider.gameObject.tag != gameObject.tag && target_Unit == null) {
            // An enemy hitbox was entered so the unit begins to attack
            target_Unit = collider.gameObject;
            counter = time_To_Attack * multiplying_Constant;
            unit_Script.ChangeAttackingState(true);
            attacking = true;
            StartCoroutine(unit_Script.FaceAttacker(collider.transform.parent.transform));
        }
    }
    protected void OnTriggerExit(Collider collider) {
        // A trigger has left this collider
        if (target_Unit == collider.gameObject) {
            // The target has left or died
            counter = -1;
            target_Unit = null;
            unit_Script.ChangeAttackingState(false);
            // The unit stops attacking
            attacking = false;
            CheckForOtherEnemies();
        }
        if (target_Unit_2 == collider.gameObject) {
            target_Unit_2 = null;
        }
    }
    protected void OnTriggerStay(Collider collider) {
        if (collider.gameObject.name == "Unit_Hitbox" && collider.gameObject.tag != gameObject.tag && target_Unit != collider.gameObject) {
            // A 2nd enemy hitbox is within attack range
            target_Unit_2 = collider.gameObject;
        }
        else {
            target_Unit_2 = null;
        }
    }
    protected virtual void FixedUpdate() {
        if (target_Unit != null) {
            // The unit has an attack target so attacks at a given rate and decreases the health of the enemy unit
            if (counter <= 0) {
                counter = time_Between_Attacks * multiplying_Constant;
                // Do damage to the enemy object or base
                if (target_Unit.GetComponent<Health_Script>() != null)
                {
                    target_Unit.GetComponent<Health_Script>().Decrease_Health(damage);
                }
                else {
                    if (target_Unit.GetComponent<Base_Health_Script>() != null) {
                        target_Unit.GetComponent<Base_Health_Script>().Decrease_Health(damage);
                    }
                }
            } else {
                counter -= 1;
            }
            // This is a check to ensure the character stops attacking if the enemy dies, since OnTriggerExit() is not called when a collider is destroyed
        } else if (attacking) {
            counter = -1;
            target_Unit = null;
            unit_Script.ChangeAttackingState(false);
            attacking = false;
            CheckForOtherEnemies();
        }
    }
    protected void CheckForOtherEnemies()
    {
        if (target_Unit_2 != null)
        {
            // This switches the attack target to the 2nd enemy unit within attack range
            target_Unit = target_Unit_2;
            counter = time_To_Attack * multiplying_Constant;
            unit_Script.ChangeAttackingState(true);
            attacking = true;
            StartCoroutine(unit_Script.FaceAttacker(target_Unit_2.transform.parent.transform));
        }
    }
}