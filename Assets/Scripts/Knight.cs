using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    Idle,
    WaltToPoint,
    WalkToEnemy,
    Attack
}

public class Knight : Unit
{
    public UnitState CurrentUnitState;

    public Vector3 TargetPoint;
    public EnemyHealth TargetEnemy;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f; 

    public float AttackPeriod = 1f;
    private float _timer;

    public override void Start()
    {
        base.Start();
        SetState(UnitState.WaltToPoint);
    }

    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);
        SetState(UnitState.WaltToPoint);
    }

    void Update()
    {
        if (CurrentUnitState == UnitState.Idle)
        {
            FindClosestEnemy();
        }
        else if (CurrentUnitState == UnitState.WaltToPoint)
        {
            FindClosestEnemy();
            if (IsTargetPointReached())
            {
                SetState(UnitState.Idle);
            }
        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {
            if (TargetEnemy)
            {
                NavMeshAgent.SetDestination(TargetEnemy.transform.position);
                float distance = Vector3.Distance(transform.position, TargetEnemy.transform.position);
                if (distance > DistanceToFollow)
                {
                    SetState(UnitState.WaltToPoint);
                }
                if (distance < DistanceToAttack)
                {
                    SetState(UnitState.Attack);
                }
            }
            else
            {
                SetState(UnitState.Idle);
            }
        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            if (TargetEnemy)
            {
                NavMeshAgent.SetDestination(TargetEnemy.transform.position);
                NavMeshAgent.stoppingDistance = 2;
                float distance = Vector3.Distance(transform.position, TargetEnemy.transform.position);
                if (distance > DistanceToAttack)
                {
                    SetState(UnitState.WalkToEnemy);
                }
                _timer += Time.deltaTime;
                if (_timer > AttackPeriod)
                {
                    _timer = 0;
                    //UnitAnimator.Attack();
                    TargetEnemy.TakeDamage(1);
                }
            }
            else
            {
                SetState(UnitState.WaltToPoint);
                NavMeshAgent.stoppingDistance = 0.5f;
            }
        }
    }

    public void SetState(UnitState unitState)
    {
        CurrentUnitState = unitState;

        if (CurrentUnitState == UnitState.Idle)
        {
            Debug.Log("Idle");
            //UnitAnimator.Idle();
        }
        else if (CurrentUnitState == UnitState.WaltToPoint)
        {
            Debug.Log("Walk");
            //UnitAnimator.Walk();
        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {
            Debug.Log("Walk");
            //UnitAnimator.Walk();
            ResetTargetPoint();
        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            //UnitAnimator.Attack();
            _timer = 0;
            ResetTargetPoint();
        }
    }

    public void FindClosestEnemy()
    {
        EnemyHealth[] allEnemies = FindObjectsOfType<EnemyHealth>();
        float minDistance = Mathf.Infinity;
        EnemyHealth closestEnemy = null;

        for (int i = 0; i < allEnemies.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allEnemies[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = allEnemies[i];
            }
        }

        if (minDistance < DistanceToFollow)
        {
            TargetEnemy = closestEnemy;
            SetState(UnitState.WalkToEnemy);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToFollow);
    }
#endif
}
