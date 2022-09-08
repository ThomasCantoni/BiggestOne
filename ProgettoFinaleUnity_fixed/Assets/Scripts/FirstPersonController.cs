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
    [Header("General Movement Values")]

    [Tooltip("The speed of the Player")]
    public float Speed = 10f;
    [Tooltip("The max velocity in Units/Second of the Player")]
    public float MaximumAllowedVelocity = 10f;


    [Header("Jump Values")]
    [Tooltip("The height of the jump in meters")]
    public float JumpHeight = 1.2f;
    //public float Gravity = -9.8f;
    [Tooltip("Time before the Player can jump again. Ticks when grounded")]
    public float JumpCooldown = 0.2f;
    [Tooltip("Time before the Player can slide again.")]
    public float SlideCooldown = 200f;
    public float SlideTimeOutMilliseconds = 150f;



    [Header("Grounded and Airborne values")]
    public float GroundcheckFrequency = 0.15f;
    [Tooltip("Time before the groundcheck is executed again after jumping. Should never be 0, else the jump will stop immediately.")]
    public float FallTimeOutMilliseconds = 150f;
    [Tooltip("The amount of gravitational pull the Player should get grounded. Used to manage slopes and cliffdrops")]
    public float GroundedGravity = 5;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.5f;
    public LayerMask GroundLayers;

    [Tooltip("The deceleration factor of the Player when he stops moving")]
    public float GroundedDrag = 5f;
    [Tooltip("The velocity factor when the Player is grounded")]
    public float GroundedVelocityMul = 1f;
    [Tooltip("The deceleration factor of the Player when in the air. The lower this is, the higher the air control.")]
    public float AirbornDrag = 0f;

    [Tooltip("The velocity factor when the Player is in the air.")]
    public float AirborneVelocityMul = 0.2f;

    [Header("Slope Values")]
    [Range(0, 1)]
    public float FrictionSlope = 0.2f;
    public float SlopeMinAdjustRange = 45f;
    public float SlopeMaxAdjustRange = 89f;

    [Header("READ ONLY")]
    public bool Grounded = true;
    public bool DoubleJumpPossible = false;
    private float _jumpCD;
    private float _fallTimeOut;
    private float slopeAngle = 0f;
    private float velocityMultiplier = 1;
    [HideInInspector]
    public Vector3 SlopeCounterVector, SlopeCounterVectorNORMALIZED;

    [Header("Slide")]
    CapsuleCollider capsColl;
    float originalHeight;
    public float reduceHeight;
    public float slideSpeed = 10f;
    bool canSlide = true;
    private float _SlideCD;
    private float _slideTimeOut;


    private PhysicMaterial physicsMat;
    public bool wallRunning;
    protected SimpleTimer JumpTimer, GroundcheckTimer, SlideTimer;
    Repeater GroundcheckRepeater;
    public bool CanJump = true, CanGroundCheck = true;
    [HideInInspector]
    public Vector3 PositionBeforeJump;
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        InputCooker = GetComponent<InputCooker>();
        capsColl = GetComponent<CapsuleCollider>();
        originalHeight = capsColl.height;
        _jumpCD = JumpCooldown;
        _SlideCD = SlideCooldown;
        _fallTimeOut = FallTimeOutMilliseconds;
        _slideTimeOut = SlideTimeOutMilliseconds;
        physicsMat = this.gameObject.GetComponent<CapsuleCollider>().sharedMaterial;

        GroundcheckRepeater = new Repeater();
        GroundcheckRepeater.Frequency = GroundcheckFrequency;
        GroundcheckRepeater.RepeaterTickEvent += GroundedCheck;
        GroundcheckRepeater.RepeaterPauseEvent += () => Grounded = false;
        GroundcheckRepeater.StartRepeater();

        //set the actions of timers 
        JumpTimer = new SimpleTimer(_jumpCD);
        JumpTimer.TimerStartEvent += () => CanJump = false;
        JumpTimer.TimerCompleteEvent += () => CanJump = true;

        //set the actions of timers 
        SlideTimer = new SimpleTimer(_SlideCD);
        SlideTimer.TimerStartEvent += () => canSlide = false;
        //SlideTimer.TimerCompleteEvent += GoUp;
        SlideTimer.TimerCompleteEvent += () => canSlide = true;
        InputCooker.PlayerStopSliding += SlideTimer.StopTimer;
        SlideTimer.TimerCompleteEvent += () => Debug.LogError("SLIDE TIMER COMPLETED");
    }
    void Update()
    {
        JumpAndGravity();
    }

    private void FixedUpdate()
    {
        Vector3 toAdd = (InputCooker.RelativeDirection.normalized * velocityMultiplier * Speed);
        Vector3 RigidBody_horizontalVelocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
        Vector3 predictive = RigidBody_horizontalVelocity + toAdd * Time.fixedDeltaTime;

        if (predictive.magnitude >= MaximumAllowedVelocity)
        {
            toAdd = Vector3.ClampMagnitude(toAdd, MaximumAllowedVelocity);
        }

        RB.AddForce(toAdd * Time.fixedDeltaTime, ForceMode.VelocityChange);

        RigidBody_horizontalVelocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);

        if (RigidBody_horizontalVelocity.magnitude >= MaximumAllowedVelocity)
        {
            RigidBody_horizontalVelocity = RigidBody_horizontalVelocity.normalized * MaximumAllowedVelocity;
            RB.velocity = new Vector3(RigidBody_horizontalVelocity.x, RB.velocity.y, RigidBody_horizontalVelocity.z);
        }

        if (Grounded)
        {
            AccountForSlope();
        }
        if (canSlide)
        {
            GoUp();
        }
        InputCooker.UpdateCameras();
    }
    private void GoUp()
    {
        if (_slideTimeOut >= 0.0f)
        {
            _slideTimeOut -= Time.deltaTime * 1000;
        }
        capsColl.height = originalHeight;
    }
    private void AccountForSlope()
    {

        //i am on a slope that isn't too steep or too flat and grounded
        //Debug.Log(" Angle: " + slopeAngle + "  IC.Dir: "+InputCooker.inputDirection);
        Vector3 goDown = Physics.gravity * 0.25f;
        RB.AddForce(goDown, ForceMode.Acceleration);

        if ((slopeAngle >= SlopeMinAdjustRange && slopeAngle <= SlopeMaxAdjustRange)
            && InputCooker.AbsoluteDirection.sqrMagnitude < 1f)

        {
            //physicsMat.dynamicFriction = FrictionSlope;
            physicsMat.frictionCombine = PhysicMaterialCombine.Maximum;
            Vector3 test = Vector3.down * Vector3.Dot(-SlopeCounterVectorNORMALIZED, goDown);
            RB.AddForce(SlopeCounterVector + SlopeCounterVector.normalized * test.magnitude, ForceMode.Acceleration);
            Debug.DrawRay(transform.position, SlopeCounterVector + SlopeCounterVector.normalized * Physics.gravity.magnitude, Color.red, 1f);
            Debug.DrawRay(transform.position, goDown, Color.blue, 1f);

        }
        else
        {
            SlopeCounterVector = Vector3.zero;
            physicsMat.dynamicFriction = 0;
            physicsMat.frictionCombine = PhysicMaterialCombine.Average;
        }
    }

    private void SlopeDetector()
    {
        Vector3 post = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Ray towardsGround = new Ray(post, Vector3.down);
        RaycastHit info;
        if (!Physics.Raycast(towardsGround, out info, 0.1f, 3))
        {
            SlopeCounterVector = Vector3.zero;
            return;
        }
        SlopeCounterVectorNORMALIZED = Vector3.ProjectOnPlane(Vector3.up, info.normal).normalized;
        SlopeCounterVector = SlopeCounterVectorNORMALIZED * Vector3.Dot(-SlopeCounterVectorNORMALIZED, Physics.gravity);

        Debug.DrawRay(transform.position, SlopeCounterVector, Color.red, 1f);

        slopeAngle = VectorOps.AngleVec(Vector3.up, SlopeCounterVector.normalized);
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
        if (canSlide)
        {
            RB.drag = GroundedDrag;
            velocityMultiplier = GroundedVelocityMul;
        }
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

        if (InputCooker.isSliding && canSlide)
        {
            InputCooker.isSliding = false;
            _slideTimeOut = SlideTimeOutMilliseconds;
            capsColl.height = reduceHeight;
            RB.drag = AirbornDrag;
            RB.AddForce(InputCooker.RelativeDirection * slideSpeed, ForceMode.VelocityChange);
            SlideTimer.StartTimer();
        }
    }
    private void StartAirborne()
    {
        //_jumpTimeOut = JumpTimeout;
        RB.drag = AirbornDrag;
        velocityMultiplier = AirborneVelocityMul;
        //canSlide = false;
        //SlideTimer.StopTimer();

        if (_fallTimeOut >= 0.0f)
        {
            _fallTimeOut -= Time.deltaTime * 1000;
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
        //Debug.Log("Checking if grounded");
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
