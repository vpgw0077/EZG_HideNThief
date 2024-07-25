using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TestEnemyStateList
{
    Wander = 0,
    LookAround,
    Chase

}
public class TestEnemyAgent : BaseEnemy
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

    private BaseEnemyState<TestEnemyAgent>[] states = null;
    private StateMachine<TestEnemyAgent> stateMachine = null;

    public TestEnemyStateList CurrentState { get; private set; }
    public TestEnemyStateList PreviousState { get; private set; }

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
        states = new BaseEnemyState<TestEnemyAgent>[3];
        states[0] = gameObject.AddComponent<TestEnemyState.StateWander>();
        states[1] = gameObject.AddComponent<TestEnemyState.StateLookAround>();
        states[2] = gameObject.AddComponent<TestEnemyState.StateChase>();

        stateMachine = new StateMachine<TestEnemyAgent>();
        stateMachine.Setup(this, states[(int)TestEnemyStateList.Wander]);
    }

    private void Update()
    {
        UpdateAgentRotation();
        SearchForPlayer();
    }
    public override void SearchForPlayer() // 플레이어 찾기
    {

        if (CurrentState == TestEnemyStateList.Chase || isInSmoke) return;

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
                ChangeState(TestEnemyStateList.Chase);
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
                ChangeState(TestEnemyStateList.Wander);

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
            ChangeState(TestEnemyStateList.Wander);

        }

    }
    private float RotationSpeed()
    {
        if (CurrentState == TestEnemyStateList.Chase)
        {
            return 8f;
        }
        return 2f;
    }
    public void ChangeState(TestEnemyStateList state)
    {
        PreviousState = CurrentState;
        CurrentState = state;
        stateMachine.ChangeState(states[(int)state]);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (CurrentState == TestEnemyStateList.Chase && !isInSmoke)
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
