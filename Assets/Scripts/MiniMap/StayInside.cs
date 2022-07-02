using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInside : MonoBehaviour
{
    private GameObject _minimapCam;
    private float _minimapSize;
    private Vector3 _tempV3;

    private void Start()
    {
        _minimapCam = GameObject.FindGameObjectWithTag("MinimapCamera");
    }
    // Update is called once per frame
    void Update()
    {
        _tempV3 = transform.parent.transform.position;
        _tempV3.y = transform.position.y;
        transform.position = _tempV3;
        _minimapSize = _minimapCam.GetComponent<Camera>().orthographicSize;
    }

    private void LateUpdate()
    {
        /*float minX = _minimapCam.transform.position.x - _minimapSize + transform.localScale.x / 2;
        float maxX = _minimapCam.transform.position.x + _minimapSize - transform.localScale.x / 2;
        float minZ = _minimapCam.transform.position.z - _minimapSize + transform.localScale.z / 2;
        float maxZ = _minimapCam.transform.position.z + _minimapSize - transform.localScale.z / 2;*/
        float minX = _minimapCam.transform.position.x - _minimapSize;
        float maxX = _minimapCam.transform.position.x + _minimapSize;
        float minZ = _minimapCam.transform.position.z - _minimapSize;
        float maxZ = _minimapCam.transform.position.z + _minimapSize;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            transform.position.y,
            Mathf.Clamp(transform.position.z, minZ, maxZ)
        );
    }
}
