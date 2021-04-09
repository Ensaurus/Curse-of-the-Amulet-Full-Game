using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    Vector2 movement;

    private Animator playerAnimator;

    public bool isTalkingToSpirit;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        isTalkingToSpirit = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(isTalkingToSpirit){
            playerAnimator.SetFloat("horizontal", 0);
            playerAnimator.SetFloat("vertical", 0);
            playerAnimator.SetFloat("speed", 0);
        }
        else {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            playerAnimator.SetFloat("horizontal", movement.x);
            playerAnimator.SetFloat("vertical", movement.y);
            playerAnimator.SetFloat("speed", movement.sqrMagnitude);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
