using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FirstPersonMovement : MonoBehaviour
{
    float recoverTime = 0;

    public bool isMove;
    public bool useDrink;
    public bool isRun;
    public float Movespeed = 5f;
    public Slider Stamina_Bar;

    Vector2 velocity;
    Vector3 lastPos;

    GroundCheck theGround;

    private void Start()
    {
        theGround = GetComponentInChildren<GroundCheck>();
    }

    void Update()
    {
        if (useDrink)
        {
            Stamina_Bar.value = 1;
           // StartCoroutine(DrinkOver());
        }

        Run();
        if (isMove)
        {
            ItemManager.currentWeaponAnim.SetBool("Walk",true);
            if (isRun)
            {
                ItemManager.currentWeaponAnim.SetBool("Run", true);
            }
            else
            {
             ItemManager.currentWeaponAnim.SetBool("Run", false);
            }
        }
        else
        {
            ItemManager.currentWeaponAnim.SetBool("Walk", false);
        }
    }
    private void FixedUpdate()
    {
        velocity.y = Input.GetAxis("Vertical") * Movespeed * Time.deltaTime;
        velocity.x = Input.GetAxis("Horizontal") * Movespeed * Time.deltaTime;
        transform.Translate(velocity.x, 0, velocity.y);
        MoveCheck();

        
    }

    private void MoveCheck()
    {

        if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
        {

            isMove = true;



        }
        else
        {
            isMove = false;
            

        }


        lastPos = transform.position;

    }

    void Run()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && theGround.isGrounded)
        {

            if (Stamina_Bar.value > 0)
            {
                isRun = true;
                Movespeed = 15f;
                Stamina_Bar.value -= 0.001f;
                recoverTime = 0;
            }
            else if (Stamina_Bar.value == 0)
            {
                isRun = false;
                Movespeed = 3f;
                recoverTime = 0;
            }


        }
        else
        {
            isRun = false;
            Movespeed = 5f;

            if (Stamina_Bar.value > 0)
            {
                recoverTime += Time.deltaTime;
            }
            else if (Stamina_Bar.value == 0)
            {
                Movespeed = 2f;
                recoverTime += Time.deltaTime;
            }
        }
        if (recoverTime > 1)
        {
            Stamina_Bar.value += Time.deltaTime;
        }
    }

    public IEnumerator DrinkOver()
    {
        yield return new WaitForSeconds(3f);
        useDrink = false;
    }

}
