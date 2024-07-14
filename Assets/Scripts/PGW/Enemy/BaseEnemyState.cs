using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class BaseEnemyState<T> : MonoBehaviour
{
    public abstract void EnterState(T entity);
    public abstract IEnumerator ExecuteState(T entity);
    public abstract void ExitState();

}
