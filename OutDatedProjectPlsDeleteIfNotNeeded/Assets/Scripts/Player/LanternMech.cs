using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternMech : MonoBehaviour
{
    private bool isOn;
    public GameObject flashlight;
    public GameObject lightSource;
    public float maxEnergy;
    private float currentEnergy; 

    private int flames;
    private GameObject flamePickedUp;
    private float usedEnergy;

    public void Start(){
        currentEnergy = maxEnergy;
        maxEnergy = 50 * flames;
    }

    public void FixedUpdate()
    {
        maxEnergy = 50 * flames;
        currentEnergy = maxEnergy;

        //Turning the light on
        if (Input.GetKeyDown(KeyCode.F)){
           isOn =! isOn;
        }
        if (isOn){
            lightSource.SetActive(true);

            if(currentEnergy <= 0){
                lightSource.SetActive(false);
                flames = 0;
                }
            if (currentEnergy > 0){
                lightSource.SetActive(true);
                currentEnergy -= 0.5f * Time.deltaTime;
                usedEnergy += 0.5f * Time.deltaTime;
            }
            if (usedEnergy >= 50){
                flames -= 1;
                usedEnergy = 0;
            }
        }
        else{
            lightSource.SetActive(false);
        }
    }  
    public void OnTriggerEnter(Collider other){
        if(other.tag == "flame"){
            flamePickedUp = other.gameObject;
            flames += 1;
            Destroy(flamePickedUp);
        }
    }
}

