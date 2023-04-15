using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private PlayerCamera _camera;

    [SerializeField] private float _recoilX, _recoilY;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _camera.Recoil(_recoilX,_recoilY);    
        }
    }
}
