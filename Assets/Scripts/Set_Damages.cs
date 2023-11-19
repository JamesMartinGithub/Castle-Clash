using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Damages : MonoBehaviour
{
    // This centralised script stores all the damages for all weapons and effects
    public float sword_Damage;
    public float axe_Damage;
    public float pike_Damage;
    public float torch_Damage;
    public float bow_Damage;
    public float crossbow_Damage;
    public float fire_Damage;
    public float burntearth_Damage;
    public float catapultturret_Damage;
    public float bowturret_Damage;
    [Space(10)]
    //public float grindstone_Damage_Multiplier;
    public float fire_Time;
    // This method returns the damage for the requested weapon
    public float GetDamage(string weapon) {
        switch (weapon) {
            case "sword":
                return sword_Damage;
            case "axe":
                return axe_Damage;
            case "pike":
                return pike_Damage;
            case "torch":
                return torch_Damage;
            case "bow":
                return bow_Damage;
            case "crossbow":
                return crossbow_Damage;
            case "fire":
                return fire_Damage;
            case "burntearth":
                return burntearth_Damage;
            case "catapultturret":
                return catapultturret_Damage;
            case "bowturret":
                return bowturret_Damage;
            default:
                return -1f;

        }
    }
    // This method returns the time fire should last for
    public float GetOther(string item)
    {
        switch (item)
        {
            case "firetime":
                return fire_Time;
            default:
                return -1f;

        }
    }
}