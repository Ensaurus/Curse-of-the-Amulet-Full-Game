﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet : Singleton<Amulet>
{
    public float charge;
    public bool isActive;

    // temp, move to a better place
    //[SerializeField] private GameObject stoneSprite;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && charge > 0)
        {
            StartCoroutine(Activate());
        }
    }


    IEnumerator Activate()
    {
        //stoneSprite.SetActive(true);

        while (Input.GetKey(KeyCode.Space) && charge > 0) {
            isActive = true;
            charge -= Time.deltaTime;
            yield return null;
        }

        // deactivate
        isActive = false;
       // stoneSprite.SetActive(false);
    }
}