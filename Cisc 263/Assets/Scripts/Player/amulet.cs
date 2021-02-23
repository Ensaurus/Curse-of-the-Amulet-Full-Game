using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class amulet : MonoBehaviour
{
    private bool is_stone;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        is_stone = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("space")){
            //print("space is held");
            is_stone = true;
            animator.SetBool("frozen", true);
        }
        else{
            //print("nothing");
            stone_off();
        }
    }

    void stone_off(){

        if(is_stone){
            DateTime startTime = DateTime.Now;
            double time_elapsed = 0;
            //print("back to normal");
            while(time_elapsed < 5000){
                //print("waiting");
                time_elapsed = (DateTime.Now - startTime).TotalMilliseconds;
            }
            is_stone = false;
            animator.SetBool("frozen", false);
        }
    }
}
