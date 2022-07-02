using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    public NavMeshAgent NavMeshAgent;
    public int Price;
    public int Health;
    private int _maxHealth;

    public GameObject HealthBar;
    private HealthBar _healthBar;

    public GameObject NavigationIndicator;

    public UnitAnimator UnitAnimator;

    public override void Start()
    {
        base.Start();
        _maxHealth = Health;
        //GameObject healthBar = Instantiate(HealthBarPrefab);
        _healthBar = HealthBar.GetComponent<HealthBar>();
        //_healthBar.Setup(transform);

        NavigationIndicator.SetActive(false);
        NavigationIndicator.transform.parent = null;
    }


    private void LateUpdate()
    {
        if (IsTargetPointReached())
        {
            ResetTargetPoint();
        }
    }

    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);

        NavMeshAgent.SetDestination(point);

        NavigationIndicator.SetActive(true);
        NavigationIndicator.transform.position = new Vector3(point.x, NavigationIndicator.transform.position.y, point.z);
    }

    public void ResetTargetPoint()
    {
        //navMeshAgent.ResetPath();
        //onTargetPointReached.Invoke();
        NavigationIndicator.SetActive(false);
    }

    public bool IsTargetPointReached()
    {
        if (NavMeshAgent.pathPending || NavMeshAgent.remainingDistance > NavMeshAgent.stoppingDistance)
        {
            return false;
        }

        return !NavMeshAgent.hasPath || NavMeshAgent.velocity.sqrMagnitude == 0f;
    }

    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);
        if(Health <= 0)
        {
            //Die
            //Destroy(gameObject);
            UnitAnimator.Death();
        }
    }

    private void OnDestroy()
    {
        Management management = FindObjectOfType<Management>();
        if (management)
        {
            FindObjectOfType<Management>().Unselect(this);
        }
    }
}
