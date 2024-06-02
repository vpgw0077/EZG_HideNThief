using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FirstPersonMovement : MonoBehaviour
{
    public delegate void StaminaUI_EventHandler();
    public event StaminaUI_EventHandler DecreaseStaminaEvent;
    public event StaminaUI_EventHandler IncreaseStaminaEvent;
    public event StaminaUI_EventHandler IconColorEvent;

    [SerializeField] private Equipment_EnergyDrink drinkController = null;

    [SerializeField] private AudioClip[] WalkSound = null;
    [SerializeField] private AudioSource SoundPlayer = null;

    [SerializeField] private float increaseValue = 0.005f;
    [SerializeField] private float decreaseValue = 0.0005f;

    [SerializeField] private LayerMask layerMask;

    private bool isMove = false;
    private bool useDrink = false;
    private bool isRun = false;
    private bool isJump = false;
    private bool canJump = true;

    [SerializeField] private float stamina;
    public float Stamina
    {
        get => stamina;
        set => stamina = Mathf.Clamp(value, 0, 10f);
    }
    [SerializeField] private float jumpForce = 0;
    [SerializeField] private float recoverTime = 0;

    private float h = 0;
    private float v = 0;
    private float maxSpeed => SetMoveSpeed();

    private readonly float jumpCoolDown = 1f;
    private float currentJumpCoolDown = 0;

    private float groundRayLength = 1.5f;
    private float staminaBuffTime = 10f;

    [SerializeField] private float maxSlopeAngle = 0;
    private float currentSlope = 0;

    private readonly float walkSoundPeriod = 0.6f;
    private readonly float runSoundPeriod = 0.27f;
    private float currentSoundTime = 0;

    private Rigidbody rb = null;
    private CapsuleCollider theCapsule = null;

    private RaycastHit groundRay;
    private RaycastHit slopeRay;
    private Vector3 lastPos = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 boxCastSize = new Vector3(0.3f, 0.05f, 0.3f);

    private ItemManager itemManager = null;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        theCapsule = GetComponent<CapsuleCollider>();
        itemManager = GetComponentInChildren<ItemManager>();
        drinkController = GetComponentInChildren<Equipment_EnergyDrink>(true);
        drinkController.drinkEvent += StaminaBuff;
    }
    private void Start()
    {
        Stamina = 1f;
        currentJumpCoolDown = jumpCoolDown;
    }
    void Update()
    {
        CalculateMoveValue();
        ControlMaxSpeed();
        PlayWalkAnimation();
        PlayFootStepSound();
        CalculateJumpCoolDown();
        IconColorEvent?.Invoke();


        if (useDrink) Stamina = 1;

        if (isGround())
        {
            Run();
            TryJump();

        }

    }


    private void CalculateMoveValue() // 이동에 필요한 값 계산
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        Vector3 horizontal = transform.right * h;
        Vector3 vertical = transform.forward * v;
        moveDirection = (horizontal + vertical).normalized;
        rb.useGravity = !isSlope();

        if (isGround())
        {
            rb.drag = 5f;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void ControlMaxSpeed() // 최대 속도 제한
    {
        Vector3 currentXZ_Speed = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (currentXZ_Speed.magnitude > maxSpeed)
        {
            Vector3 velXZ = currentXZ_Speed.normalized * maxSpeed;
            rb.velocity = new Vector3(velXZ.x, rb.velocity.y, velXZ.z);
        }
    }

    private void PlayWalkAnimation() // 걸을 때 애니메이션 재생
    {

        if (itemManager.currentEquipAnim == null) return;

        if (isMove && isGround())
        {
            itemManager.currentEquipAnim.SetBool("Walk", true);

            if (isRun)
            {
                itemManager.currentEquipAnim.SetBool("Run", true);
            }
            else
            {
                itemManager.currentEquipAnim.SetBool("Run", false);

            }

        }
        else
        {
            itemManager.currentEquipAnim.SetBool("Walk", false);
            itemManager.currentEquipAnim.SetBool("Run", false);
        }
    }
    private void PlayFootStepSound() // 발소리 재생
    {
        if (isMove && isGround())
        {

            if (isRun)
            {
                currentSoundTime += Time.deltaTime;
                if (currentSoundTime >= runSoundPeriod)
                {
                    currentSoundTime = 0;
                    SoundPlayer.clip = WalkSound[UnityEngine.Random.Range(0, WalkSound.Length)];
                    SoundPlayer.Play();

                }
            }
            else
            {

                currentSoundTime += Time.deltaTime;
                if (currentSoundTime >= walkSoundPeriod)
                {
                    currentSoundTime = 0;
                    SoundPlayer.clip = WalkSound[UnityEngine.Random.Range(0, WalkSound.Length)];
                    SoundPlayer.Play();

                }



            }

        }
    }

    private void Run() // 달리기
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {

            if (Stamina > 0)
            {
                if (!useDrink && DecreaseStaminaEvent != null)
                {
                    DecreaseStaminaEvent();
                }
                isRun = true;
                Stamina -= decreaseValue * Time.deltaTime;
                recoverTime = 0;
            }
            else if (Stamina == 0) //스태미너가 0이 됐을때
            {
                isRun = false;
                recoverTime = 0;
            }


        }
        else
        {
            isRun = false;
            if (Stamina > 0)
            {
                recoverTime += Time.deltaTime;
            }
            else if (Stamina == 0)
            {
                recoverTime += 0.003f * Time.deltaTime;
            }
        }
        if (recoverTime > 1)
        {
            IncreaseStaminaEvent?.Invoke();
            Stamina += increaseValue * Time.deltaTime;
        }
    }


    private void FixedUpdate()
    {

        Move();
        MoveCheck();
        if (isJump) Jump();

    }

    #region 점프 코드
    private void TryJump()
    {
        if (Input.GetKey(KeyCode.Space) && canJump && isGround() && currentSlope < maxSlopeAngle)
        {
            isJump = true;

        }
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        canJump = false;
        isJump = false;
    }

    private void CalculateJumpCoolDown()
    {
        if (!canJump)
        {
            currentJumpCoolDown -= Time.deltaTime;
            if (currentJumpCoolDown <= 0)
            {
                currentJumpCoolDown = jumpCoolDown;
                canJump = true;
            }

        }

    }
    #endregion

    #region 경사로 관련 코드
    private bool isSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeRay, theCapsule.height * 0.5f + 0.5f, layerMask))
        {
            currentSlope = Vector3.Angle(Vector3.up, slopeRay.normal);
            if (currentSlope > 0 && currentSlope < maxSlopeAngle)
            {
                return true;
            }

        }

        return false;


    }

    private Vector3 GetDirectionOnSlope() // 경사로에서의 이동 방향 계산
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeRay.normal).normalized;
    }
    #endregion
    private void Move()
    {
        if (isSlope())
        {
            rb.AddForce(GetDirectionOnSlope() * maxSpeed * SetAcceleration(), ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }

        }
        else
        {

            rb.AddForce(moveDirection * maxSpeed * SetAcceleration(), ForceMode.Force);
        }

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

    private bool isGround()
    {
        if (Physics.BoxCast(theCapsule.bounds.center, boxCastSize / 2, Vector3.down, out groundRay, Quaternion.identity, groundRayLength, -1, QueryTriggerInteraction.Ignore))
        {
            return true;
        }

        return false;

    }

    private void StaminaBuff() // 드링크 아이템 사용 시 스태미너 무한 버프
    {
        useDrink = true;
        StopCoroutine(EndStaminaBuff());
        StartCoroutine(EndStaminaBuff());
    }

    private IEnumerator EndStaminaBuff()
    {
        yield return new WaitForSeconds(staminaBuffTime);
        useDrink = false;
    }

    private float SetAcceleration()
    {
        if (isSlope())
        {
            return 20f;
        }
        return 10f;
    }
    private float SetMoveSpeed()
    {
        if (isGround() && isRun)
        {
            return 13f;
        }
        else if (stamina == 0)
        {
            return 3f;
        }

        return 7f;
    }
}
