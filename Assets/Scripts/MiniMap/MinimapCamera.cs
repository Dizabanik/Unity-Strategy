using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private void Start()
    {
        Camera targetCamera = GetComponent<Camera>();
        targetCamera.SetReplacementShader(Shader.Find("Unlit/Color"), "RenderType");
    }
}
