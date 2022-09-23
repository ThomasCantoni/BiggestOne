using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;


public class FirstPersonController : MonoBehaviour
{
    public SlideCharacter SC;
    public Dash DashScript;
    public InputCooker IC;
    public Rigidbody RB;
    [Header("General Movement Values")]

    [Tooltip("The speed of the Player")]
    public float Speed = 10f;
    [Tooltip("The max velocity in Units/Second of the Player")]
    public float MaximumAllowedVelocity = 10f;

    public bool ClampSpeed = true;



    [Header("Jump Values")]
    [Tooltip("The height of the jump in meters")]
    public float JumpHeight = 1.2f;
    //public float Gravity = -9.8f;
    [Tooltip("Time before the Player can jump again. Ticks when grounded")]
    public float JumpCooldown = 0.2f;




    [Header("Grounded and Airborne values")]
    public float GroundcheckFrequency = 0.15f;
    [Tooltip("Time before the groundcheck is executed again after jumping. Should never be 0, else the jump will stop immediately.")]
    public float FallTimeOutMilliseconds = 150f;
    [Tooltip("The amount of gravitational pull the Player should get grounded. Used to manage slopes and cliffdrops")]
    public float GravityMul = 2f;
    [Tooltip("Soft Grounded is a condition in which the player is touching the ground but is still being pulled down by gravity. Used for going down slopes")]
    public float SoftGroundedOffset = -0.14f;
    public float SoftGroundedRadius = 0.5f;
    [Tooltip("Hard Grounded is a condition in which the player is touching the ground and is not being pulled down by gravity.")]
    public float HardGroundedOffset = -0.14f;
    public float HardGroundedRadius = 0.15f;

    public LayerMask GroundLayers;


    [Tooltip("The speed multiplier when the Player is grounded")]
    public float GroundedVelocityMul = 1f;

    public bool ApplyDrag = true;
    [Tooltip("The friction factor of the Player when he stops moving")]
    public Vector3 GroundedStillDragVector;

    [Tooltip("The friction factor of the Player whhile he is moving")]
    public Vector3 GroundedMovementDragVector;
    //[Tooltip("The friction factor of the Player while in the air and no input is given.")]
    //public float AirbornStillDrag = 0f;
    [Tooltip("The friction vector of the Player while in the air moving. The lower this is, the higher the air control.")]
    public Vector3 AirborneDragVector;
    //public float AirbornMovementDrag = 1f;
    [Tooltip("The speed multiplier when the Player is in the air.")]
    public float AirborneVelocityMul = 0.2f;
    [Header("Slope Values")]
    //[Range(0, 1)]
    //public float FrictionSlope = 0.2f;
    public float SlopeMinAdjustRange = 45f;
    public float SlopeMaxAdjustRange = 89f;

    [Header("READ ONLY")]
    public bool SoftGrounded = true;
    public bool HardGrounded = false;
    public bool DoubleJumpPossible = false;
    private bool wasGroundedBefore = false;
    private float _jumpCD;
    private float _fallTimeOut;
    private float slopeAngle = 0f;
    private float velocityMultiplier = 1;
    [HideInInspector]
    public Vector3 SlopeCounterVector, SlopeCounterVectorNORMALIZED;
    public delegate void PlayerGroundedEvent();
    public PlayerGroundedEvent PlayerStartedGrounded, PlayerStartedAirborne;
    
    private PhysicMaterial physicsMat;
    protected SimpleTimer JumpTimer, GroundcheckTimer, JumpGraceTimer;
    Repeater GroundcheckRepeater;
    private bool jumpCooledDown = true;
    private float currentGroundY;
    
    public Vector3 currentArtificialDrag;
    public bool WantsToMove
    {
        get { return IC.AbsoluteDirection.magnitude >= 1f; }
    }
    public bool IsMoving
    {
        get { return RB_velocityXZ.magnitude > 1f; }
    }
    public Vector3 RB_velocityXZ
    {
        get
        {
            return new Vector3(RB.velocity.x, 0, RB.velocity.z);
        }
        set
        {
            RB.velocity = new Vector3(value.x, 0, value.z);
        }
    }
    public bool IsDashing
    {
        get { return DashScript.IsDashing; }

    }
    public Vector3 PlayerGravity
    {
        get
        {
            return Physics.gravity * GravityMul;
        }
    }
    public bool IsOnSlope = false;
    private void Start()
    {
        SC = GetComponent<SlideCharacter>();
        RB = GetComponent<Rigidbody>();
        IC = GetComponent<InputCooker>();
        DashScript = GetComponent<Dash>();
        _fallTimeOut = FallTimeOutMilliseconds;
        _jumpCD = JumpCooldown;
        physicsMat = this.gameObject.GetComponent<CapsuleCollider>().sharedMaterial;

        GroundcheckRepeater = new Repeater();
        GroundcheckRepeater.Frequency = GroundcheckFrequency;
        GroundcheckRepeater.RepeaterTickEvent += GroundedCheck;
        GroundcheckRepeater.RepeaterPauseEvent += () => SoftGrounded = false;
        GroundcheckRepeater.StartRepeater();

        //set the actions of timers 
        JumpTimer = new SimpleTimer(_jumpCD);
        JumpTimer.TimerStartEvent += () => jumpCooledDown = false;
        JumpTimer.TimerCompleteEvent += () => jumpCooledDown = true;

        //JumpGraceTimer = new SimpleTimer(_fallTimeOut);
        //JumpGraceTimer.TimerStartEvent += () => graceTimer = true;
        //JumpGraceTimer.TimerCompleteEvent += () => graceTimer = false;

        PlayerStartedGrounded += () => DoubleJumpPossible = true;
    }
    void Update()
    {
        JumpAndGravity();
        if (SoftGrounded && !wasGroundedBefore)
        {
            PlayerStartedGrounded?.Invoke();
        }
        if (!SoftGrounded && wasGroundedBefore)
        {
            PlayerStartedAirborne?.Invoke();
        }
        wasGroundedBefore = SoftGrounded;
    }

    private void FixedUpdate()
    {

        ApplyForce();


        //if(!FullStopGrounded || !Grounded)
        //{
        //    RB.constraints &= ~RigidbodyConstraints.FreezePositionY;
        //    Debug.Log("RESUMING FALL " + RB.constraints);
        //    //RB.constraints = RigidbodyConstraints.None;
        //    RB.AddForce(PlayerGravity, ForceMode.Acceleration);   
        //}
        if (SoftGrounded)
        {
            if (HardGrounded)
            {
                RB.AddForce (new Vector3(0, Physics.gravity.y*-1,0),ForceMode.Acceleration);
                //RB.velocity = new Vector3(RB.velocity.x,0, RB.velocity.z);
            }
            else
            {
                RB.AddForce(PlayerGravity,ForceMode.Acceleration);

            }
            AccountForSlope();
           
            //if(IsOnSlope)
            //{
            //    if(RB.velocity.y > 0)
            //    {
            //        RB.AddForce(Vector3.down)
            //    }
            //}
        }

        IC.UpdateCameras();
    }

    private void ApplyForce()
    {
        //clamp before

        Vector3 toAdd = Speed * Time.fixedDeltaTime * velocityMultiplier * IC.RelativeDirection.normalized;
        Vector3 RigidBody_horizontalVelocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
        Vector3 predictive = RigidBody_horizontalVelocity + toAdd;
        if (ClampSpeed && predictive.magnitude >= MaximumAllowedVelocity * Time.fixedDeltaTime)
        {
            toAdd = Vector3.ClampMagnitude(toAdd, MaximumAllowedVelocity);
        }

            RB.AddForce(toAdd, ForceMode.VelocityChange);
        if (ApplyDrag)
        {
            
            RB.velocity = new Vector3(RB.velocity.x * (1 - currentArtificialDrag.x * Time.fixedDeltaTime),
                    RB.velocity.y * (1 - currentArtificialDrag.y * Time.fixedDeltaTime),
                    RB.velocity.z * (1 - currentArtificialDrag.z * Time.fixedDeltaTime));


        }

        RigidBody_horizontalVelocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);

        //clamp after

        if (ClampSpeed && RigidBody_horizontalVelocity.magnitude >= MaximumAllowedVelocity)

        {
            RigidBody_horizontalVelocity = RigidBody_horizontalVelocity.normalized * MaximumAllowedVelocity;
            RB.velocity = new Vector3(RigidBody_horizontalVelocity.x, RB.velocity.y, RigidBody_horizontalVelocity.z);
        }

        //Debug.Log(new Vector3(RB.velocity.x, 0, RB.velocity.z).magnitude);

    }

    private void AccountForSlope()
    {

        //i am on a slope that isn't too steep or too flat and grounded
        //Debug.Log(" Angle: " + slopeAngle + "  IC.Dir: "+InputCooker.inputDirection);
        //Vector3 goDown = Physics.gravity * 0.25f;

        if (slopeAngle >= SlopeMinAdjustRange && slopeAngle <= SlopeMaxAdjustRange)
        {
            //physicsMat.dynamicFriction = FrictionSlope;
            //physicsMat.frictionCombine = PhysicMaterialCombine.Maximum;
            int mult = HardGrounded ? 0 : 1;
            Vector3 test = Vector3.down * Vector3.Dot(-SlopeCounterVectorNORMALIZED, PlayerGravity*mult);
          
            RB.AddForce(SlopeCounterVector*mult+ (SlopeCounterVector.normalized * test.magnitude), ForceMode.Acceleration);
        }
        else
        {


            SlopeCounterVector = Vector3.zero;
            //physicsMat.dynamicFriction = 0;
            //physicsMat.frictionCombine = PhysicMaterialCombine.Average;
        }
    }

    private void SlopeDetector()
    {
        Vector3 post = new Vector3(transform.position.x, transform.position.y - SoftGroundedOffset, transform.position.z);
        Ray towardsGround = new Ray(post, Vector3.down);
        RaycastHit info;
        if (!Physics.Raycast(towardsGround, out info, 0.1f, 3))
        {
            SlopeCounterVector = Vector3.zero;
            return;
        }
        SlopeCounterVectorNORMALIZED = Vector3.ProjectOnPlane(Vector3.up, info.normal).normalized;
        SlopeCounterVector = SlopeCounterVectorNORMALIZED * Vector3.Dot(-SlopeCounterVectorNORMALIZED, Physics.gravity);
        slopeAngle = VectorOps.AngleVec(Vector3.up, SlopeCounterVector.normalized);
        //Debug.Log(slopeAngle);


        IsOnSlope = slopeAngle < 90f;



    }
    private void JumpAndGravity()
    {
        if (SoftGrounded)
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
        if (!SC.isSliding && !IsDashing)
        {
            if (WantsToMove)
            {
                // RB.drag = GroundedMovementDrag;
                currentArtificialDrag = GroundedMovementDragVector;
            }
            else
            {
                currentArtificialDrag = GroundedStillDragVector;

                //RB.drag = GroundedStillDrag;
            }
            velocityMultiplier = GroundedVelocityMul;
        }
        else
        {
            if (SC.isSliding)
            {
                velocityMultiplier = 0;
                currentArtificialDrag = Vector3.zero;

                Debug.Log("SLIDE");
            }

        }
        
        //Debug.Log(FullStopGrounded);
        //if (FullStopGrounded && RB.velocity.y < 0f)
        //{
        //    Debug.Log("STOPPING FALL " + RB.velocity.y);
        //    RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y * (-1), RB.velocity.z);
        //}
        //if (_jumpTimeOut >= 0.0f)
        //{
        //    _jumpTimeOut -= Time.deltaTime;
        //}
        if (IC.isJump && jumpCooledDown)
        {
            GroundcheckRepeater.StopRepeater(200);
            SoftGrounded = false;
            HardGrounded = false;
            

            _fallTimeOut = FallTimeOutMilliseconds;
            //GroundcheckTimer.StartTimer();
            //RB.drag = AirbornStillDrag;
            float jumpForce = Mathf.Sqrt(JumpHeight * -2 * Physics.gravity.y);
            RB.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            IC.isJump = false;

            JumpTimer.StartTimer();
        }


    }
    private void StartAirborne()
    {
        //_jumpTimeOut = JumpTimeout;

        if (WantsToMove || IsDashing)
        {
            RB.drag = 0;
            currentArtificialDrag = Vector3.zero;
        }
        else
        {
            currentArtificialDrag = AirborneDragVector;
        }
        velocityMultiplier = AirborneVelocityMul;

        //canSlide = false;
        //SlideTimer.StopTimer();


        if (IC.isJump && DoubleJumpPossible)
        {
            RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
            float jumpForce = Mathf.Sqrt(JumpHeight * -2 * Physics.gravity.y);
            RB.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            DoubleJumpPossible = false;
        }
        IC.isJump = false;
    }

    public void GroundedCheck()
    {
        //Debug.Log("Checking if grounded");
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - SoftGroundedOffset, transform.position.z);
        Vector3 spherePosition2 = new Vector3(transform.position.x, transform.position.y - HardGroundedOffset, transform.position.z);

        SoftGrounded = Physics.CheckSphere(spherePosition, SoftGroundedRadius, GroundLayers);
        HardGrounded = Physics.CheckSphere(spherePosition2, 
            HardGroundedRadius,
            GroundLayers);
        if(HardGrounded)
            currentGroundY = transform.position.y;
        //GroundcheckTimer.StartTimer();
    }
    private void OnDrawGizmosSelected()
    {
        if (SoftGrounded) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - SoftGroundedOffset, transform.position.z), SoftGroundedRadius);
        if (HardGrounded)Gizmos.color = Color.red+Color.blue;
        else Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - HardGroundedOffset, transform.position.z), HardGroundedRadius);


    }
    //private void OnApplicationQuit()
    //{
    //    JumpTimer.StopTimer();
    //    GroundcheckRepeater.StopRepeater();
    //}

}
