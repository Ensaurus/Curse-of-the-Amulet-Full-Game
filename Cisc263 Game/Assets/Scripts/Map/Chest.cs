using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class Chest : MonoBehaviour
{
    public bool isOpened;
    public bool playerIsColliding;
    [SerializeField] private Animator chestAnimator;
    public GameObject contains;

    void OnEnable()
    {
        isOpened = false;
        playerIsColliding = false;
        chestAnimator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        //open chest
        if (Input.GetKeyDown(KeyCode.R) && playerIsColliding && !isOpened)
        {
            chestAnimator.SetBool("chestOpened", true);
            isOpened = true;
            PowerUp newFind = contains.GetComponent<PowerUp>();
            newFind.OnCollect();
            EventManager.Instance.PowerUpCollected.Invoke(contains);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(isOpened == false)
                UIManager.Instance.openChestTextDisplay();

            playerIsColliding = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isOpened == true)
        {
            UIManager.Instance.openChestTextHide();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            {
                UIManager.Instance.openChestTextHide();

                playerIsColliding = false;
            }
    }
}