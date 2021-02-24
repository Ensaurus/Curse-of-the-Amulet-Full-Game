using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class movement_2D : MonoBehaviour
{
    public float speed = 5f;

    public Rigidbody2D rb;
    Vector2 movement;

    private bool is_stone;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (is_stone == false)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            animator.SetFloat("horizontal", movement.x);
            animator.SetFloat("vertical", movement.y);
            animator.SetFloat("speed", movement.sqrMagnitude);
        }

        /*
        if (Input.GetKey("space"))
        {

            is_stone = true;
            animator.SetBool("frozen", true);

            movement.x = 0;
            movement.y = 0;
        }
        else if (Input.GetKeyUp("space"))
        {
            DateTime startTime = DateTime.Now;
            double time_elapsed = 0;
            //print("back to normal");
            while (time_elapsed < 5000)
            {
                //print("waiting");
                time_elapsed = (DateTime.Now - startTime).TotalMilliseconds;
            }
            is_stone = false;
            animator.SetBool("frozen", false);
        }
        */
    }

    void FixedUpdate()
    {
        // move if not using amulet
        if (!Amulet.Instance.isActive)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }

}