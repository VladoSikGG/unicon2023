using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private PlayerCamera _camera;

    //надо это запихнуть в конструктор оружий
    [SerializeField] private float _recoilX, _recoilY;
    [SerializeField] private float _range;
    [SerializeField] private float _delayFire;
    [SerializeField] private bool _canFire;

    private void Update()
    {
        if (Input.GetMouseButton(0) && _canFire)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        _camera.Recoil(_recoilX, _recoilY);
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, _range))
        {
            Debug.Log("Fire");
        }
        _canFire = false;
        StartCoroutine("FireDelay");
    }

    IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(_delayFire);
        _canFire = true;
    }
}
