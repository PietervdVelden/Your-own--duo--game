using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{

    // Variables: Gravity
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;
    
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    // public float jumpPower = 7f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift); // 'keyCode' should be 'KeyCode'
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        #endregion

        // #region Handles Jumping
        // if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        // {
        //     moveDirection.y = jumpPower;
        // }
        // else
        // {
        //     moveDirection.y = movementDirectionY;
        // }

        // if (!characterController.isGrounded)
        // {
        //     moveDirection.y -= gravity * Time.deltaTime;
        // }
        // #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0); // 'PlayerCamera' should be 'playerCamera'
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion

        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0)
        {
            _velocity = -1f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }

        moveDirection.y = _velocity;
    }

    private bool IsGrounded() => characterController.isGrounded;

}

