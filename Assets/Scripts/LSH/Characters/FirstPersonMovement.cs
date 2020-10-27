using UnityEngine;
using UnityEngine.UI;

public class FirstPersonMovement : MonoBehaviour
{
    float recoverTime = 0;

    public float Movespeed = 5f;
    public Slider Stamina_Bar;
    Vector2 velocity;

    void Update()
    {
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (Stamina_Bar.value > 0)
                {
                    Movespeed = 15f;
                    Stamina_Bar.value -= 0.001f;
                    recoverTime = 0;
                }
                else if (Stamina_Bar.value == 0)
                {
                    Movespeed = 3f;
                    recoverTime = 0;
                }
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

}
