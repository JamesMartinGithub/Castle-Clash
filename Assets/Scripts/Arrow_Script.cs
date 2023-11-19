using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Script : MonoBehaviour
{
    public float cycles_Left;
    private float current_Cycle;
    public Transform target;
    private Vector3 target_Position;
    private Vector3 start_Position;
    private void Start()
    {
        start_Position = transform.position;
        target_Position = new Vector3(target.position.x, target.position.y + 0.5f, target.position.z);
        current_Cycle = cycles_Left;
    }
    private void FixedUpdate()
    {
        // The arrow lerps its position between the bow it was fired from to the enemy unit, based on a given time value
        transform.position = Vector3.Lerp(start_Position, target_Position, 1 - (current_Cycle / cycles_Left));
        if (current_Cycle > 0)
        {
            current_Cycle -= 1;
        }
        else {
            // When the given time has passed, the arrow is destroyed
            Destroy(gameObject);
        }
    }
}