using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> 
{
    private T ownerEntitiy; // StateMachine의 소유주
    private BaseEnemyState<T> currentState; // 현재 상태

    public void Setup(T owner, BaseEnemyState<T> entryState)
    {
        ownerEntitiy = owner;
        currentState = null;

        // entryState 상태로 변경
        ChangeState(entryState);
    }

    public void ChangeState(BaseEnemyState<T> newState)
    {
        // 새로 바꾸려는 상태가 비어있으면 상태를 바꾸지 않는다
        if (newState == null) return;

        if(currentState != null)
        {
            currentState.ExitState();
        }
        // 새로운 상태로 변경하고 새로 바뀐 상태의 Enter 메소드 호출
        currentState = newState;
        currentState.EnterState(ownerEntitiy);

    }

}
