using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    //[SerializeField] private Joystick _joystick;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _groundDrag;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    private bool _readyToJump;

    [Header("Keybinds")]
    public KeyCode _jumpKey = KeyCode.Space;

  /*  [Header("Ground Check")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _whatIsGround;
  */
    private bool _grounded;

    [Header("From Player")]
    [SerializeField] private Transform _orientationInWorld;
    private Rigidbody _rigidBody;

    private float _horizontalInput, _verticalInput;
    private Vector3 _moveDirection;
    

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
        if (Input.GetKey(_jumpKey) && _readyToJump && _grounded)
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Boost");
            currentSpped = _runSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Debug.Log("NotBoost");
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
}