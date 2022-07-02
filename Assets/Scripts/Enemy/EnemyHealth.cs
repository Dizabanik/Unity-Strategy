using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyHealth : MonoBehaviour
{
    public int Health;
    private int _maxHealth;

    public GameObject HealthBar;
    private HealthBar _healthBar;
    public UnitAnimator UnitAnimator;

    public void Start()
    {
        _maxHealth = Health;
        _healthBar = HealthBar.GetComponent<HealthBar>();
    }

    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);
        if (Health <= 0)
        {
            if (UnitAnimator)
            {
                UnitAnimator.Death();
            } 
            else
            {
                Destroy(gameObject);
            }
        }
    }
}



