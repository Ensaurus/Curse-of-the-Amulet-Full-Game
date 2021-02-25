﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        ROAMING,
        TRACKING,
        ATTACKING
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
    [SerializeField] private float roamSpeed;
    [SerializeField] private float trackSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private GameObject player;
    private Transform myTransform;
    private Transform playerTransform;
    private bool cooldown;

    private Vector2 target;
    
    // for roaming
    private bool searching;
    [SerializeField] private float searchTime;

    // for tracking
    private Scent currentScent;
    [SerializeField] private float cooldownTime;  // time in seconds to cool down tracking mode
    private float currentConfidence;    // time in seconds enemy will be in tracking mode

    // for attacking
    [SerializeField] private float attackTime;  // time in seconds for enemy to attack player


    // Start is called before the first frame update
    void Start()
    {
        // make functions listen for events
        EventManager.Instance.PlayerSeen.AddListener(SeePlayer);
        EventManager.Instance.PlayerNoise.AddListener(HearSound);
        
        // set up transforms
        myTransform = this.transform;
        playerTransform = player.transform;

        // set up first target
        float targetX = Random.Range(0, SceneManager.Instance.levelWidth);
        float targetY = Random.Range(0, SceneManager.Instance.levelHeight);
        target.Set(targetX, targetY);
        // Debug.Log("target: " + target);

        // set up starting state
        ChangeState(State.ROAMING);
        searching = false;
        cooldown = false;
        currentConfidence = 0;
        speed = roamSpeed;        
    }

    // Update is called once per frame
    void Update()
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


    private void Move()
    {
        // move towards target
        myTransform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
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

        while (searchTimer <= searchTime && activeState == State.ROAMING)
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
        gameObject.AddComponent<BoxCollider2D>();
        searching = false;
    }

    
    // set new target from a random point within a certain radius of given point
    private void generateTargetNear(Vector2 point, float radius)
    {
        float newX = Random.Range(point.x - radius, point.x + radius);
        float newY = Random.Range(point.y - radius, point.y + radius);

        target.Set(newX % SceneManager.Instance.levelWidth, newY % SceneManager.Instance.levelHeight);
    }

    // set new target as a radom point in bounds of level
    private void TargetRandomPointOnMap()
    {
        // set new target anywhere on the map
        float newX = Random.Range(0, SceneManager.Instance.levelWidth);
        float newY = Random.Range(0, SceneManager.Instance.levelHeight);
        target.Set(newX, newY);
        // Debug.Log("New Target: " + target);
    }


    #endregion



    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Hit something");
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
        }
    }


    #endregion


    #region Attacking
    private void SeePlayer()
    {
        if (activeState != State.ATTACKING)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        ChangeState(State.ATTACKING);
        float attackTimer = 0f;

        // move towards player
        while (!PositionIsVeryNear(myTransform.position, playerTransform.position))
        {
            // Debug.Log("player pos: " + playerTransform.position + " enemy pos: " + myTransform.position);
            target = playerTransform.position;
            Move();
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //stay on player for set time
        while (attackTimer <= attackTime)
        {
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

        while (cooldownTimer <= cooldownTime)
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
        Debug.Log("changed state to: " + activeState);
        EventManager.Instance.EnemyStateChange.Invoke(newState);
        switch (newState){
            case State.ATTACKING:
                speed = attackSpeed;
                break;
            case State.TRACKING:
                speed = trackSpeed;
                break;
            case State.ROAMING:
                speed = roamSpeed;
                break;
        }
    }
}