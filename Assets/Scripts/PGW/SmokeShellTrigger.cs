using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeShellTrigger : MonoBehaviour
{
    EnemyAI theEnemy;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            theEnemy = other.GetComponent<EnemyAI>();
           // theEnemy.isBlind = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            theEnemy = other.GetComponent<EnemyAI>();
            //theEnemy.isBlind = false;
        }
    }


}
