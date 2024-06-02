using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public enum EnemyState
{
    Wander,
    LookAround,
    Stun,
    Chase

}
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData = null;
    [SerializeField] private LayerMask smokeLayer;

    private EnemyState enemyState = EnemyState.Wander;
    private EnemyState previousState = EnemyState.Wander;

    private float stunTime = 5f;
    private float blockCheckRayLength = 3f;
    private float maxStuckCheckTime = 3f;
    private float currentCheckTime = 0;

    private GameObject player = null; // 추격 대상

    private bool isInSmoke = false;


    [SerializeField] private AudioClip[] walkSound;
    [SerializeField] private AudioSource soundPlayer;

    private Animator anim;

    private Vector3 wanderPoint = Vector3.zero; // 정찰 지점
    private Vector3 dir2Player = Vector3.zero; // 플레이어와의 방향

    private RaycastHit hit;


    private NavMeshAgent agent;
    private string stunAnimName = "Enemy_Stun";

    // Start is called before the first frame update
    void Awake()
    {

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        soundPlayer = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Start()
    {

        agent.updateRotation = false;
        wanderPoint = RandomWanderPoint();
        ChangeState(EnemyState.Wander);
    }

    // Update is called once per frame
    void Update()
    {

        SearchForPlayer();
        UpdateAgentRotation();
    }

    private void SearchForPlayer() // 플레이어 찾기
    {

        if (enemyState == EnemyState.Chase || enemyState == EnemyState.Stun || isInSmoke) return;

        if (Physics.Raycast(transform.position + transform.up * 3f, transform.forward, out hit, enemyData.ViewDistance, smokeLayer)) // 연막탄 안에 있는 플레이어를 감지하지 못하게
        {
            if (hit.transform.CompareTag("SmokeTrigger")) return;
        }

        dir2Player = player.transform.position - transform.position;

        if (Vector3.Angle(transform.forward, dir2Player) > enemyData.Fov) return;

        if (Vector3.Distance(player.transform.position, transform.position) > enemyData.ViewDistance) return;

        if (Physics.Linecast(transform.position, player.transform.position, out hit)) // Raycast가 아닌 Linecast를 쓰는 이유는 방향은 필요없고 
        {                                                                                 // 시야 범위 내에 플레이어가 있는지만 확인하면 되니까
            if (hit.transform.CompareTag("Player"))
            {
                ChangeState(EnemyState.Chase);
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
    private void CheckStuck()
    {

        if (Physics.Raycast(transform.position + transform.up * 4f, transform.forward, out hit, blockCheckRayLength) ||
            agent.velocity.magnitude < agent.speed * 0.1f)
        {
            currentCheckTime += Time.deltaTime;
            if (currentCheckTime >= maxStuckCheckTime) ChangeState(EnemyState.Wander);
        }
    }
    public void ChangeState(EnemyState newState)
    {
        StopAllCoroutines();
        previousState = enemyState;
        enemyState = newState;
        StartCoroutine(enemyState.ToString());
    }
    public void DrawAttention(Vector3 SoundSpot) // 플레이어가 돌을 던졌을 때 어그로 끌림
    {
        wanderPoint = SoundSpot;
    }


    #region enemyFSM
    private IEnumerator Wander() // 주변 정찰
    {
        currentCheckTime = 0;
        agent.speed = enemyData.WanderSpeed;
        wanderPoint = RandomWanderPoint();
        while (true)
        {

            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(wanderPoint.x, wanderPoint.z)) < 1f)
            {

                ChangeState(EnemyState.LookAround);
            }
            else
            {
                agent.SetDestination(wanderPoint);
                CheckStuck();
            }

            yield return null;

        }


    }

    private IEnumerator LookAround() // 두리번 거리는 애니메이션 실행
    {
        if (agent.enabled == true)
        {
            agent.isStopped = true;
            anim.SetBool("LookAround", true);
            yield return new WaitForSeconds(3.5f);
            anim.SetBool("LookAround", false);
            agent.isStopped = false;

            ChangeState(EnemyState.Wander);

        }

    }

    private IEnumerator Stun() // 스턴 상태
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stunAnimName)) // 현재 스턴 상태일 때 섬광탄을 또 맞을 시 애니메이션 초기화
        {
            anim.Play(stunAnimName, -1, 0.1f);
        }

        if (previousState != enemyState && previousState != EnemyState.Wander)
        {
            anim.SetBool(previousState.ToString(), false);
        }

        if (GameController.instance.awarePoliceList.Contains(this))
        {
            GameController.instance.RemoveAwaredPolice(this);
        }

        agent.enabled = false;
        anim.SetBool("Stun", true);
        yield return new WaitForSeconds(stunTime);
        anim.SetBool("Stun", false);
        agent.enabled = true;

        ChangeState(EnemyState.LookAround);


    }

    private IEnumerator Chase()
    {
        if (previousState == EnemyState.LookAround)
        {
            agent.isStopped = false;
            anim.SetBool(previousState.ToString(), false);
        }

        GameController.instance.AddAwaredPolice(this);

        agent.speed = enemyData.ChaceSpeed;
        anim.SetBool("Chase", true);

        while (true)
        {
            agent.SetDestination(player.transform.position);
            CheckDistancePlayerAndEnemy();
            yield return null;

        }


    }
    #endregion

    private void CheckDistancePlayerAndEnemy() // 추격 종료
    {
        if (Vector3.Distance(transform.position, player.transform.position) > enemyData.MaxChaseDistance)
        {
            anim.SetBool("Chase", false);
            GameController.instance.RemoveAwaredPolice(this);
            ChangeState(EnemyState.Wander);

        }

    }

    private Vector3 RandomWanderPoint() // 정찰 위치 지정
    {
        Vector3 randomPoint = (Random.insideUnitSphere * enemyData.WanderRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, enemyData.WanderRadius, NavMesh.AllAreas);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);


    }
    private float RotationSpeed()
    {
        if (enemyState == EnemyState.Chase)
        {
            return 8f;
        }
        return 2f;
    }
    private void FootStepSound()
    {
        soundPlayer.clip = walkSound[UnityEngine.Random.Range(0, walkSound.Length)];
        soundPlayer.Play();
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (enemyState == EnemyState.Chase && !isInSmoke)
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
