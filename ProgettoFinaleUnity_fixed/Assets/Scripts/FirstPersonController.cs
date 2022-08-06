using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
   
    public CharacterController Controller;
    public InputCooker InputCooker;
    public Rigidbody RB;
   
    [Tooltip("The height of the jump in meters")]
    public float JumpHeight = 1.2f;
    //public float Gravity = -9.8f;
    [Tooltip("Time before the Player can jump again")]
    public float JumpTimeout = 0.2f;
    [Tooltip("Time before the groundcheck is executed again. Should never be 0, else the jump won't work")]
    public float FallTimeout = 0.15f;

    [Header("Grounded and Airborne values")]
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.5f;
    public LayerMask GroundLayers;

    
    [Tooltip("The deceleration factor of the Player when he stops moving")]
    public float GroundedDrag = 5f;
    [Tooltip("The velocity factor when the Player is grounded")]
    public float GroundedVelocityMul = 1f;
    public float AirbornDrag = 0f;
    [Tooltip("The velocity factor when the player is in the air. "+
        "To have the same velocity when grounded, this value needs to be 1/GroundedDrag ")]
    public float AirborneVelocityMul =0.2f;
    [Range(0,1)]
    public float FrictionSlope = 1f;
    public float slopeVectorMul;
    public float MaximumVelocity = 10f;
    public bool Grounded = true;
    private float _jumpTimeOut;
    private float _fallTimeOut;
    private float slopeAngle = 0f;
    private float velocityMultiplier=1;
    private Vector3 SlopeVector;
    private Vector3 additionalVectors;
    private PhysicMaterial physicsMat;
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        InputCooker = GetComponent<InputCooker>();
        _jumpTimeOut = JumpTimeout;
        _fallTimeOut = FallTimeout;
        physicsMat = this.gameObject.GetComponent<CapsuleCollider>().sharedMaterial;
    }
    private void SlopeDetector()
    {
        Vector3 post = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Ray towardsGround = new Ray(post, Vector3.down);
        RaycastHit info;
        if(! Physics.Raycast(towardsGround, out info, 0.1f,3))
        {
            return;
        }
      
        
        Vector3 perpendicular = Vector3.Cross(Vector3.down, info.normal);

        
        Vector3 newForward = Vector3.Cross(perpendicular, -info.normal).normalized;

        // Debug.DrawRay(info.point, newForward, Color.red,2f);
        //Debug.DrawRay(info.point, perpendicular, Color.blue, 2f);
        // Debug.Log("normal = "+ info.normal +"   perp: "+ newForward + "  angle ="+VectorOps.AngleVec(Vector3.down, newForward));
        SlopeVector = -newForward.normalized;
        slopeAngle = VectorOps.AngleVec(Vector3.down, newForward);
    }
    void Update()
    {
        GroundedCheck();
        JumpAndGravity();
       
    }
    private void FixedUpdate()
    {

        RB.AddForce(InputCooker.RotatedMoveValue *velocityMultiplier, ForceMode.Force);
        Vector3 horizontalVelocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
        if (horizontalVelocity.sqrMagnitude >= (MaximumVelocity * MaximumVelocity))
        {
            horizontalVelocity = horizontalVelocity.normalized * 10;
            RB.velocity = new Vector3(horizontalVelocity.x, RB.velocity.y, horizontalVelocity.z);
        }

        if (slopeAngle <= 89f)
        {
           
            physicsMat.dynamicFriction = FrictionSlope;
            physicsMat.frictionCombine = PhysicMaterialCombine.Maximum;
            RB.AddForce(SlopeVector * slopeVectorMul, ForceMode.Acceleration);
        }
        else
        {
            physicsMat.dynamicFriction = 0;
            physicsMat.frictionCombine = PhysicMaterialCombine.Average;

        }
        additionalVectors = Vector3.zero;
        Debug.Log(this.gameObject.GetComponent<CapsuleCollider>().sharedMaterial.dynamicFriction);
    }
    private void Sprint()
    {

    }
    private void JumpAndGravity()
    {
        if (Grounded)
        {
            SlopeDetector();
            
            StartGrounded();
        }
        else
        {
            StartAirborne();
        }

        //// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        //if (_verticalVelocity < _terminalVelocity)
        //{
        //    _verticalVelocity += Gravity * Time.deltaTime;
        //}
    }
    private void StartGrounded()
    {
        RB.drag = GroundedDrag;
        // reset the fall timeout timer
        velocityMultiplier = GroundedVelocityMul;

        ////stop our velocity dropping infinitely when grounded
        //if (_verticalVelocity < 0.0f)
        //{
        //    _verticalVelocity = -2f;
        //}

        // jump timeout
        if (_jumpTimeOut >= 0.0f)
        {
            _jumpTimeOut -= Time.deltaTime;
        }


        // Jump
        if (InputCooker.isJump && _jumpTimeOut <= 0.0f)
        {
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            _fallTimeOut = FallTimeout;
            // _verticalVelocity = Mathf.Sqrt(JumpHeight * 2f * -Gravity);
            //float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
            RB.drag =AirbornDrag;
            
            
            float jumpForce = Mathf.Sqrt(JumpHeight * -2 * Physics.gravity.y);
            RB.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            InputCooker.isJump = false;
        }

    }
    private void StartAirborne()
    {
        // reset the jump timeout timer
        _jumpTimeOut = JumpTimeout;

        //drag must be 0 for gravity
        RB.drag = AirbornDrag;
        velocityMultiplier = AirborneVelocityMul;
        // fall timeout
        if (_fallTimeOut >= 0.0f)
        {
            _fallTimeOut -= Time.deltaTime;
        }
        InputCooker.isJump = false;
    }

    private void GroundedCheck()
    {
        if(_fallTimeOut > 0)
        {
            Grounded = false;
            return;
        }

        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers);
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
