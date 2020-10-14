using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeShellTrigger : MonoBehaviour
{
    EnemyAI theEnemy;

    private void Update()
    {
        //Gizmos.DrawCube(transform.position, new Vector3(20, 3, 20));
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            theEnemy = other.GetComponent<EnemyAI>();
            theEnemy.isBlind = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            theEnemy.isBlind = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(20, 3, 20));
    }

}
