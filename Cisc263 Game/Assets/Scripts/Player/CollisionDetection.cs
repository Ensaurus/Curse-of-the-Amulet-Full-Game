using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Player hit something");
        // if collided enemy and
        if (other.CompareTag("Enemy") && !Amulet.Instance.isActive) 
        {
            //Debug.Log("it was an enemy");
            // can add a condition here for health later if we want
            EventManager.Instance.JumpScare.Invoke();
            EventManager.Instance.Death.Invoke();
        }
    }

}
