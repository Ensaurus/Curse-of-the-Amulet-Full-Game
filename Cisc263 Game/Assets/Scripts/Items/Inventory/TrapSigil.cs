using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSigil : MonoBehaviour
{
    private Transform myTransform;
    private Vector3 rotationDirection;
    public float trapTime;

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
