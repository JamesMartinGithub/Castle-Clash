using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_After_Time : MonoBehaviour
{
    public float time;
    void Start() {
        // This object is destroyed after a given time
        Destroy(gameObject, time);
    }

}