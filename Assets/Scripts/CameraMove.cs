using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera RaycastCamera;
    private Vector3 _startPoint;
    private Vector3 _cameraStartPosition;
    private Plane _plane;
    private Camera _mainCamera;
    private bool _isStartMoving = false;

    void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
        _mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);
        if (Input.GetMouseButtonDown(2))
        {
            _startPoint = point;
            _cameraStartPosition = _mainCamera.transform.position;
            _isStartMoving = true;
        }

        if (Input.GetMouseButton(2) && _isStartMoving)
        {
            Vector3 offset = point - _startPoint;
            _mainCamera.transform.position = _cameraStartPosition - offset;
        }

        if (Input.GetMouseButtonUp(2))
        {
            _isStartMoving = false;
        }

        _mainCamera.transform.position -= new Vector3(0, Input.mouseScrollDelta.y, 0);
        RaycastCamera.transform.position -= new Vector3(0, Input.mouseScrollDelta.y, 0);

        ClampValue(_mainCamera);
        ClampValue(RaycastCamera);
/*        _mainCamera.transform.position = new Vector3(
            Mathf.Clamp(_mainCamera.transform.position.x, -45, 45),
            Mathf.Clamp(_mainCamera.transform.position.y, 8, 14),
            Mathf.Clamp(_mainCamera.transform.position.z, -51, 39)
        );
        RaycastCamera.transform.position = new Vector3(
            Mathf.Clamp(RaycastCamera.transform.position.x, -45, 45),
            Mathf.Clamp(RaycastCamera.transform.position.y, 8, 14),
            Mathf.Clamp(RaycastCamera.transform.position.z, -51, 39)
        );*/

        /*_mainCamera.transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
        RaycastCamera.transform.Translate(0f, 0f, Input.mouseScrollDelta.y);

        _mainCamera.transform.position = new Vector3(
            _mainCamera.transform.position.x,
            Mathf.Clamp(_mainCamera.transform.position.y, 8, 14),
            Mathf.Clamp(_mainCamera.transform.position.z, -10, -6)
        );

        RaycastCamera.transform.position = new Vector3(
            RaycastCamera.transform.position.x,
            Mathf.Clamp(RaycastCamera.transform.position.y, 8, 14),
            Mathf.Clamp(RaycastCamera.transform.position.z, -10, -6)
        );*/
    }

    void ClampValue(Camera camera)
    {
        camera.transform.position = new Vector3(
            Mathf.Clamp(camera.transform.position.x, -45, 45),
            Mathf.Clamp(camera.transform.position.y, 8, 14),
            Mathf.Clamp(camera.transform.position.z, -51, 39)
        );
    }
}
