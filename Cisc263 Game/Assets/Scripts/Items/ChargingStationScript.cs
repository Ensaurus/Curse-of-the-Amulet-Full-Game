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
    private Animator chargingStationAnimator;
    [SerializeField] GameObject chargingStationObject;
    [SerializeField] private GameObject yellowLight;
    [SerializeField] private GameObject orangeLight;

    // Start is called before the first frame update
    void Start()
    {
        chargingStationAnimator = gameObject.GetComponent<Animator>();
        chargingState = false;
        chargeTimeLeft = maxChargeTime;
        inChargeCircle = false;
        coolDown = maxCoolDown;

        // yellowLight = chargingStationObject.transform.GetChild(0).gameObject;
        // orangeLight = chargingStationObject.transform.GetChild(1).gameObject;
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

    void FixedUpdate()
    {
        if ((chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("rechargingStation") || chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("powerDown")) && inChargeCircle)
        {
            //pause animation
            chargingStationAnimator.speed = 0;
        }
        else if ((chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("rechargingStation") || chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("powerDown")) && !inChargeCircle)
        {
            //resume animation
            chargingStationAnimator.speed = 1;
        }

        //Changing light colour
        //This is not very inefficient, might fix later
        if ((chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle")) || (chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("transitionToCharge")) || (chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("chargingPlayer")))
        {
            yellowLight.SetActive(true);
            orangeLight.SetActive(false);
        }
        else if((chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("rechargingStation")))
        {
            yellowLight.SetActive(false);
            orangeLight.SetActive(true);
        }
    }

    void reduceTimeLeft()
    {
        chargeTimeLeft -= Time.deltaTime;
    }

    private void reduceCoolDown()
    {
        coolDown -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("rechargingStation") || chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("powerDown"))
            {
                //Pause the animation
                chargingStationAnimator.speed = 0;
            }
            else if(chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("chargingPlayer"))
            {
                //resume animation
                chargingStationAnimator.speed = 1;
            }

            chargingStationAnimator.SetBool("playerCollide", true);
            inChargeCircle = true;
            chargingState = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("chargingPlayer"))
            {
                //Pause the animation
                chargingStationAnimator.speed = 0;
            }
            else if(chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("rechargeStation") || chargingStationAnimator.GetCurrentAnimatorStateInfo(0).IsName("powerDown"))
            {
                //resume animation
                chargingStationAnimator.speed = 1;   
            }

            chargingStationAnimator.SetBool("playerCollide", false);
            inChargeCircle = false;
            chargingState = false;
        }
    }

}
