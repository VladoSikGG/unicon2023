using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;

    [Header("From Player")]
    [SerializeField] private Transform _orientation;


    [Header("Limitation")]
    [SerializeField] private float _minLimitX;
    [SerializeField] private float _maxLimitX;
    private float _offsetX, _offsetY;

    private float _xRotation, _yRotation;

    private void Start()
    {
       //lock cursor and hide him
       Cursor.lockState = CursorLockMode.Locked;
       Cursor.visible = false;
    }

    private void Update()
    {

        float mouseX = Time.deltaTime * Input.GetAxis("Mouse X") * _sensX * 50 + _offsetX;
        float mouseY = Time.deltaTime * Input.GetAxis("Mouse Y") * _sensY * 50 + _offsetY;

        _offsetX = 0;
        _offsetY = 0;

        _yRotation += mouseX;
        _xRotation -= mouseY;

        _xRotation = Mathf.Clamp(_xRotation, _minLimitX, _maxLimitX);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        _orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

    public void Recoil(float x, float y)
    {
        _offsetX = Random.Range(-x, x);
        _offsetY = Random.Range(0, y);
    }
}
