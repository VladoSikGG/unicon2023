using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    //[SerializeField] private Joystick _joystick;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _groundDrag;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    private bool _readyToJump;
    private bool _isBoost;
    private bool _isCrouch;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode speedKey = KeyCode.LeftShift;
    public KeyCode leftIncline = KeyCode.Q,
                   rightIncline = KeyCode.E;

    private bool _grounded;

    [Header("From Player")]
    [SerializeField] private Transform _orientationInWorld;
    private Rigidbody _rigidBody;

    private float _horizontalInput, _verticalInput;
    private Vector3 _moveDirection;

    [SerializeField] private CameraHandler _camHandler;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;

        _readyToJump = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            _grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            _grounded = false;
        }
    }
    private void Update()
    {

        MyInput();

       // handle drag
        if (_grounded)
        {
            _rigidBody.drag = _groundDrag;
        }
        else
        {
            _rigidBody.drag = 0;
        }

        if (Input.GetKeyDown(rightIncline)) _camHandler.IclineRight();
        if (Input.GetKeyDown(leftIncline))_camHandler.IclineLeft();
        if (Input.GetKeyUp(leftIncline) || Input.GetKeyUp(rightIncline)) _camHandler.OffIncline();

        if (Input.GetKeyDown(crouchKey)) Crouch(0.5f);
        if (Input.GetKeyUp(crouchKey)) Crouch();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        if (_grounded)
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical");
        }
        

        // when to jump
        if (Input.GetKey(jumpKey) && _readyToJump && _grounded)
        {
            _readyToJump = false;

            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        _moveDirection = _orientationInWorld.forward * _verticalInput + _orientationInWorld.right * _horizontalInput;

        float currentSpped = _moveSpeed;
        //boost think how make that
        if (Input.GetKey(speedKey) && !_isCrouch)
        {
            _isBoost = true;
            currentSpped = _runSpeed;
        }
        else if (Input.GetKey(crouchKey) && !_isBoost)
        {
            _isCrouch = true;
            currentSpped = _crouchSpeed;
        }
        else
        {
            _isCrouch = false;
            _isBoost = false;
            currentSpped = _moveSpeed;
        }

        // on ground
        Vector3 moveDirection = transform.TransformDirection(_moveDirection) * currentSpped;
        if (_grounded)
        {
            _rigidBody.velocity = new Vector3(moveDirection.x, _rigidBody.velocity.y, moveDirection.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z);
        _rigidBody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        _readyToJump = true;
    }

    public void Incline(float value)
    {
        _orientationInWorld.transform.rotation = Quaternion.Euler(_orientationInWorld.transform.rotation.x + value,0,0);
    }

    //public void Incline()
    //{
    //    transform.rotation = Quaternion.Euler(_orientationInWorld.transform.rotation.x - value, 0, 0);
    //}

    public void Crouch(float value)
    {
        transform.localScale = new Vector3(1, value, 1);
        _rigidBody.AddForce(Vector3.up * 5);
    }

    public void Crouch()
    {
        transform.localScale = new Vector3(1,1,1);
    }
}