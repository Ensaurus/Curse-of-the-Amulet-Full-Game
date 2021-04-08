using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cave : MonoBehaviour
{
    private bool isThereCameraInside;

    private void OnEnable(){
        isThereCameraInside = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (other.CompareTag("CameraDrop"))
        {
            isThereCameraInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isThereCameraInside)
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
