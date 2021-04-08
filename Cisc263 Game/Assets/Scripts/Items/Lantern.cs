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
    public float maxEnergy;   // max energy flashlight can hold 
    public float startingEnergy;    // amount player starts with
    public float currentEnergy;    // time in seconds before latern runs out of energy

    [SerializeField] public float flameEnergy;   // time in seconds that each flame will add to currentEnergy

    [SerializeField] public AudioSource flamePickedUp; //Sound for when the flames are picked up

    public bool triggered = false;
    public void Start()
    {
        currentEnergy = startingEnergy;
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
        flamePickedUp.Play();
        // triggered used for ui to make text green
        StartCoroutine(Untrigger());
    }

    IEnumerator Untrigger()
    {
        float timer = 0.5f;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        triggered = false;
    }
}

