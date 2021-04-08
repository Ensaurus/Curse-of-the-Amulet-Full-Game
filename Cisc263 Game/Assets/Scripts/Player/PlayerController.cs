using UnityEngine;


public class PlayerController: Singleton<PlayerController>
{
    public float speed = 5f;
    public bool waitingForTransition = true;   // needs to start as true as FadeComplete is called when scene loaded

    private Rigidbody2D rb;
    Vector2 movement;

    private Animator playerAnimator;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        playerAnimator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        EventManager.Instance.LevelCompleted.AddListener(ToggleWaiting);
        EventManager.Instance.FadeComplete.AddListener(ToggleWaiting);
        EventManager.Instance.Death.AddListener(Dead);
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraManager.Instance.checkMovingAllowed() && !waitingForTransition)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            playerAnimator.SetFloat("horizontal", movement.x);
            playerAnimator.SetFloat("vertical", movement.y);
            playerAnimator.SetFloat("speed", movement.sqrMagnitude);
        }
        else{
            playerAnimator.SetFloat("horizontal", 0);
            playerAnimator.SetFloat("vertical", 0);
            playerAnimator.SetFloat("speed", 0);
        }
        
    }

    void FixedUpdate()
    {
        if (CameraManager.Instance.checkMovingAllowed() && !waitingForTransition)
        {
            // move if not using amulet
            if (!Amulet.Instance.isActive)
            {
                rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
            }
        }
        
    }

    private void ToggleWaiting()
    {
        waitingForTransition = !waitingForTransition;
    }

    private void Dead()
    {
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        collider.enabled = false;
        waitingForTransition = true;
    }

}