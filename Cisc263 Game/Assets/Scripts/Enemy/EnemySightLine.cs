using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightLine : MonoBehaviour
{
    private EnemyAI me;

    private void Start()
    {
        me = gameObject.GetComponentInParent<EnemyAI>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // || "Player flashlight or whatever"
        {
            EventManager.Instance.PlayerSeen.Invoke(me);
        }
    }
}
