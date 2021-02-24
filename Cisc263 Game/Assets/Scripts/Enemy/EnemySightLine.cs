using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // || "Player flashlight or whatever"
        {
            Debug.Log("this works");
            EventManager.Instance.PlayerSeen.Invoke();
        }
    }
}
