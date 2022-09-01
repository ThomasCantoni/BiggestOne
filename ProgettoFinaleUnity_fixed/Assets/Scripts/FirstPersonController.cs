using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;


public class FirstPersonController : MonoBehaviour
{
    public CharacterController Controller;
    public InputCooker InputCooker;
    public Rigidbody RB;

    [Tooltip("The height of the jump in meters")]
    public float JumpHeight = 1.2f;
    //public float Gravity = -9.8f;
    [Tooltip("Time before the Player can jump again. Ticks when grounded")]
    public float JumpCooldown = 0.2f;


    public float GroundcheckFrequency = 0.15f;
    [Tooltip("Time before the groundcheck is executed again after jumping. Should never be 0, else the jump won't work")]
    public float FallTimeOutMilliseconds = 150f;

    [Header("Grounded and Airborne values")]
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.5f;
    public LayerMask GroundLayers;

    [Tooltip("The deceleration factor of the Player when he stops moving")]
    public float GroundedDrag = 5f;
    [Tooltip("The velocity factor when the Player is grounded")]
    public float GroundedVelocityMul = 1f;
    public float AirbornDrag = 0f;
    [Tooltip("The velocity factor when the player is in the air. " +
        "To have the same velocity when grounded, this value needs to be 1/GroundedDrag ")]
    public float AirborneVelocityMul = 0.2f;

    [Header("Slope Values")]
    [Range(0, 1)]
    public float FrictionSlope = 1f;
    public float SlopeMinAdjustRange = 45f;
    public float SlopeMaxAdjustRange = 89f;
    private float slopeVectorRawMul;
    public float slopeVectorMagnitude;
    public float MaximumVelocity = 10f;
    public bool Grounded = true;
    public bool DoubleJumpPossible = false;
    private float _jumpCD;
    private float _fallTimeOut;
    private float slopeAngle = 0f;
    private float velocityMultiplier = 1;
    public Vector3 SlopeVector;
    private Vector3 additionalVectors;
    private PhysicMaterial physicsMat;
    public bool wallRunning;
    protected SimpleTimer JumpTimer,GroundcheckTimer;
    Repeater GroundcheckRepeater;
    public bool CanJump = true, CanGroundCheck=true;
    [HideInInspector]
    public Vector3 PositionBeforeJump;
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        InputCooker = GetComponent<InputCooker>();
        _jumpCD = JumpCooldown;
        _fallTimeOut = FallTimeOutMilliseconds;
        physicsMat = this.gameObject.GetComponent<CapsuleCollider>().sharedMaterial;


        GroundcheckRepeater = new Repeater();
        GroundcheckRepeater.Frequency = GroundcheckFrequency;
        GroundcheckRepeater.RepeaterTickEvent += GroundedCheck;
        GroundcheckRepeater.RepeaterTickEvent += () => Debug.Log("TickEvent triggering Hz = " + GroundcheckRepeater.Frequency);
        //GroundcheckRepeater.RepeaterTickEvent += () => Debug.Log(Grounded);
        GroundcheckRepeater.RepeaterPauseEvent += () => Grounded = false;
        GroundcheckRepeater.RepeaterPauseEvent += () => Debug.Log("PAUSE EVENT " + Grounded);

        GroundcheckRepeater.StartRepeater();


        //set the actions of timers 
        JumpTimer = new SimpleTimer(_jumpCD);
        JumpTimer.TimerStartEvent    += () => CanJump = false;
        JumpTimer.TimerCompleteEvent += () => CanJump = true;

        //GroundcheckTimer = new SimpleTimer(_fallTimeOut);
        //GroundcheckTimer.TimerStartEvent += () => CanGroundCheck = true;
       // GroundcheckTimer.TimerCompleteEvent += () => CanGroundCheck = true;
    }
    private void SlopeDetector()
    {
        Vector3 post = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Ray towardsGround = new Ray(post, Vector3.down);
        RaycastHit info;
        if (!Physics.Raycast(towardsGround, out info, 0.1f, 3))
        {
            SlopeVector = Vector3.zero;
            return;
        }
        SlopeVector = Vector3.ProjectOnPlane(Vector3.up, info.normal);
        slopeAngle = VectorOps.AngleVec(Vector3.up, SlopeVector);
    }
    void Update()
    {
        if (_fallTimeOut > 0)
        {
            //Grounded = false;
            _fallTimeOut -= Time.deltaTime*1000;
        }
        else
        {
          // GroundedCheck();

        }
        JumpAndGravity();
        

    }

    private void FixedUpdate()
    {
        RB.AddForce(InputCooker.RotatedMoveValue * velocityMultiplier * Time.fixedDeltaTime, ForceMode.Force);
        InputCooker.UpdateCameras();
        Vector3 horizontalVelocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
        if (horizontalVelocity.sqrMagnitude >= (MaximumVelocity * MaximumVelocity))
        {
            horizontalVelocity = horizontalVelocity.normalized * 10;
            RB.velocity = new Vector3((int)horizontalVelocity.x, RB.velocity.y, (int)horizontalVelocity.z);
        }
        if (Grounded)
        {
            RB.AddForce(Vector3.down * 3, ForceMode.Acceleration);
        }

        //i am on a slope that isn't too steep or too flat and grounded
        //Debug.Log(" Angle: " + slopeAngle + "  IC.Dir: "+InputCooker.inputDirection);
        if ((slopeAngle>= SlopeMinAdjustRange && slopeAngle <= SlopeMaxAdjustRange) 
            && Grounded 
            && InputCooker.AbsoluteDirection.sqrMagnitude <1f)

        {
            physicsMat.dynamicFriction = FrictionSlope;
            physicsMat.frictionCombine = PhysicMaterialCombine.Maximum;
            RB.AddForce(SlopeVector * slopeVectorMagnitude, ForceMode.Force);
        }
        else
        {
            SlopeVector = Vector3.zero;
            physicsMat.dynamicFriction = 0;
            physicsMat.frictionCombine = PhysicMaterialCombine.Average;
        }
        additionalVectors = Vector3.zero;
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
    }
    private void StartGrounded()
    {
        RB.drag = GroundedDrag;
        velocityMultiplier = GroundedVelocityMul;

        //if (_jumpTimeOut >= 0.0f)
        //{
        //    _jumpTimeOut -= Time.deltaTime;
        //}
        if (InputCooker.isJump && CanJump)
        {
            GroundcheckRepeater.StopRepeater(200);
            Grounded = false;
            _fallTimeOut = FallTimeOutMilliseconds;
            //GroundcheckTimer.StartTimer();
            RB.drag = AirbornDrag;
            float jumpForce = Mathf.Sqrt(JumpHeight * -2 * Physics.gravity.y);
            RB.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            InputCooker.isJump = false;
            DoubleJumpPossible = true;
            JumpTimer.StartTimer();
            Grounded = false;
        }
    }
    private void StartAirborne()
    {
        //_jumpTimeOut = JumpTimeout;
        RB.drag = AirbornDrag;
        velocityMultiplier = AirborneVelocityMul;

        if (_fallTimeOut >= 0.0f)
        {
            _fallTimeOut -= Time.deltaTime*1000;
        }
        if (InputCooker.isJump && DoubleJumpPossible)
        {
            RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
            float jumpForce = Mathf.Sqrt(JumpHeight * -2 * Physics.gravity.y);
            RB.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            DoubleJumpPossible = false;
        }
        InputCooker.isJump = false;
    }

    public void GroundedCheck()
    {
        Debug.Log("Checking if grounded");
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers);
        //GroundcheckTimer.StartTimer();
    }
    private void OnDrawGizmosSelected()
    {
        if (Grounded) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
    }
    //private void OnApplicationQuit()
    //{
    //    JumpTimer.StopTimer();
    //    GroundcheckRepeater.StopRepeater();
    //}
    
}
