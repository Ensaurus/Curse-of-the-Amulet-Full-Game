using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] private AudioSource DeathSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if collided enemy and amulet not active
        if (other.CompareTag("Enemy") && !Amulet.Instance.isActive) 
        {
            // Debug.Log("player hit an enemy");
            // can add a condition here for health later if we want
            DeathSound.Play();
            EventManager.Instance.JumpScare.Invoke();
            EventManager.Instance.Death.Invoke();
        }
        // collided with flame pickup for lantern
        if (other.CompareTag("Flame"))
        {
            // deactivate flame
            other.gameObject.SetActive(false);
            // increase energy of lantern
            Lantern.Instance.IncreaseCurrentEnergy();            
        }
        // collided with exit
        if (other.CompareTag("Exit"))
        {
            if (Amulet.Instance.charge >= Exit.Instance.requiredEnergy)
            {
                EventManager.Instance.LevelCompleted.Invoke();
            }
        }
    }

}
