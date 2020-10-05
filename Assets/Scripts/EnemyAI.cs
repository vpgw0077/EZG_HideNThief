using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float fov = 120f;
    public GameObject Player;
    public float viewDistance = 10f;
    public float wanderRadius = 8f;
    public float wanderSpeed;
    public float ChaceSpeed;

    public float Distance;
    bool isAware = false;
    public Vector3 wanderPoint;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        wanderPoint = RandomWanderPoint();


    }

    // Update is called once per frame
    void Update()
    {
        if (isAware)
        {
            agent.SetDestination(Player.transform.position);
            agent.speed = ChaceSpeed;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.black;

        }

        else
        {
            Wander();
            SearchForPlayer();
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

        }



    }

    public void CheckDistance()
    {

        isAware = false;

    }

    private void SearchForPlayer()
    {

        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(Player.transform.position)) < fov / 2f)
        {
            if (Vector3.Distance(Player.transform.position, transform.position) < viewDistance)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, Player.transform.position, out hit, -1))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        OnAware();
                    }
                }
            }
        }
    }

    public void OnAware()
    {
        isAware = true;

    }

    public void Wander()
    {

        agent.speed = wanderSpeed;
        if (Vector3.Distance(transform.position, wanderPoint) < 3f)
        {
            wanderPoint = RandomWanderPoint();
        }
        else
        {
            agent.SetDestination(wanderPoint);
        }


    }

    public Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (UnityEngine.Random.insideUnitSphere * wanderRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, wanderRadius, NavMesh.AllAreas);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);

    }
}
