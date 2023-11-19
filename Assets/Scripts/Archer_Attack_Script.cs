using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Archer_Attack_Script : Attack_Script
{
    private bool arrow_Fired;
    public GameObject arrow_Prefab;
    public Transform arrow_Position;
    public float spawn_Speed_Modifier;
    new private void Start()
    {
        // Base Monobehaviour functions must be called in derived classes to function and can't just be inherited
        base.Start();
    }
    protected override void FixedUpdate()
    {
        if (target_Unit != null)
        {
            // The unit has a target and attacks the targeted enemy unit
            if (counter < ((time_Between_Attacks * multiplying_Constant) * spawn_Speed_Modifier))
            {
                if (arrow_Fired == false) {
                    // An arrow is instantiated to travel towards the enemy unit
                    GameObject a = Instantiate(arrow_Prefab, arrow_Position.transform.position, arrow_Position.transform.rotation);
                    Arrow_Script script = a.GetComponent<Arrow_Script>();
                    script.cycles_Left = counter;
                    script.target = target_Unit.transform;
                    arrow_Fired = true;
                }
            }
            if (counter <= 0)
            {
                arrow_Fired = false;
                counter = time_Between_Attacks * multiplying_Constant;
                // Do damage to the enemy object or base
                if (target_Unit.GetComponent<Health_Script>() != null)
                {
                    target_Unit.GetComponent<Health_Script>().Decrease_Health(damage);
                }
                else
                {
                    if (target_Unit.GetComponent<Base_Health_Script>() != null)
                    {
                        target_Unit.GetComponent<Base_Health_Script>().Decrease_Health(damage);
                    }
                }
            }
            else
            {
                counter -= 1;
            }
            // This is a check to ensure the character stops attacking if the enemy dies, since OnTriggerExit() is not called when a collider is destroyed
        }
        else if (attacking)
        {
            // The unit has no target
            counter = -1;
            target_Unit = null;
            unit_Script.ChangeAttackingState(false);
            attacking = false;
            arrow_Fired = false;
        }
    }
}