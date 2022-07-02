using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public float CellSize = 1f;
    public Camera RaycastCamera;

    private Plane _plane;

    public Building CurrentBuilding;

    public Dictionary<Vector2Int, Building> BuildingsDictionary = new Dictionary<Vector2Int, Building>();

    void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }
    void Update()
    {
        if (CurrentBuilding == null)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && CurrentBuilding != null)
        {
            Destroy(CurrentBuilding.gameObject);
            CurrentBuilding = null;
        }
        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / CellSize;

        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);

        CurrentBuilding.transform.position = new Vector3(x, 0, z) * CellSize;

        if (CheckAllow(x, z, CurrentBuilding))
        {
            CurrentBuilding.DisplayAcceptablePosition();
            if (Input.GetMouseButtonDown(0))
            {
                //CurrentBuilding.DisplayBuildPosition();
                InstallBuilding(x, z, CurrentBuilding);
                CurrentBuilding = null;
            }
        } 
        else
        {
            CurrentBuilding.DisplayUnacceptablePosition();
        }
    }

    private bool CheckAllow(int xPosition, int zPosition, Building building)
    {
        for (int x = 0; x < building.Xsize; x++)
        {
            for (int z = 0; z < building.Zsize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                if (BuildingsDictionary.ContainsKey(coordinate))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void InstallBuilding(int xPosition, int zPosition, Building building)
    {
        for (int x = 0; x < building.Xsize; x++)
        {
            for (int z = 0; z < building.Zsize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                BuildingsDictionary.Add(coordinate, building);
            }
        }

        building.CurrentBuildingState = BuildingState.Builded;
        //building.Builded();

        foreach (var item in BuildingsDictionary)
        {
            Debug.Log(item);
        }
    }

    public void CreateBuilding(GameObject buildingPrefab)
    {
        GameObject newBuilding = Instantiate(buildingPrefab);
        CurrentBuilding = newBuilding.GetComponent<Building>();
    }
}
