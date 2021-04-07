using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet : Singleton<Amulet>
{
    public float charge;
    public bool isActive;
    public float talismanBuff;   // talisman sets to 1.5f to decrease charge decrease rate by 1.5x

    public bool isCharging;
    public float chargeSpeed;
    private Collider2D playerCol;
    private Animator playerAnimator;

    [SerializeField] private GameObject stoneSpriteCollider;
    [SerializeField] private AudioSource turnToStone;

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
        turnToStone.Play();

        while (Input.GetKey(KeyCode.Space) && charge > 0) {
            isActive = true;
            charge -= Time.deltaTime / talismanBuff;
            yield return null;
        }

        // deactivate
        stoneSpriteCollider.SetActive(false);
        playerCol.enabled = true;
        isActive = false;
        playerAnimator.SetBool("frozen", isActive);
        turnToStone.Play();
    }

    public void IncreaseAmuletCharge()
    {
        charge += (Time.deltaTime * chargeSpeed);
    }


}
