using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonEnemyState
{
    public class StateWander : BaseEnemyState<CommonEnemyAgent>
    {
        public override void EnterState(CommonEnemyAgent entity)
        {
            StartCoroutine(ExecuteState(entity));
        }
        public override IEnumerator ExecuteState(CommonEnemyAgent entity)
        {
            entity.CurrentCheckTime = 0;
            entity.agent.speed = entity.EnemyData.WanderSpeed;
            entity.WanderPoint = entity.RandomWanderPoint();
            while (true)
            {

                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(entity.WanderPoint.x, entity.WanderPoint.z)) < 1f)
                {

                    entity.ChangeState(CommonEnemyStateList.LookAround);
                }
                else
                {
                    entity.agent.SetDestination(entity.WanderPoint);
                    entity.CheckStuck();
                }

                yield return null;

            }
        }
        public override void ExitState()
        {
            StopAllCoroutines();
        }
    }

    public class StateLookAround : BaseEnemyState<CommonEnemyAgent>
    {
        public override void EnterState(CommonEnemyAgent entity)
        {
            StartCoroutine(ExecuteState(entity));
        }
        private WaitForSeconds lookArondTime = new WaitForSeconds(3.5f);
        public override IEnumerator ExecuteState(CommonEnemyAgent entity)
        {
            if (entity.agent.enabled == true)
            {
                entity.agent.isStopped = true;
                entity.Anim.SetBool("LookAround", true);
                yield return lookArondTime;
                entity.Anim.SetBool("LookAround", false);
                entity.agent.isStopped = false;

                entity.ChangeState(CommonEnemyStateList.Wander);

            }
        }
        public override void ExitState()
        {
            StopAllCoroutines();
        }
    }

    public class StateChase : BaseEnemyState<CommonEnemyAgent>
    {
        public override void EnterState(CommonEnemyAgent entity)
        {
            StartCoroutine(ExecuteState(entity));
        }
        public override IEnumerator ExecuteState(CommonEnemyAgent entity)
        {
            if (entity.PreviousState == CommonEnemyStateList.LookAround)
            {
                entity.agent.isStopped = false;
                entity.Anim.SetBool(entity.PreviousState.ToString(), false);
            }

            GameController.instance.AddAwaredPolice(entity);

            entity.agent.speed = entity.EnemyData.ChaceSpeed;
            entity.Anim.SetBool("Chase", true);

            while (true)
            {
                entity.agent.SetDestination(entity.Player.transform.position);
                entity.CheckDistancePlayerAndEnemy();
                yield return null;

            }
        }
        public override void ExitState()
        {
            StopAllCoroutines();
        }
    }

    public class StateStun : BaseEnemyState<CommonEnemyAgent>
    {
        private string stunAnimName = "Enemy_Stun";
        private WaitForSeconds stunTime = new WaitForSeconds(5f);
        public override void EnterState(CommonEnemyAgent entity)
        {
            StartCoroutine(ExecuteState(entity));
        }
        public override IEnumerator ExecuteState(CommonEnemyAgent entity)
        {
            if (entity.Anim.GetCurrentAnimatorStateInfo(0).IsName(stunAnimName)) // 현재 스턴 상태일 때 섬광탄을 또 맞을 시 애니메이션 초기화
            {
                entity.Anim.Play(stunAnimName, -1, 0.1f);
            }

            if (entity.PreviousState != entity.CurrentState && entity.PreviousState != CommonEnemyStateList.Wander)
            {
                entity.Anim.SetBool(entity.PreviousState.ToString(), false);
            }

            if (GameController.instance.awarePoliceList.Contains(entity))
            {
                GameController.instance.RemoveAwaredPolice(entity);
            }

            entity.agent.enabled = false;
            entity.Anim.SetBool("Stun", true);
            yield return stunTime;
            entity.Anim.SetBool("Stun", false);
            entity.agent.enabled = true;

            entity.ChangeState(CommonEnemyStateList.LookAround);

        }
        public override void ExitState()
        {
            StopAllCoroutines();
        }
    }
}
