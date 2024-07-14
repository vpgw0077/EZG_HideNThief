using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum CommonEnemyStateList
{
    Wander = 0,
    LookAround,
    Chase,
    Stun
}
public class CommonEnemyAgent : BaseEnemy , IRockRespond, IFlashBangRespond, IAlarmRespond
{
    private float blockCheckRayLength = 3f;
    private float maxStuckCheckTime = 3f;
    private bool isInSmoke = false;
    private Vector3 dir2Player = Vector3.zero;
    public GameObject Player { get; private set; }
    [SerializeField] private LayerMask smokeLayer;

    [SerializeField] private EnemyData enemyData = null;
    public EnemyData EnemyData
    {
        get { return enemyData; }
    }

    public Animator Anim { get; private set; }

    public float CurrentCheckTime { get; set; }

    public NavMeshAgent agent { get; private set; }
    public Vector3 WanderPoint { get; set; }

    private BaseEnemyState<CommonEnemyAgent>[] states = null;
    private StateMachine<CommonEnemyAgent> stateMachine = null;

    public CommonEnemyStateList CurrentState { get; private set; }
    public CommonEnemyStateList PreviousState { get; private set; }

    private void Awake()
    {
        Anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        agent.updateRotation = false;
        SetUpEnemy();
    }
    public override void SetUpEnemy()
    {
        states = new BaseEnemyState<CommonEnemyAgent>[4];
        states[0] = gameObject.AddComponent<CommonEnemyState.StateWander>();
        states[1] = gameObject.AddComponent<CommonEnemyState.StateLookAround>();
        states[2] = gameObject.AddComponent<CommonEnemyState.StateChase>();
        states[3] = gameObject.AddComponent<CommonEnemyState.StateStun>();

        stateMachine = new StateMachine<CommonEnemyAgent>();
        stateMachine.Setup(this, states[(int)CommonEnemyStateList.Wander]);
    }

    private void Update()
    {
        UpdateAgentRotation();
        SearchForPlayer();
    }
    public override void SearchForPlayer() // 플레이어 찾기
    {

        if (CurrentState == CommonEnemyStateList.Chase || CurrentState == CommonEnemyStateList.Stun || isInSmoke) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 3f, transform.forward, out hit, enemyData.ViewDistance, smokeLayer)) // 연막탄 안에 있는 플레이어를 감지하지 못하게
        {
            if (hit.transform.CompareTag("SmokeTrigger")) return;
        }

        dir2Player = Player.transform.position - transform.position;

        if (Vector3.Angle(transform.forward, dir2Player) > enemyData.Fov) return;

        if (Vector3.Distance(Player.transform.position, transform.position) > enemyData.ViewDistance) return;

        if (Physics.Linecast(transform.position, Player.transform.position, out hit)) // Raycast가 아닌 Linecast를 쓰는 이유는 방향은 필요없고 
        {                                                                                 // 시야 범위 내에 플레이어가 있는지만 확인하면 되니까
            if (hit.transform.CompareTag("Player"))
            {
                ChangeState(CommonEnemyStateList.Chase);
            }
        }
    }
    public Vector3 RandomWanderPoint() // 정찰 위치 지정
    {
        Vector3 randomPoint = (Random.insideUnitSphere * enemyData.WanderRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, enemyData.WanderRadius, NavMesh.AllAreas);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);

    }

    public void CheckStuck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 4f, transform.forward, out hit, blockCheckRayLength) ||
            agent.velocity.magnitude < agent.speed * 0.1f)
        {
            CurrentCheckTime += Time.deltaTime;
            if (CurrentCheckTime >= maxStuckCheckTime)
            {
                ChangeState(CommonEnemyStateList.Wander);

            }
        }
    }
    private void UpdateAgentRotation()
    {
        Vector3 direction = agent.desiredVelocity;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed());

        }
    }
    public void CheckDistancePlayerAndEnemy() // 추격 종료
    {
        if (Vector3.Distance(transform.position, Player.transform.position) > enemyData.MaxChaseDistance)
        {
            Anim.SetBool("Chase", false);
            GameController.instance.RemoveAwaredPolice(this);
            ChangeState(CommonEnemyStateList.Wander);

        }

    }
    private float RotationSpeed()
    {
        if (CurrentState == CommonEnemyStateList.Chase)
        {
            return 8f;
        }
        return 2f;
    }
    public void RespondToRock(Vector3 soundSpot) // 플레이어가 돌을 던졌을 때 어그로 끌림
    {
        WanderPoint = soundSpot;
    }
    public void RespondToFlashBang()
    {
        ChangeState(CommonEnemyStateList.Stun);
    }

    public void RespondToAlarm()
    {
        ChangeState(CommonEnemyStateList.Chase);
    }
    public void ChangeState(CommonEnemyStateList state)
    {
        PreviousState = CurrentState;
        CurrentState = state;
        stateMachine.ChangeState(states[(int)state]);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (CurrentState == CommonEnemyStateList.Chase && !isInSmoke)
        {
            if (collision.transform.CompareTag("Player"))
            {
                StartCoroutine(GameController.instance.GameOver(GameOverType.Defeat));
            }
        }


    }
    private void OnTriggerStay(Collider other)
    {
        if (isInSmoke) return;

        if (other.CompareTag("SmokeTrigger"))
        {
            isInSmoke = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SmokeTrigger"))
        {
            isInSmoke = false;
        }
    }

}
