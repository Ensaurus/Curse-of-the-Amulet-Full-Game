using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingStationScript : MonoBehaviour
{
    public float maxChargeTime;
    public float chargeTimeLeft;
    public float maxCoolDown;
    public float coolDown;
    public bool chargingState;

    public bool inChargeCircle;

    // Start is called before the first frame update
    void Start()
    {
        chargingState = false;
        chargeTimeLeft = maxChargeTime;
        inChargeCircle = false;
        coolDown = maxCoolDown;

    }

    // Update is called once per frame
    void Update()
    {
        if(chargeTimeLeft <= 0)
        {
            chargingState = false;

            if(!inChargeCircle)
            {
                if(coolDown > 0)
                {
                    reduceCoolDown();
                }
                else
                {
                    coolDown = maxCoolDown;
                    chargeTimeLeft = maxChargeTime;
                }
            }
        }

        if(chargingState)
        {
            reduceTimeLeft();
        }
    }

    void reduceTimeLeft()
    {
        chargeTimeLeft -= Time.deltaTime;
    }

    // IEnumerator cooldownReset()
    // {
    //     //coolDown = maxCoolDown;

    //     while(coolDown > 0)
    //     {
    //         if(!inChargeCircle)
    //         {
    //             reduceCoolDown();
    //         }

    //     }
    //     chargeTimeLeft = maxChargeTime;
    //     yield return null;
        
    // }

    private void reduceCoolDown()
    {
        coolDown -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inChargeCircle = true;
            chargingState = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inChargeCircle = false;
            chargingState = false;
        }
    }

}
