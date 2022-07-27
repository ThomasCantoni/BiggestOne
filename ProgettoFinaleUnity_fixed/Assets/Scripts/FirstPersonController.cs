using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
   
    public CharacterController Controller;
    public InputCooker InputCooker;

    private float _verticalVelocity;
    public float JumpHeight = 1.2f;
    public float Gravity = -7.0f;
    public float JumpTimeout = 0.1f;
    public float FallTimeout = 0.15f;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.5f;
    private float _terminalVelocity = 53.0f;

    public LayerMask GroundLayers;
    public bool Grounded = true;
    private float _jumpTimeOut;
    private float _fallTimeOut;


    private void Start()
    {
        InputCooker = GetComponent<InputCooker>();
        _jumpTimeOut = JumpTimeout;
        _fallTimeOut = FallTimeout;
    }
    void Update()
    {
        GroundedCheck();
        JumpAndGravity();
        Controller.Move(InputCooker.RotatedMoveValue * Time.deltaTime + new Vector3(0,_verticalVelocity,0) * Time.deltaTime);
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers);
    }
    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            _fallTimeOut = FallTimeout;

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (InputCooker.isJump && _jumpTimeOut <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                InputCooker.isJump = false;
            }

            // jump timeout
            if (_jumpTimeOut >= 0.0f)
            {
                _jumpTimeOut -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeOut = JumpTimeout;

            // fall timeout
            if (_fallTimeOut >= 0.0f)
            {
                _fallTimeOut -= Time.deltaTime;
            }
            InputCooker.isJump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }
    private void OnDrawGizmosSelected()
    {

        if (Grounded) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
    }


    // Start is called before the first frame update
    //void Start()
    //{
    //    Controls = new Controls();
    //    Controls.Enable();
    //    Controls.Player.Movement.performed += OnMove;
    //    Controls.Player.Movement.canceled += StopMovement;
    //    Controls.Player.Look.performed += RotateCamera;

    //}

    // Update is called once per frame
    /* public void MoveUpdate()
     {
         moveValue = new Vector3(inputDirection.x, 0.0f, inputDirection.y);

         // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
         // if there is a move input rotate player when the player is moving
         if (inputDirection != Vector2.zero)
         {
             // move
             moveValue = transform.rotation * moveValue;
         }
         Controller.Move(moveValue * Time.deltaTime);

     }
     public void OnMove(InputAction.CallbackContext value)
     {
         inputDirection =  value.ReadValue<Vector2>();

     }

     public void StopMovement(InputAction.CallbackContext value)
     {
         inputDirection = Vector3.zero;
     }
     public void RotateCamera(InputAction.CallbackContext value)
     {
         Vector2 val = value.ReadValue<Vector2>();

         CameraTargetPitch += val.y * 1f;
         CameraTargetPitch= ClampAngle(CameraTargetPitch, -90, 90);
         MainCamera.transform.localRotation = Quaternion.Euler(CameraTargetPitch, 0.0f, 0.0f);

         float rotationVelocity = val.x * 1f ;
         transform.Rotate(Vector3.up * rotationVelocity);


     }
     private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
     {
         if (lfAngle < -360f) lfAngle += 360f;
         if (lfAngle > 360f) lfAngle -= 360f;
         return Mathf.Clamp(lfAngle, lfMin, lfMax);
     }
    */
}
