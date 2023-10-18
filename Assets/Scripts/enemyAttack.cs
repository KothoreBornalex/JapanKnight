using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static IStatistics;

public class enemyAttack : MonoBehaviour
{
    [Header("AI Fields")]
    public Transform[] waypoints;
    [Range(0, 5)] public int degats = 1;

    private int currentIndex;


    private float timer;
    [Range(0, 5)] public float cooldown;



    void Start() {

    }

    void Update()
    {
        Patrol();
        HandleAttack();
    }


    public float GetDistance()
    {
        return Vector2.Distance(transform.position, PlayerStateMachine.instance.transform.position);
    }

    public bool IsPlayerInSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, PlayerStateMachine.instance.transform.position - transform.position, 1000, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            return hit.collider.tag == "Player";
        }
        return false;
    }

    public void HandleAttack()
    {
        float distance = GetDistance();
        bool isInSight = IsPlayerInSight();

        if (distance <= 1.5f && isInSight)
        {
            timer += Time.deltaTime;
            if (timer > cooldown)
            {
                Debug.Log("attack!");
                PlayerStateMachine.instance.DecreaseStat(StatName.Health, degats);
                timer = 0;
            }
            else
            {
                //patPatrol.enabled = true;
            }
        }
    }
    public void Patrol()
    {
        if (waypoints.Length == 0)
        {
            return;//le code s'arrete
        }


        if (transform.position == waypoints[currentIndex].position)
        {
            if (currentIndex == waypoints.Length - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentIndex].position, 2.5f * Time.deltaTime);
        }

    }

    private void OnDrawGizmos()
    {
        if(PlayerStateMachine.instance != null)
        {
            Ray ray = new Ray();
            ray.direction = PlayerStateMachine.instance.transform.position - transform.position;
            ray.origin = transform.position;
            Gizmos.DrawRay(ray);
        }
        
    }
}

