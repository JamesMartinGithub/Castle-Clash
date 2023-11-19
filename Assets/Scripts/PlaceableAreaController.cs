using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableAreaController : MonoBehaviour
{
    public Transform targetPos;
    public Transform trackingObject;
    private void FixedUpdate()
    {
        float furthestDistance = 13;
        // The furthest forward friendly unit is found by checking the z position of each friendly unit
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("FriendlyMarker")) {
            if (((unit.transform.parent.position.z + 15) > furthestDistance) && unit.transform.parent.position.y == 0) {
                furthestDistance = unit.transform.parent.position.z + 15;
            }
        }
        transform.localPosition = new Vector3(0, 0, furthestDistance + 1);
        trackingObject.position = targetPos.position;
    }
}