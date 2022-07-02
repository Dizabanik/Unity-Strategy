using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BuildingState
{
    Builded,
    Selected
}

public class Building : SelectableObject
{
    public int Price;
    public int Xsize = 3;
    public int Zsize = 3;

    private Color _startColor;
    public Renderer[] Renderers;

    public GameObject MenuObject;

    private BuildingState _currentBuildingState = BuildingState.Selected;

    public BuildingState CurrentBuildingState {
        get
        {
            return _currentBuildingState;
        }

        set
        {
            _currentBuildingState = value;
            if (_currentBuildingState == BuildingState.Builded)
            {
                Builded();
            }
        }
    }

    private NavMeshObstacle _navMeshObstacle;

    public int Health;
    private int _maxHealth;

    public GameObject HealthBar;
    private HealthBar _healthBar;

    public override void Start()
    {
        base.Start();
        Unselect();
        _navMeshObstacle = GetComponentInChildren<NavMeshObstacle>();
        _navMeshObstacle.enabled = false;

        _maxHealth = Health;
        _healthBar = HealthBar.GetComponent<HealthBar>();
    }

    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        _startColor = Renderers[0].material.color;
    }

    private void OnDrawGizmos()
    {
        float cellSize = FindObjectOfType<BuildingPlacer>().CellSize;

        for (int x = 0; x < Xsize; x++)
        {
            for (int z = 0; z < Zsize; z++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * cellSize, new Vector3(1, 0, 1) * cellSize);
            }
        }
    }

    private void OnDestroy()
    {
        Management management = FindObjectOfType<Management>();
        if (management)
        {
            FindObjectOfType<Management>().Unselect(this);
        }
        if (_healthBar)
        {
            Destroy(_healthBar.gameObject);
        }
    }

    public override void Select()
    {
        base.Select();
        MenuObject.SetActive(true);
    }

    public override void Unselect()
    {
        base.Unselect();
        MenuObject.SetActive(false);
    }

    public void DisplayUnacceptablePosition()
    {

    }

    public void DisplayAcceptablePosition()
    {

    }

    public virtual void Builded()
    {
        FindObjectOfType<Resources>().Money -= Price;
        _navMeshObstacle.enabled = true;
    }
}



