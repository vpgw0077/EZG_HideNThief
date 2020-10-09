using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public GameObject theCamera;


    RaycastHit hit;
    public MissionCreate theMission;
    // Start is called before the first frame update
    void Start()
    {
        theMission = FindObjectOfType<MissionCreate>();
        
    }


    // Update is called once per frame
    void Update()
    {
        Interact();
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hit, 5f))
            {
                if (hit.transform.CompareTag("Generator"))
                {
                    var Gen = hit.transform.GetComponent<GenerateMission>();
                    if (!Gen.GenerateOn)
                    {
                        Gen.Operation();
                    }
                }

                else if (hit.transform.CompareTag("GasCan"))
                {
                    ++theMission.CurrentGascan;
                    Destroy(hit.transform.gameObject);

                }

            }
        }
    }


}
