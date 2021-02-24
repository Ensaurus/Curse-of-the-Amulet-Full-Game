using UnityEngine;


public class PlayerController: MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    Vector2 movement;

    private Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        playerAnimator.SetFloat("horizontal", movement.x);
        playerAnimator.SetFloat("vertical", movement.y);
        playerAnimator.SetFloat("speed", movement.sqrMagnitude);
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