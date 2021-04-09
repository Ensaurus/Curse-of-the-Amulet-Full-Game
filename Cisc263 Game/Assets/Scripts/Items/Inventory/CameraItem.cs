using UnityEngine;

public class CameraItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MinimapIcon"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MinimapIcon"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
