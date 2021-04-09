using UnityEngine;

public class IntroChest : Singleton<IntroChest>
{
    public bool isOpened;
    [SerializeField] private GameObject openText;
    [SerializeField] private GameObject chestOpenedText;
    private bool isTouching;
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();       
    }

    private void Update()
    {
        //open chest
        if (!isOpened && isTouching && Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("chestOpened", true);
            isOpened = true;
            chestOpenedText.SetActive(true);
            openText.SetActive(false);            
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isOpened == false)
            {
                openText.SetActive(true);
                isTouching = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            openText.SetActive(false);
            chestOpenedText.SetActive(false);
            isTouching = false;
        }
    }
}
