using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public GameObject theCamera;


    RaycastHit hit;
    public MissionCreate theMission;

    RockController theRock;
    SmokeShellController theSmoke;
    FlashBangController theFlash;
    // Start is called before the first frame update
    void Start()
    {
        theMission = FindObjectOfType<MissionCreate>();
        theRock = GetComponentInChildren<RockController>();
        theSmoke = GetComponentInChildren<SmokeShellController>();
        theFlash = GetComponentInChildren<FlashBangController>();
        
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
                else if (hit.transform.CompareTag("Rock") && !(theRock.HoldCount >= theRock.MaxCount))
                {
                    theRock.HoldCount += 1;
                    hit.transform.gameObject.SetActive(false);

                }
                else if (hit.transform.CompareTag("FlashBang") && !(theFlash.HoldCount >= theFlash.MaxCount))
                {
                    theFlash.HoldCount += 1;
                    hit.transform.gameObject.SetActive(false);

                }
                else if (hit.transform.CompareTag("SmokeShell") && !(theSmoke.HoldCount >= theSmoke.MaxCount))
                {
                    theSmoke.HoldCount += 1;
                    hit.transform.gameObject.SetActive(false);

                }

            }
        }
    }


}
