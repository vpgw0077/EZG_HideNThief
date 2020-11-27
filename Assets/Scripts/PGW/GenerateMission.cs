using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMission : MonoBehaviour
{
    public enum GeneratorType
    {
        Common,
        GasCanGenerator
    }
    public GeneratorType GenType;
    public bool GenerateOn = false;
    public GameObject GeneratorLight;
    public Animator anim;

    public AudioSource SfxPlayer;

    MissionCreate theMission;

    private void Start()
    {
        theMission = FindObjectOfType<MissionCreate>();
    }
    private void Update()
    {
        if (GenerateOn)
        {
            anim.SetTrigger("Operation");
        }
    }

    public void Operation()
    {
        if (GenType == GeneratorType.Common)
        {
            GenerateOn = true;
            ++theMission.CurrentGenerator;
            GeneratorLight.SetActive(true);
            SfxPlayer.Play();
            theMission.CheckClear();

            Collider[] colls = Physics.OverlapSphere(transform.position, 50f);

            foreach (var coll in colls)
            {
                var police = coll.GetComponent<EnemyAI>();
                if (police != null)
                {
                    police.OnAware();

                }
            }
        }
        else if(GenType == GeneratorType.GasCanGenerator)
        {

            theMission.CheckClear();
            if (theMission.isClear)
            {
                GenerateOn = true;
                SfxPlayer.Play();
            }


        }

    }
}
