using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to hold values to be used by enemies
public class EnemyStats : Singleton<EnemyStats>
{
    public float roamSpeed;
    public float trackSpeed;
    public float attackSpeed;
    public GameObject player;
    public float cooldownTime;  // time in seconds to cool down tracking mode
    public float attackTime;  // time in seconds for enemy to attack player
    //Sound for tracking
    // [SerializeField] private AudioSource trackingSound;
    public float searchTime;  // time enemies will spend searching an area
}
