using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public float lookSensitivity; // 민감도
    public float cameraRotationLimit; // 카메라 회전 제한
    public float currentCameraRotationX = 0; // 현재 카메라 각도
    public GameObject theCamera;
    private Rigidbody myRigid;

    RaycastHit hit;
    MissionCreate theMission;
    // Use this for initialization
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        theMission = FindObjectOfType<MissionCreate>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
        CharacterRotation();
        Interact();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;



    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hit, 5f))
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

    private void CharacterRotation()
    {
        // 좌우 캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));

    }

    private void CameraRotation()
    {
        // 상하 카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
