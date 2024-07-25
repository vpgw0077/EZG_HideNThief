using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Range(0, 500f)]
    [SerializeField] private float mouseSensitivity = 0f;
    [SerializeField] private float mouseLimitUp = 90f;
    [SerializeField] private float mouseLimitDown = -70f;
    [SerializeField] private Transform player = null;

    private float mouseX = 0f;
    private float mouseY = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            ControlCam();
        }
    }

    
    private void ControlCam()
    {
        mouseX -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;
        mouseY += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;

        mouseX = Mathf.Clamp(mouseX, mouseLimitDown, mouseLimitUp);

        transform.rotation = Quaternion.Euler(mouseX, mouseY, 0);
        player.rotation = Quaternion.Euler(0, mouseY, 0);
    }
}
