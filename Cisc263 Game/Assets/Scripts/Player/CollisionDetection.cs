using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] private AudioSource DeathSound;

    public GameObject chargingStat;

    [SerializeField] private AudioSource chargingSound; 

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
            if(Lantern.Instance.currentEnergy < Lantern.Instance.maxEnergy)
            {
                // increase energy of lantern
                Lantern.Instance.IncreaseCurrentEnergy();
            }
        }
        // collided with charging station for charging amulet
        if (other.CompareTag("ChargingStation"))
        {
            if(Amulet.Instance.charge < Amulet.Instance.maxCharge)
            {
                Amulet.Instance.isCharging = true;
                chargingSound.Play();
            }
        }
        // collided with exit
        if (other.CompareTag("Exit"))
        {
            int required = Exit.Instance.requiredEnergy;
            if (Amulet.Instance.charge >= required)
            {
                EventManager.Instance.AttemptedExitWithEnoughCharge.Invoke();   
            }
            else
            {
                EventManager.Instance.FailedPortalEntry.Invoke(required);
            }
        }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            // collided with charging station for charging amulet
            if (other.CompareTag("ChargingStation"))
            {
                ChargingStationScript chargingStatScript = other.GetComponent<ChargingStationScript>();
                if(chargingStatScript.chargingState)
                {
                    Amulet.Instance.IncreaseAmuletCharge();
                }
                else
                {
                    Amulet.Instance.isCharging = false;
                    chargingSound.Stop();
                }
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("ChargingStation") && Amulet.Instance.charge < Amulet.Instance.maxCharge)
            {
                Amulet.Instance.isCharging = false;
                chargingSound.Stop();
            }
        }


        public void PortalTaken()
        {
            Amulet.Instance.charge -= Exit.Instance.requiredEnergy; // needs to be called before LevelCompleted because LevelCompleted will raise requiredCharge
            EventManager.Instance.LevelCompleted.Invoke();
            UIManager.Instance.updateAmuletCharge();
        }
        public void PortalNotTaken()
        {
            EventManager.Instance.PortalNotTaken.Invoke();
        }
    }



