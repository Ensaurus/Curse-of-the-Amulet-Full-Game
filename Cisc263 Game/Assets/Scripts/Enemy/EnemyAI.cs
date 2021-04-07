﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        ROAMING,
        TRACKING,
        ATTACKING,
        TRAPPED
    }

    private State activeState; 
    public State ActiveState
    {
        get
        {
            return ActiveState;
        }
    }

    private float speed;
    private Transform myTransform;
    private Transform playerTransform;
    private bool cooldown;
    private bool stopped;
    private Vector2 target;
    
    // for roaming
    private bool searching;

    // for tracking
    private Scent currentScent;
    private float currentConfidence;    // time in seconds enemy will be in tracking mode

   
    [SerializeField] private AudioSource trackingSound;  //Sound to indicate start of tracking
    [SerializeField] private AudioSource trackingSniff;  //Sound to indicate continued tracking
    [SerializeField] private AudioSource attackStone;  //Sound to indicate continued tracking

    [SerializeField] private AudioSource attackingSound; //Sound to indicate attacking

    private bool trackingToggle = false; //Used to loop sound if tracking

    // Start is called before the first frame update
    void Awake()
    {
        // set up transforms
        myTransform = this.transform;
        playerTransform = EnemyStats.Instance.player.transform;
        // make functions listen for events
        EventManager.Instance.PlayerSeen.AddListener(SeePlayer);
        EventManager.Instance.PlayerNoise.AddListener(HearSound);
        EventManager.Instance.FadeComplete.AddListener(ToggleStop); // unfreeze enemy only when level starts
    }

    private void OnEnable()
    {
        // set up first target
        float targetX = Random.Range(0, SceneController.Instance.levelDimensions.x);
        float targetY = Random.Range(0, SceneController.Instance.levelDimensions.y);
        target.Set(targetX, targetY);

        // set up starting state
        ChangeState(State.TRAPPED);
        stopped = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (activeState != State.TRAPPED)
        {
            myTransform.LookAt(target, Vector3.forward);
            if (activeState != State.ATTACKING)
            {
                Move();
            }

            if (activeState == State.ROAMING)
            {
                Roam();
            }
        }
    }

    private void ToggleStop()
    {
        // not sure this is best fix, but i needed to make sure enemies in the enemy pool didn't get toggled before being enabled in the scene
        if (gameObject.activeInHierarchy)
        {
            stopped = !stopped;
            if (stopped)
            {
                ChangeState(State.TRAPPED);
            }
            else
            {
                ChangeState(State.ROAMING);
            }

        }
    }

    private void Move()
    {
        // move towards target
        myTransform.position = Vector2.MoveTowards(myTransform.position, target, speed*Time.deltaTime);
        // myTransform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        Debug.DrawRay(myTransform.position, myTransform.forward * speed * Time.deltaTime, Color.blue);
    }



    #region Roaming

    private void Roam()
    {
        // roam target reached
        if (!searching && PositionIsNear(myTransform.position, target))
        {
            StartCoroutine(Search());
        }
    }


    // wander randomly around roam target for searchTime seconds
    IEnumerator Search()
    {
        searching = true;
        Vector2 searchOrigin = target;
        float searchTimer = 0;

        while (searchTimer <= EnemyStats.Instance.searchTime && activeState == State.ROAMING)
        {        
            if (PositionIsNear(myTransform.position, target))
            {
                generateTargetNear(searchOrigin, 15);
            }

            searchTimer += Time.deltaTime;
            yield return null;
        }
        
        if (activeState == State.ROAMING)
        {   
            // set up next roam target
            TargetRandomPointOnMap();
        }
        searching = false;
    }

    
    // set new target from a random point within a certain radius of given point
    private void generateTargetNear(Vector2 point, float radius)
    {
        float newX = Random.Range(point.x - radius, point.x + radius);
        float newY = Random.Range(point.y - radius, point.y + radius);

        if (newX < 0)
        {
            newX = -newX;
        }
        if (newY < 0)
        {
            newY = -newY;
        }
        target.Set(newX % SceneController.Instance.levelDimensions.x, newY % SceneController.Instance.levelDimensions.y);
    }

    // set new target as a radom point in bounds of level
    private void TargetRandomPointOnMap()
    {
        // set new target anywhere on the map
        float newX = Random.Range(0, SceneController.Instance.levelDimensions.x);
        float newY = Random.Range(0, SceneController.Instance.levelDimensions.y);
        target.Set(newX, newY);
        // Debug.Log("New Target: " + target);
    }


    #endregion



    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Hit something");
        if (other.CompareTag("Trap"))
        {
            TrapSigil script = other.GetComponent<TrapSigil>();
            StopAllCoroutines();
            StartCoroutine(StuckInTrap(script.trapTime, other.gameObject));
        }
        if (other.CompareTag("Scent") && activeState != State.ATTACKING)
        {
            // Debug.Log("it was a scent node");
            Scent node = other.GetComponent<Scent>();
            float confidence = node.pungence;

            // if node is fresher than current node or first node seen
            if (currentScent == null || node.pungence > currentScent.pungence)
            {
                currentScent = node;
            }

            // if node is over a couple seconds fresher than ours was when we found it (i.e. ran into a new fresher trail)
            if ((confidence >= currentConfidence + 2) && !cooldown)
            {
                // restart tracking
                currentConfidence = confidence;
                StartCoroutine(Tracking(false));
            }
        }
    }

    IEnumerator StuckInTrap(float timeStuck, GameObject trap)
    {
        ToggleStop();
        while (timeStuck >= 0)
        {
            timeStuck -= Time.deltaTime;
            yield return null;
        }
        ToggleStop();
        TrapSigilManager.Instance.activeTraps.Remove(trap);
        Destroy(trap);
    }

    #region Tracking

    // needs testing to determine how confidence should be calculated
    private void HearSound(float volume)
    {
        float distanceFromPlayer = (playerTransform.position - myTransform.position).magnitude; 
        float confidence = volume / distanceFromPlayer;

        if ((confidence > currentConfidence) && !cooldown){
            currentConfidence = confidence;
            StartCoroutine(Tracking(true));

        }
    }

    IEnumerator Tracking(bool sound)
    {
        ChangeState(State.TRACKING);
        trackingSound.Play(); //Sound indicated enemy has started to track
        
        
        trackingToggle = true;
        trackingSniff.Play(); //Sound indicating continued tracking

      
        float uncertainty = 0;
        while (uncertainty <= currentConfidence && activeState == State.TRACKING)
        {
            if (!sound)
            {
                // move towards next scent
                target = currentScent.next;
            }
            else
            {
                // move towards player
                target = playerTransform.position;
            }
            // Debug.Log("next node: " + currentScent.next);

            uncertainty += Time.deltaTime;
            yield return null;
        }

        currentConfidence = 0f;
        if (activeState != State.ATTACKING)
        {
            StartCoroutine(CoolingDown());
            trackingSniff.Stop();
        }
    }


    #endregion


    #region Attacking
    private void SeePlayer(EnemyAI enemy)
    {
        // only do anything if this was the enemy that saw the player 
        if (enemy == this)
        {
            // Debug.Log("I alone saw the player and I'm attacking now.");
            if (activeState != State.ATTACKING && activeState != State.TRAPPED)
            {
                StartCoroutine(Attack());
                if (trackingToggle == true){
                    trackingToggle = false;
                    trackingSniff.Stop();
                }
            }
        }
    }

    IEnumerator Attack()
    {
        ChangeState(State.ATTACKING);
        float attackTimer = 0f;
        attackingSound.Play();

        // move towards player
        while (!PositionIsVeryNear(myTransform.position, playerTransform.position))
        {
            target = playerTransform.position;
            Move();
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //stay on player for set time
        while (attackTimer <= EnemyStats.Instance.attackTime)
        {
            attackStone.Play();
            // TODO: run attack animation and prevent moving
            attackTimer += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(CoolingDown());
    }

    #endregion

    IEnumerator CoolingDown()
    {
        float cooldownTimer = 0f;

        ChangeState(State.ROAMING);
        cooldown = true;

        while (cooldownTimer <= EnemyStats.Instance.cooldownTime)
        {
            cooldownTimer += Time.deltaTime;
            yield return null;
        }

        cooldown = false;
    }

    // returns true is point is within a radius of 0.5 Unity units from other
    private bool PositionIsNear(Vector3 point, Vector2 other)
    {
        if ((point.x <= other.x + 0.5 && point.x >= other.x - 0.5) && (point.y <= other.y + 0.5 && point.y >= other.y - 0.5))
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    // returns true is point is within a radius of 0.2 Unity units from other
    private bool PositionIsVeryNear(Vector3 point, Vector2 other)
    {
        if ((point.x <= other.x + 0.2 && point.x >= other.x - 0.2) && (point.y <= other.y + 0.2 && point.y >= other.y - 0.2))
        {
            return true;
        }
        else
        {
            return false;
        }

    }



    private void ChangeState(State newState)
    {
        activeState = newState;
        EventManager.Instance.EnemyStateChange.Invoke(newState);
        switch (newState){
            case State.ATTACKING:
                speed = EnemyStats.Instance.attackSpeed;
                break;
            case State.TRACKING:
                speed = EnemyStats.Instance.trackSpeed;
                break;
            case State.ROAMING:
                speed = EnemyStats.Instance.roamSpeed;
                break;
            case State.TRAPPED:
                // reset everything, so when it goes free starts in normal roaming
                searching = false;
                cooldown = false;
                currentConfidence = 0;
                break;
        }
    }
}
