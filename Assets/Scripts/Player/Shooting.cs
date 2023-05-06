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
    [SerializeField] private ParticleSystem _smokeFX;
    [SerializeField] private Transform _pointForFX;
    [SerializeField] private Animation _anim;
    [SerializeField] private int _ammo = 30;
    [SerializeField] const int ONE_CYCLE = 30;
    [SerializeField] GameObject _bulletHole;

    [SerializeField] private bool _canFire;
    [SerializeField] private KeyCode _reloadKey;
    [SerializeField] private ParticleSystem _hitEnemy;

    private void Update()
    {
        if (Input.GetMouseButton(0) && _canFire)
        {
            Shoot();
        }

        if (Input.GetKeyDown(_reloadKey))
        {
            Reloading();
        }
    }

    private void Shoot()
    {
        if (_ammo > 0)
        {
            _camera.Recoil(_recoilX, _recoilY);
            RaycastHit hit;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, _range))
            {
                if (hit.collider.tag == "Enemy")
                {
                    Debug.Log("Hit Enemy");
                    BotController controller = hit.collider.GetComponent<BotController>();
                    Instantiate(_hitEnemy, hit.point, Quaternion.identity);
                    controller.EnemyRunAway();
                    controller.Damage(10);
                }
                else
                {
                    GameObject hitHole = Instantiate(_bulletHole, hit.point + (hit.normal * 0.025f), Quaternion.FromToRotation(Vector3.up, hit.normal));
                    Debug.Log("Fire " + hit.transform.name);
                    Destroy(hitHole, 20f);
                }
                
            }
            _ammo--;
            _smokeFX.Play();
            _anim.Play("Fire");
            _canFire = false;
            StartCoroutine("FireDelay");
        }
    }

    IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(_delayFire);
        _canFire = true;
    }

    public void Reloading()
    {
        _anim.Play("Reload");
        _ammo = ONE_CYCLE;
    }
}
