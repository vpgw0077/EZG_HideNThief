using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopChace : MonoBehaviour
{
    EnemyAI TheAI;
    // Start is called before the first frame update
    void Start()
    {
        TheAI = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TheAI.CheckDistance();
        }
    }

}
