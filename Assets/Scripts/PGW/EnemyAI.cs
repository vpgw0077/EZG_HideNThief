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
    public bool isStuned;
    public bool isBlind;
    public Vector3 wanderPoint;
    public float CurrentTime;

    public Animator anim;

    public AudioClip[] WalkSound;
    public AudioSource SoundPlayer;

    float StunTime = 5f;
    public bool isAware = false;

    NavMeshAgent agent;

    Vector3 CheckStuck = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = 0;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        wanderPoint = RandomWanderPoint();



    }

    // Update is called once per frame
    void Update()
    {
         if (Vector3.Distance(Player.transform.position, transform.position) < viewDistance&& Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(Player.transform.position)) < fov)
        {
            print("발견됨");
        }
        if (Vector3.Distance(Player.transform.position, transform.position) < viewDistance)
        {
            print("탐지거리");
        }
        if (agent.velocity == CheckStuck && anim.GetBool("LookAround") == false) // 끼임 확인
        {
            wanderPoint = RandomWanderPoint();
        }

        if (isAware && !isStuned)
        {
            agent.SetDestination(Player.transform.position);
            agent.speed = ChaceSpeed;
            anim.SetBool("Chase", true);


        }

        else
        {
            Wander();
            SearchForPlayer();
            anim.SetBool("Chase", false);


        }



    }

    public void FootStepSound()
    {
        SoundPlayer.clip = WalkSound[UnityEngine.Random.Range(0, WalkSound.Length)];
        SoundPlayer.Play();
    }
    public void DrawAttention(Vector3 SoundSpot)
    {
        wanderPoint = SoundSpot;
        Wander();
    }

    public void CheckDistance()
    {

        isAware = false;
        GameController.instance.PoliceAware.Remove(true);

    }

    private void SearchForPlayer()
    {

        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(Player.transform.position)) < fov)
        {
            if (Vector3.Distance(Player.transform.position, transform.position) < viewDistance)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, Player.transform.position, out hit, -1))
                {
                    if (hit.transform.CompareTag("Player") && !isBlind && !isStuned )
                    {
                        OnAware();
                    }
                }
            }
        }
    }

    public void OnAware()
    {
        if (agent.isStopped)
        {
            anim.SetBool("LookAround", false);
            agent.isStopped = false;

        }
        isAware = true;
        GameController.instance.PoliceAware.Add(isAware);


    }
    public IEnumerator Stun()
    {
        StopCoroutine(LookAround());
        if (isStuned)
        {
            isAware = false;
            GameController.instance.PoliceAware.Remove(true);
            agent.enabled = false;
            anim.SetBool("Stun", true);
            yield return new WaitForSeconds(StunTime);
        }

        agent.enabled = true;
        isStuned = false;
        anim.SetBool("Stun", false);

    }

    public void Wander()
    {
        if (!isStuned)
        {
            agent.speed = wanderSpeed;
            if (Vector3.Distance(transform.position, wanderPoint) < 1f)
            {
                //StartCoroutine(LookAround());
                wanderPoint = RandomWanderPoint();
            }
            else
            {
                agent.SetDestination(wanderPoint);
            }

        }

    }
    IEnumerator LookAround()
    {
        if (agent.enabled == true)
        {
            agent.isStopped = true;
            anim.SetBool("LookAround", true);
            yield return new WaitForSeconds(3.5f);
            anim.SetBool("LookAround", false);
            agent.isStopped = false;

        }

    }

    public Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (UnityEngine.Random.insideUnitSphere * wanderRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, wanderRadius, NavMesh.AllAreas);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isAware)
        {
            if (collision.transform.CompareTag("Player"))
            {
                GameController.instance.isOver = true;
            }
        }


    }
}
