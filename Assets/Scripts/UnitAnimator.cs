using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class UnitAnimator : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Idle()
    {
        _animator.SetBool("Walk", false);
    }

    public void Walk()
    {
        _animator.SetBool("Walk", true);
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    public void Death()
    {
        _animator.SetTrigger("Death");

        Unit unit = GetComponentInParent<Unit>();
        Enemy enemy = GetComponentInParent<Enemy>();
        NavMeshAgent navMeshAgent = GetComponentInParent<NavMeshAgent>();
        EnemyHealth enemyHealth = GetComponentInParent<EnemyHealth>();
        if (unit)
        {
            Destroy(unit);
        }
        if (enemy)
        {
            Destroy(enemy);
        }
        if (enemyHealth)
        {
            Destroy(enemyHealth);
        }
        if (navMeshAgent)
        {
            Destroy(navMeshAgent);
        }
    }

    public void DestoyEvent()
    {
        Destroy(transform.parent.gameObject);
    }
}
