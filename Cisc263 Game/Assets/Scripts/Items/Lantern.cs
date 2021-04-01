using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : Singleton<Lantern>
{
    public bool isOn = false;
    [SerializeField] private GameObject lantern;
    [SerializeField] private GameObject lanternLight;
    [SerializeField] private GameObject idleLight;
    [SerializeField] private GameObject activeGlow;
    private Transform myTransform;
    [SerializeField] private float maxEnergy;   // max energy flashlight can hold (also functions as starting energy
    public float currentEnergy;    // time in seconds before latern runs out of energy

    [SerializeField] public float flameEnergy;   // time in seconds that each flame will add to currentEnergy

    public bool triggered = false;
    public void Start()
    {
        currentEnergy = maxEnergy;
        myTransform = lanternLight.transform;
    }



    void Update()
    {
        //Turning the light on
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(ToggleLantern());
        }

        if (isOn)
        {
            FollowMouse();
        }
    }


    IEnumerator ToggleLantern()
    {
        isOn = !isOn;

        if (isOn && currentEnergy > 0)
        {
            idleLight.SetActive(false);
            activeGlow.SetActive(true);
            lanternLight.SetActive(true);
            while (isOn && currentEnergy > 0)
            {
                currentEnergy -= Time.deltaTime;
                yield return null;
            }
            lanternLight.SetActive(false);
            activeGlow.SetActive(false);
            idleLight.SetActive(true);
        }
    }


    private void FollowMouse()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        myTransform.LookAt(mouse);
    }



    // currentEnergy increased when flame collected in CollisionDetection
    public void IncreaseCurrentEnergy()
    {
        currentEnergy += flameEnergy;
        triggered = true;
    }




    // original: 
    /* 
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
           isOn = !isOn;
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

    // moved to CollisionDetection

    public void OnTriggerEnter(Collider other){
        if(other.tag == "flame"){
            flamePickedUp = other.gameObject;
            flames += 1;
            Destroy(flamePickedUp);
        }
    }
    */

}

