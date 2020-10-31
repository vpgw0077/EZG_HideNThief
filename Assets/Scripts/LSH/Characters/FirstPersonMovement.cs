using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FirstPersonMovement : MonoBehaviour
{
    float recoverTime = 0;

    public bool useDrink;
    public float Movespeed = 5f;
    public Slider Stamina_Bar;

    Vector2 velocity;

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
        }

        Run();
    }
    private void FixedUpdate()
    {
        velocity.y = Input.GetAxis("Vertical") * Movespeed * Time.deltaTime;
        velocity.x = Input.GetAxis("Horizontal") * Movespeed * Time.deltaTime;
        transform.Translate(velocity.x, 0, velocity.y);
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && theGround.isGrounded)
        {

            if (Stamina_Bar.value > 0)
            {
                Movespeed = 15f;
                Stamina_Bar.value -= 0.005f;
                recoverTime = 0;
            }
            else if (Stamina_Bar.value == 0)
            {
                Movespeed = 3f;
                recoverTime = 0;
            }

            if (useDrink)
            {
                recoverTime = 0;
                StartCoroutine(DrinkOver());
            }

        }
        else
        {

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
