using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Ray2D rayCast;
    NavMeshAgent navMeshAgent;

    public void Init()
    {
        rayCast = new Ray2D();
        navMeshAgent = this.gameObject.AddComponent<NavMeshAgent>();
    }
}
