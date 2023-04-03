﻿using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    //[SerializeField] private Joystick _joystick;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _groundDrag;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;
    private bool _readyToJump;
    private float _walkSpeed;
    private float _sprintSpeed;

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
        _rigidBody = this.GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;

        _readyToJump = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.layer);
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
        SpeedControl();

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
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

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
      
        /* //boost think how make that
        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentSpped = _runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            currentSpped = _moveSpeed;
        }
*/
        // on ground
        if (_grounded)
        {
            _rigidBody.AddForce(_moveDirection.normalized * currentSpped * 10f, ForceMode.Force);
        }
        // in air
        else
        {
            _rigidBody.AddForce(_moveDirection.normalized * currentSpped * 10f * _airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > _moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            _rigidBody.velocity = new Vector3(limitedVel.x, _rigidBody.velocity.y, limitedVel.z);
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