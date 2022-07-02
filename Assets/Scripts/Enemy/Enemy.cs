using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    WaltToBuilding,
    WalkToUnit,
    Attack
}

public class Enemy : MonoBehaviour
{
    public EnemyState CurrentEnemyState;

/*    public int Health;*/
    public Building TargetBuilding;
    public Unit TargetUnit;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;
    public NavMeshAgent NavMeshAgent;

    public float AttackPeriod = 1f;
    private float _timer;
    public UnitAnimator UnitAnimator;

/*    private int _maxHealth;

    public GameObject HealthBar;
    private HealthBar _healthBar;*/

    void Start()
    {
        SetState(EnemyState.Idle);
/*        _maxHealth = Health;
        //GameObject healthBar = Instantiate(HealthBarPrefab);
        _healthBar = HealthBar.GetComponent<HealthBar>();*/
        //_healthBar.Setup(transform);
    }

    void Update()
    {
        if (CurrentEnemyState == EnemyState.Idle)
        {
            FindClosestBuilding();
            if(TargetBuilding)
            {
                SetState(EnemyState.WaltToBuilding);
            }
            FindClosestUnit();
        }
        else if (CurrentEnemyState == EnemyState.WaltToBuilding)
        {
            FindClosestUnit();
            if (TargetBuilding == null)
            {
                SetState(EnemyState.Idle);
            }

            float distance = Vector3.Distance(transform.position, TargetBuilding.transform.position);
            if (distance < DistanceToAttack)
            {
                SetState(EnemyState.Attack);
            }

        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {
            if (TargetUnit)
            {
                NavMeshAgent.SetDestination(TargetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanceToFollow)
                {
                    SetState(EnemyState.WaltToBuilding);
                }
                if (distance < DistanceToAttack)
                {
                    SetState(EnemyState.Attack);
                }
            } 
            else
            {
                SetState(EnemyState.WaltToBuilding);
            }
        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            if (TargetUnit)
            {
                NavMeshAgent.SetDestination(TargetUnit.transform.position);
                NavMeshAgent.stoppingDistance = 2;
                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanceToAttack)
                {
                    SetState(EnemyState.WalkToUnit);
                }
                _timer += Time.deltaTime;
                if (_timer > AttackPeriod)
                {
                    _timer = 0;
                 //   UnitAnimator.Attack();
                    TargetUnit.TakeDamage(1);
                }
            } 
            else if (TargetBuilding)
            {
                FindClosestUnit();
                _timer += Time.deltaTime;
                if (_timer > AttackPeriod)
                {
                    _timer = 0;
                    //UnitAnimator.Attack();
                    TargetBuilding.TakeDamage(1);
                }
                float distance = Vector3.Distance(transform.position, TargetBuilding.transform.position);
                if (distance > DistanceToAttack)
                {
                    SetState(EnemyState.WaltToBuilding);
                }
            }
            else
            {
                SetState(EnemyState.WaltToBuilding);
                NavMeshAgent.stoppingDistance = 0.5f;
            }
        }
    }

    public void SetState(EnemyState enemyState)
    {
        CurrentEnemyState = enemyState;

        if (CurrentEnemyState == EnemyState.Idle)
        {
            //UnitAnimator.Idle();
        }
        else if (CurrentEnemyState == EnemyState.WaltToBuilding)
        {
            //UnitAnimator.Walk();

            FindClosestBuilding();
            if (TargetBuilding)
            {
                NavMeshAgent.SetDestination(TargetBuilding.transform.position);
            }
            else
            {
                SetState(EnemyState.Idle);
            }
        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {
            //UnitAnimator.Walk();
        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            //UnitAnimator.Attack();
            _timer = 0;
        }
    }

    public void FindClosestBuilding()
    {
        Building[] allBuildings = FindObjectsOfType<Building>();
        float minDistance = Mathf.Infinity;
        Building closestBuilding = null;

        for (int i = 0; i < allBuildings.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allBuildings[i].transform.position);
            if(distance < minDistance && allBuildings[i].CurrentBuildingState == BuildingState.Builded)
            {
                minDistance = distance;
                closestBuilding = allBuildings[i];
            }
        }

        TargetBuilding = closestBuilding;
    }

    public void FindClosestUnit()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        float minDistance = Mathf.Infinity;
        Unit closestUnit = null;

        for (int i = 0; i < allUnits.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestUnit = allUnits[i];
            }
        }

        if (minDistance < DistanceToFollow)
        {
            TargetUnit = closestUnit;
            SetState(EnemyState.WalkToUnit);
        }
    }

/*    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);
        if (Health <= 0)
        {
            //Die
            Destroy(gameObject);
        }
    }*/

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
