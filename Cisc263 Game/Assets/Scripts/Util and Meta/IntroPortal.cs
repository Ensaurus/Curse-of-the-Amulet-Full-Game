using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroPortal : MonoBehaviour
{
    //[SerializeField] private AudioSource introMusic;
    [SerializeField] private GameObject pickupChestText;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            if (IntroChest.Instance.isOpened)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                pickupChestText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        pickupChestText.SetActive(false);
    }
}
