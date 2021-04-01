using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chargingFlame : MonoBehaviour
{

    private Animator flameAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        flameAnimator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            flameAnimator.SetBool("isDestroyed", true);
            StartCoroutine(deactivate());
        }
    }

    //Destroy object once used
    private IEnumerator deactivate()
    {
        // float duration = 5f;
        // float totalTime = 0;

        // while(totalTime <= duration)
        // {
        //     totalTime += Time.deltaTime;
        // }
        // if(totalTime >= duration){
        //     //gameObject.SetActive(false);
        // }
        
        // yield return null;
        yield return new WaitForSeconds (0.375f);
        gameObject.SetActive(false);
    }
}
