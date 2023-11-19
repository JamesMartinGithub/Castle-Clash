using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Unit_Script : MonoBehaviour
{
    public bool friendly;
    protected bool picked_Up;
    protected bool healing;
    protected bool attacking;
    protected string movement_State = "Walking"; // "Walking", "Idle", "Grabbed" or "Attacking"
    public float movement_Speed;
    public new Animation animation;
    private Vector3 savedGrabPosition;
    private Health_Script healthScript;
    virtual public void Start() {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        ChangeAnimation();
        healthScript = GetComponentInChildren<Health_Script>();
        InvokeRepeating("HealingCheck", 0.3f, 0.3f);
    }
    virtual public void FixedUpdate()
    {
        // movement_State is set according to the units actions and the animations are changed accordingly
        if (picked_Up || transform.position.y > 0)
        {
            if (movement_State != "Grabbed")
            {
                movement_State = "Grabbed";
                ChangeAnimation();
                // Ensure the unit doesn't go out of placeable bounds
                savedGrabPosition = gameObject.transform.localPosition;
            }
        }
        else if (healing)
        {
            if (movement_State != "Idle")
            {
                movement_State = "Idle";
                ChangeAnimation();
            }
        }
        else if (attacking)
        {
            if (movement_State != "Attacking")
            {
                movement_State = "Attacking";
                ChangeAnimation();
            }
        }
        else {
            if (movement_State != "Walking")
            {
                movement_State = "Walking";
                ChangeAnimation();
            }
        }
        if (movement_State == "Walking" && transform.position.y == 0) {
            // The unit is walking on the ground
            FaceTarget();
            transform.Translate(transform.forward * movement_Speed, Space.World);
        }
        if (transform.position.z < -13 && transform.position.z > -14.5f && transform.position.x < -1.4f && transform.position.x > -3.4f && transform.position.y == 0) {
            // The unit is in the infirmary tent
            if (healing == false) {
                healing = true;
                transform.position = new Vector3(-2.422f, 0, -13.81f);
            }
        }
        else {
            if (healing) {
                healing = false;
            }
        }
    }
    // This method makes the unit face the location it is walking towards
    public void FaceTarget() {
        Transform target = null;
        if (friendly)
        {
            // Decides whether to go to the closest gap or enemy base depending on how far forward it is
            if (transform.position.z >= 0)
            {
                target = GameObject.Find("Base_Enemy").transform;
            }
            else
            {
                target = ClosestFenceGap();
            }
        }
        else {
            if (transform.position.z <= 0)
            {
                target = GameObject.Find("Base_Friendly").transform;
            }
            else
            {
                target = ClosestFenceGap();
            }
        }
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    // This method finds the closest gap in the fence out of the 3 in the middle
    public Transform ClosestFenceGap() {
        Transform closest_Transform = GameObject.Find("Fence_Middle").transform;
        // The 3 potential distances are calculated
        float smallest_Distance = Vector3.Distance(transform.position, GameObject.Find("Fence_Middle").transform.position);
        float left_Route = Vector3.Distance(transform.position, GameObject.Find("Fence_Left").transform.position);
        float right_Route = Vector3.Distance(transform.position, GameObject.Find("Fence_Right").transform.position);
        // The distances are compared
        if (left_Route < smallest_Distance) {
            smallest_Distance = left_Route;
            closest_Transform = GameObject.Find("Fence_Left").transform;
        }
        if (right_Route < smallest_Distance)
        {
            smallest_Distance = right_Route;
            closest_Transform = GameObject.Find("Fence_Right").transform;
        }
        // The closest distance is returned
        return closest_Transform;
    }
    // Setter method to change picked_Up value from external class
    public void ChangePickupState(bool pickedUp) {
        picked_Up = pickedUp;
    }
    // Setter method to change attacking value from external class
    public void ChangeAttackingState(bool beingAttacked)
    {
        attacking = beingAttacked;
    }
    // This method handles the proper blending and changing of animations to prevent unwanted poses
    public void ChangeAnimation() {
        switch (movement_State) {
            case "Walking":
                StartCoroutine(PlayAnimation("Unit_Walk"));
                break;
            case "Idle":
                animation.Play("Unit_Idle");
                break;
            case "Grabbed":
                StartCoroutine(PlayAnimation("Unit_Pickup"));
                break;
            case "Attacking":
                foreach (AnimationState clip in animation) {
                    if (clip.name.Contains("Unit_Attack")) {
                        StartCoroutine(PlayAnimation(clip.name));
                    }
                }
                break;
        }
    }
    // This function plays the idle animation for 0.01s then plays the intended animation, so that body parts not addressed in the intended animation are reset
    // and don't keep the same state as in the previous animation
    public IEnumerator PlayAnimation(string clip) {
        animation.Play("Unit_Idle");
        yield return new WaitForSeconds(0.01f);
        if (healing == false)
        {
            print("PLAYING: " + clip);
            animation.Play(clip);
        }
        else {
            print("PLAYING: " + "Unit_Idle");
            animation.Play("Unit_Idle");
        }

    }
    // This method makes the unit face the enemy its attacking, but ensures its x and z rotation remains 0
    public IEnumerator FaceAttacker(Transform attacker) {
        yield return new WaitForSeconds(0.01f);
        transform.LookAt(attacker);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    // This method increases the health of the unit when its in the infirmary
    private void HealingCheck() {
        if (healing) {
            healthScript.Increase_Health(10);
        }
    }
}