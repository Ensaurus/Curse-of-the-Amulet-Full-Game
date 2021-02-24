using System.Collections;
using UnityEngine;

/*
 * functions as a simple node class, just holds a reference to the position
 * of the next scent node and fades away after a number of seconds.
*/
public class Scent: MonoBehaviour
{
    public Vector2 next;    // a reference to position of next node
    public float pungence;  // time in seconds before smell fades

    private void OnEnable()
    {
        StartCoroutine(fadeAway());
    }

    IEnumerator fadeAway()
    {
        while (pungence >= 0f)
        {
            pungence -= Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
