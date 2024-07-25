using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestEnemyState
{
    public class StateWander : BaseEnemyState<TestEnemyAgent>
    {
        public override void EnterState(TestEnemyAgent entity)
        {
            StartCoroutine(ExecuteState(entity));
        }
        public override IEnumerator ExecuteState(TestEnemyAgent entity)
        {
            entity.CurrentCheckTime = 0;
            entity.agent.speed = entity.EnemyData.WanderSpeed;
            entity.WanderPoint = entity.RandomWanderPoint();
            while (true)
            {

                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(entity.WanderPoint.x, entity.WanderPoint.z)) < 1f)
                {

                    entity.ChangeState(TestEnemyStateList.LookAround);
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

    public class StateLookAround : BaseEnemyState<TestEnemyAgent>
    {
        public override void EnterState(TestEnemyAgent entity)
        {
            StartCoroutine(ExecuteState(entity));
        }
        private WaitForSeconds lookArondTime = new WaitForSeconds(3.5f);
        public override IEnumerator ExecuteState(TestEnemyAgent entity)
        {
            if (entity.agent.enabled == true)
            {
                entity.agent.isStopped = true;
                entity.Anim.SetBool("LookAround", true);
                yield return lookArondTime;
                entity.Anim.SetBool("LookAround", false);
                entity.agent.isStopped = false;

                entity.ChangeState(TestEnemyStateList.Wander);

            }
        }
        public override void ExitState()
        {
            StopAllCoroutines();
        }
    }

    public class StateChase : BaseEnemyState<TestEnemyAgent>
    {
        public override void EnterState(TestEnemyAgent entity)
        {
            StartCoroutine(ExecuteState(entity));
        }
        public override IEnumerator ExecuteState(TestEnemyAgent entity)
        {
            if (entity.PreviousState == TestEnemyStateList.LookAround)
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
}
