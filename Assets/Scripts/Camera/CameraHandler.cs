using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Transform _posForCamera, _posLeftIncline, _posRightIncline;
    private Transform _currentPoint;
    public GameObject _visible;
    public bool isIncline;

    private void Start()
    {
        OffIncline();
    }
    private void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        transform.position = _currentPoint.position;
    }
    public void InclineLeft()
    {
        _currentPoint = _posLeftIncline;
        _visible.transform.rotation = Quaternion.Euler(0, 0, 45);
        isIncline = true;
    }

    public void InclineRight()
    {
        _currentPoint = _posRightIncline;
        _visible.transform.rotation = Quaternion.Euler(0, 0, -45);
        isIncline = true;
    }

    public void OffIncline()
    {
        _currentPoint = _posForCamera;
        _visible.transform.rotation = Quaternion.Euler(0, 0, 0);
        isIncline = false;
    }
}
