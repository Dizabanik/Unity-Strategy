using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraBoundary : MonoBehaviour
{
    private Camera _minimapCamera;
    private Camera _mainCamera;
    public float OffsetPositionZ;
    private void Start()
    {
        GameObject minimapCameraObject = GameObject.FindGameObjectWithTag("MinimapCamera");
        _minimapCamera = minimapCameraObject.GetComponent<Camera>();
        _mainCamera = Camera.main;
        OffsetPositionZ = transform.position.z - _mainCamera.transform.position.z;
        
    }
    void Update()
    {
        //transform.position = new Vector3(_mainCamera.transform.position.x, 10f, _mainCamera.transform.position.z + OffsetPositionZ);
        transform.localScale = new Vector3(
            _minimapCamera.orthographicSize / 25,
            _minimapCamera.orthographicSize / 25,
            _minimapCamera.orthographicSize / 25
        );
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
