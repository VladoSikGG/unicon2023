using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Transform _posForCamera;

    void Update()
    {
        transform.position = _posForCamera.position;
    }
}
