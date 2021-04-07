using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostSoul : MonoBehaviour
{
    [SerializeField] private float lifespan;
    [SerializeField] private float stepSize;
    [SerializeField] private bool alive;
    // just moves towards exit portal until it's time is up
    
    void Start()
    {
        alive = true;
        StartCoroutine(FizzleOut());
        StartCoroutine(MoveToExit());
    }

    IEnumerator MoveToExit()
    {
        while (alive)
        {
            Vector3 curPos = gameObject.transform.position;
            Vector3 exitPos = Exit.Instance.gameObject.transform.position;
            exitPos.z = curPos.z;
            float speed = stepSize * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(curPos, exitPos, speed);
            yield return Time.deltaTime;
        }
    }

    IEnumerator FizzleOut()
    {
        while (lifespan >= 0)
        {
            lifespan -= Time.deltaTime;
            yield return null;
        }
        alive = false;
        gameObject.SetActive(false);
    }
}
