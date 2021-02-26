using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Singleton<Exit>
{
    private Transform myTransform;
    private Vector3 rotationDirection;
    public float requiredEnergy; // charge required in amulet to be able to activate exit

    private void Start()
    {
        myTransform = this.transform;
        rotationDirection = new Vector3(0, 0, -0.1f);
    }

    void Update()
    {
        Rotate();   
    }

    private void Rotate()
    {
        myTransform.Rotate(rotationDirection);
    }
}
