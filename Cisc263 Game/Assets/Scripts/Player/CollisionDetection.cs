using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if collided enemy and amulet not active
        if (other.CompareTag("Enemy") && !Amulet.Instance.isActive) 
        {
            // Debug.Log("player hit an enemy");
            // can add a condition here for health later if we want
            EventManager.Instance.JumpScare.Invoke();
            EventManager.Instance.Death.Invoke();
        }
        if (other.CompareTag("Flame"))
        {
            // deactivate flame
            other.gameObject.SetActive(false);
            // increase energy of lantern
            Lantern.Instance.IncreaseCurrentEnergy();            
        }

    }

}
