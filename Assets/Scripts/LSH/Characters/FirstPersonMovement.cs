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
    public float Movespeed;
    public Slider Stamina_Bar;
    public Image WarningIMG;

    Vector2 velocity;
    Vector3 lastPos;

    float h;
    float v;

    GroundCheck theGround;
    Rigidbody rb;


    public AudioClip[] WalkSound;
    public AudioSource SoundPlayer;
    public float RunTime;

    private void Start()
    {
        theGround = GetComponentInChildren<GroundCheck>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Run();

        if (useDrink)
        {
            Stamina_Bar.value = 1;
        }
        if (isMove && theGround.isGrounded)
        {
            ItemManager.currentWeaponAnim.SetBool("Walk", true);

            if (isRun)
            {
                ItemManager.currentWeaponAnim.SetBool("Run", true);

                if (RunTime >= 0.27f)
                {
                    RunTime = 0;
                    RunStepSound();
                    
                }
                StopCoroutine(FootStepSound());
            }
            else
            {
                ItemManager.currentWeaponAnim.SetBool("Run", false);
                if (!SoundPlayer.isPlaying)
                {

                    StartCoroutine(FootStepSound());

                }
                
            }

        }
        else
        {
            ItemManager.currentWeaponAnim.SetBool("Walk", false);
            StopCoroutine(FootStepSound());
        }

        if(GameController.instance.PoliceAware.Count > 0)
        {
            WarningIMG.gameObject.SetActive(true);
        }
        else if(GameController.instance.PoliceAware.Count == 0)
        {
            WarningIMG.gameObject.SetActive(false);
        }

        if (!theGround.isGrounded)
        {
            rb.AddForce(Vector3.down * 0.2f, ForceMode.VelocityChange);
        }

    }
    IEnumerator FootStepSound()
    {
        yield return new WaitForSeconds(0.05f);
        SoundPlayer.clip = WalkSound[UnityEngine.Random.Range(0, WalkSound.Length)];
        SoundPlayer.Play();
    }
    void RunStepSound()
    {
        SoundPlayer.clip = WalkSound[UnityEngine.Random.Range(0, WalkSound.Length)];
        SoundPlayer.Play();


    }

    private void FixedUpdate()
    {
        /*velocity.y = Input.GetAxis("Vertical") * Movespeed * Time.deltaTime;
        velocity.x = Input.GetAxis("Horizontal") * Movespeed * Time.deltaTime;
        transform.Translate(velocity.x, 0, velocity.y);*/

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");


        Vector3 horizontal = transform.right * h;
        Vector3 vertical = transform.forward * v;
        Vector3 velocity = (horizontal + vertical).normalized * Movespeed;

        rb.MovePosition(transform.position + velocity * Time.deltaTime);

        MoveCheck();


    }

    private void MoveCheck()
    {

        if (Vector3.Distance(new Vector3(lastPos.x, 0, lastPos.z), new Vector3(transform.position.x, 0, transform.position.z)) >= 0.01f)
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
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {

            if (Stamina_Bar.value > 0)
            {
                isRun = true;
                Movespeed = 13f; //달리기 속도
                Stamina_Bar.value -= 0.0005f;  //스태미너 떨어지는 속도
                recoverTime = 0;
                RunTime += Time.deltaTime; 
            }
            else if (Stamina_Bar.value == 0) //스태미너가 0이 됐을때
            {
                isRun = false;
                Movespeed = 3f; //스태미너가 0이됐을때 속도
                recoverTime = 0;
                RunTime = 0;
            }


        }
        else
        {
            isRun = false;
            Movespeed = 7f;
            RunTime = 0;
            if (Stamina_Bar.value > 0)
            {
                recoverTime += Time.deltaTime;
            }
            else if (Stamina_Bar.value == 0)
            {
                Movespeed = 3f;
                recoverTime += 0.003f;
            }
        }
        if (recoverTime > 1)
        {
            Stamina_Bar.value += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameClear"))
        {
            GameController.instance.isClear = true;
        }
    }

}
