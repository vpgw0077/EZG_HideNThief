using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyScriptable/CreateEnemyData", order = int.MaxValue)]

public class EnemyData : ScriptableObject
{
    [SerializeField] private float viewDistance; // 가시 거리
    public float ViewDistance { get { return viewDistance; } }


    [SerializeField] private float wanderRadius; // 정찰 반경
    public float WanderRadius { get { return wanderRadius; } }

    [SerializeField] private float wanderSpeed; // 정찰 속도
    public float WanderSpeed { get { return wanderSpeed; } }

    [SerializeField] private float chaceSpeed; // 추격 속도
    public float ChaceSpeed { get { return chaceSpeed; } }

    [SerializeField] private float fov; // 시야 각도
    public float Fov { get { return fov; } }

    [SerializeField] private float maxChaseDistance; // 최대 추적 거리
    public float MaxChaseDistance { get { return maxChaseDistance; } }
}
