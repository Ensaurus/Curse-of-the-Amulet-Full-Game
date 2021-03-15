using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet : Singleton<Amulet>
{
    public float charge;
    public bool isActive;

    public bool isCharging;
    public float chargeSpeed;
    private Collider2D playerCol;
    private Animator playerAnimator;

    [SerializeField] private GameObject stoneSpriteCollider;


    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        isActive = false;
        playerCol = gameObject.GetComponent<BoxCollider2D>();
        isCharging = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && charge > 0 && !isActive)
        {
            StartCoroutine(Activate());
        }
    }


    // Note: stonerSpriteCollider needs a 2D collider with a "Player" tag so the enemy will behave normally 
    IEnumerator Activate()
    {
        isActive = true;
        playerAnimator.SetBool("frozen", isActive);
        playerCol.enabled = false;
        stoneSpriteCollider.SetActive(true);

        while (Input.GetKey(KeyCode.Space) && charge > 0) {
            isActive = true;
            charge -= Time.deltaTime;
            yield return null;
        }

        // deactivate
        stoneSpriteCollider.SetActive(false);
        playerCol.enabled = true;
        isActive = false;
        playerAnimator.SetBool("frozen", isActive);
    }

    public void IncreaseAmuletCharge()
    {
        charge += (Time.deltaTime * chargeSpeed);
    }


}
