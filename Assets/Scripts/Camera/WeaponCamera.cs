using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCamera : MonoBehaviour
{
    private PlayerCamera _mainCamera;
    void Start()
    {
        _mainCamera = FindObjectOfType<PlayerCamera>();
    }

    void Update()
    {
        transform.rotation = _mainCamera.transform.rotation;
    }
}
