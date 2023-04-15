using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Transform _posForCamera, _posLeftIncline, _posRightIncline;
    private Transform _currentPoint;

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
    public void IclineLeft()
    {
        _currentPoint = _posLeftIncline;
    }

    public void IclineRight()
    {
        _currentPoint = _posRightIncline;
    }

    public void OffIncline()
    {
        _currentPoint = _posForCamera;
    }
}
