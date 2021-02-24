using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet : Singleton<Amulet>
{
    public float charge;
    public bool isActive;
    private Collider2D playerCol;
    private Animator playerAnimator;
    // temp, move to a better place
    // [SerializeField] private GameObject stoneSprite;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        isActive = false;
        playerCol = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && charge > 0)
        {
            StartCoroutine(Activate());
        }
    }


    IEnumerator Activate()
    {
        isActive = true;
        playerAnimator.SetBool("frozen", isActive);
        playerCol.enabled = false;

        while (Input.GetKey(KeyCode.Space) && charge > 0) {
            isActive = true;
            charge -= Time.deltaTime;
            yield return null;
        }

        // deactivate
        playerCol.enabled = true;
        isActive = false;
        playerAnimator.SetBool("frozen", isActive);
    }
}
