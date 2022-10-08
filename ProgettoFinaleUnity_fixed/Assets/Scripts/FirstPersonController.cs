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
    public CharacterController RB;
    

    public WeaponSwitching WS;
    public PlayerInvetory PI;
    public FMODUnity.StudioEventEmitter FootstepSoundEmitter;
    [HideInInspector]
    public Repeater FootstepRepeater;
    public GameObject GrenadePrefab;
    public bool CanJumpInTutorial;
    public bool CanGrenadeInTutorial;
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
    public bool ApplyGravity;
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
    [Tooltip("This number dictates how fast the player will change direction when a different movement input is given")]
    public float SpeedChangeFactor = 1f;
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
    
    //[Range(0, 1)]
    //public float FrictionSlope = 0.2f;
    private float SlopeMinAdjustRange = 45f;
    private float SlopeMaxAdjustRange = 89f;

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
    public delegate void PlayerKilledSomethingEvent(FirstPersonController player, IHittable victim);
    public PlayerGroundedEvent PlayerStartedGrounded, PlayerStartedAirborne;
    public PlayerKilledSomethingEvent PlayerKilledSomething;
    
    protected SimpleTimer JumpTimer, GroundcheckTimer, JumpGraceTimer;
    Repeater GroundcheckRepeater;
    private bool jumpCooledDown = true;
  
    private float SoftG_originalOffset, HardG_originalOffset;
    public Vector3 currentArtificialDrag;
    public GameObject offset;
    private Vector3 slopeNormal,currentGravity,targetToAdd=Vector3.zero,toAdd;
    private bool CanMove = true;
    private float lerpAccumulator = 0;
    
    
    private bool jump;
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
        PI = GetComponent<PlayerInvetory>();
        SC = GetComponent<SlideCharacter>();
        RB = GetComponent<CharacterController>();
        IC = GetComponent<InputCooker>();
        DashScript = GetComponent<Dash>();
        _fallTimeOut = FallTimeOutMilliseconds;
        _jumpCD = JumpCooldown;

        FootstepRepeater = new Repeater();
        FootstepRepeater.Frequency = 2.5f;
        FootstepRepeater.RepeaterTickEvent += FootstepSoundEmitter.Play;

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

        PlayerStartedAirborne += () => RB.stepOffset = 0;
        PlayerStartedGrounded += () => DoubleJumpPossible = true;
        PlayerStartedGrounded += () => RB.stepOffset = 0.3f;



        SC.StartedSLiding += () =>
        {
            SoftG_originalOffset = SoftGroundedOffset;
            HardG_originalOffset = HardGroundedOffset;
            SoftGroundedOffset = SC.reduceHeight * 0.5f;
            HardGroundedOffset = SC.reduceHeight * 0.5f;
            CanMove = false;
        };
        SC.StoppedSliding += () =>
        {
            SoftGroundedOffset = SoftG_originalOffset;
            HardGroundedOffset = HardG_originalOffset;
            CanMove = true;
        };

        PlayerKilledSomething += (gun,victim) => Debug.Log("HOMICIDE!");

        IC.DirectionChanged += (newDir) =>
        {
            lerpAccumulator = 0;
            
        };
    }
    public void ThrowGrenade()
    {
        if (CanGrenadeInTutorial && PI.HasSpecialGrenade)
        {
            GameObject grenade = Instantiate(GrenadePrefab, offset.transform.position, IC.VirtualCamera.transform.rotation);
            //grenade.GetComponent<Rigidbody>().AddForce((offset.transform.forward * 10f), ForceMode.Impulse);
            grenade.GetComponent<GrenadeScript>().targetVector = IC.VirtualCamera.transform.position + IC.VirtualCamera.transform.forward * 3f;
            grenade.GetComponent<GrenadeScript>().targetForward = IC.VirtualCamera.transform.forward;
            PI.HasSpecialGrenade = false;
        }
        
    }
    public void Teleport(Transform destination)
    {
        RB.enabled = false;
        transform.position = destination.position;
        transform.rotation = destination.rotation;
        RB.enabled = true;
       

    }
    void Update()
    {
        //if (SoftGrounded)
        //{
        //    if (HardGrounded)
        //    {
        //        //RB.SimpleMove(new Vector3(0, Physics.gravity.y * -1, 0)); //was force
        //        //RB.velocity = new Vector3(RB.velocity.x,0, RB.velocity.z);
        //    }
        //    else
        //    {
        //        RB.MovePosition(transform.position + PlayerGravity*Time.fixedDeltaTime); //was force

        //        //if (!WantsToMove)
        //            //AccountForSlope();
        //    }

        //    //if(IsOnSlope)
        //    //{
        //    //    if(RB.velocity.y > 0)
        //    //    {
        //    //        RB.AddForce(Vector3.down)
        //    //    }
        //    //}
        //}
        //ApplyForce();

        //IC.UpdateCameras();
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



        //if(!FullStopGrounded || !Grounded)
        //{
        //    RB.constraints &= ~RigidbodyConstraints.FreezePositionY;
        //    Debug.Log("RESUMING FALL " + RB.constraints);
        //    //RB.constraints = RigidbodyConstraints.None;
        //    RB.AddForce(PlayerGravity, ForceMode.Acceleration);   
        //}




        
        ApplyForce();
        GravityJump();
        IC.UpdateCameras();
        
    }
    private void GravityJump()
    {
            Vector3 verticalForce = Vector3.zero;
        if(!SoftGrounded)
        {
            if(ApplyGravity)
            {
                currentGravity.y += PlayerGravity.y * Time.fixedDeltaTime;
                currentGravity.y = Mathf.Clamp(currentGravity.y, -60, 100);

            }
            else
            {
                currentGravity = Vector3.zero;
            }
                verticalForce = currentGravity;
        }
        else
        {
            if (ApplyGravity)
                currentGravity = PlayerGravity * 0.1f; //was force

            if (HardGrounded)
            {
                currentGravity = Vector3.zero;
                verticalForce = Vector3.zero; //was force
                //RB.velocity = new Vector3(RB.velocity.x,0, RB.velocity.z);
            }
            else
            {

                //if(!WantsToMove)
                //    AccountForSlope();
            }

            //if(IsOnSlope)
            //{
            //    if(RB.velocity.y > 0)
            //    {
            //        RB.AddForce(Vector3.down)
            //    }
            //}
        }
        
        //else if (ApplyGravity)
        //{
        //    verticalForce = currentGravity;
        //}
        if(jump)
        {
            jump = false;
            
            currentGravity.y = Mathf.Sqrt(-2f * JumpHeight * PlayerGravity.y);
            verticalForce = currentGravity;
        }
        Debug.Log("Vertical " + verticalForce);
        RB.Move(verticalForce*Time.fixedDeltaTime);
    }
    private void ApplyForce()
    {
        Vector3 target = IC.RelativeDirectionNotZero.normalized * Speed * velocityMultiplier;


        if (WantsToMove)
        {
            if(!FootstepRepeater.IsActive)
            {
                if(SoftGrounded || HardGrounded)
                    FootstepRepeater.StartRepeater();
            }
            else
            {
                if (!SoftGrounded && !HardGrounded)
                    FootstepRepeater.StopRepeater();

            }
            if (ClampSpeed)
                target = Vector3.ClampMagnitude(target, MaximumAllowedVelocity);

            targetToAdd = target;
        }
        else
        {
            FootstepRepeater.StopRepeater();
        }
        
            
        //targetToAdd = Vector3.Lerp(targetToAdd, target, 0.3f);
        //if (ClampSpeed && targetToAdd.magnitude >= MaximumAllowedVelocity)
        //{
        //    targetToAdd = Vector3.ClampMagnitude(targetToAdd, MaximumAllowedVelocity);
        //}
        //float curveValue = CurrentCurve.Evaluate(lerpAccumulator);
        if(CanMove)
        {
            toAdd = Vector3.Lerp(toAdd, targetToAdd, lerpAccumulator);

            lerpAccumulator +=  Time.fixedDeltaTime*SpeedChangeFactor;
            lerpAccumulator = Mathf.Clamp(lerpAccumulator, 0f, 1f);
        }
        else
        {
            lerpAccumulator = 0;
        }
        //toAdd = target;
        if (ApplyDrag)
        {

            targetToAdd = (new Vector3(targetToAdd.x * (1 - currentArtificialDrag.x * Time.fixedDeltaTime),
                    targetToAdd.y * (1 - currentArtificialDrag.y * Time.fixedDeltaTime),
                    targetToAdd.z * (1 - currentArtificialDrag.z * Time.fixedDeltaTime)));


        }
        if(CanMove)
            RB.Move(toAdd*Time.fixedDeltaTime); //was force
        
        
        

        Debug.DrawLine(transform.position, transform.position + target, Color.blue);

        Debug.DrawLine(transform.position, transform.position + toAdd, Color.red + Color.blue);

        return;
        #region ApplyForceOLD
        //clamp before
        /*
        Vector3 toAdd;
        toAdd = transform.rotation * IC.AbsoluteDirection;
        Debug.Log(toAdd);
        if (IsOnSlope)
        {
            toAdd = Vector3.ProjectOnPlane(IC.RelativeDirection.normalized, slopeNormal).normalized;

        }

        toAdd = toAdd * Speed * Time.fixedDeltaTime * velocityMultiplier;

        Vector3 RigidBody_horizontalVelocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
        Vector3 predictive = RigidBody_horizontalVelocity + toAdd / Time.fixedDeltaTime;
        if (ClampSpeed && predictive.magnitude >= MaximumAllowedVelocity)
        {
            toAdd = Vector3.ClampMagnitude(toAdd, MaximumAllowedVelocity);
        }
        Debug.DrawLine(transform.position, transform.position + toAdd, Color.red);
        RB.MovePosition(transform.position + toAdd); //was force
        if (ApplyDrag)
        {

            RB.MovePosition(transform.position + new Vector3(RB.velocity.x * (1 - currentArtificialDrag.x * Time.fixedDeltaTime),
                    RB.velocity.y * (1 - currentArtificialDrag.y * Time.fixedDeltaTime),
                    RB.velocity.z * (1 - currentArtificialDrag.z * Time.fixedDeltaTime)));


        }

        RigidBody_horizontalVelocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);

        //clamp after

        if (ClampSpeed && RigidBody_horizontalVelocity.magnitude >= MaximumAllowedVelocity)

        {
            RigidBody_horizontalVelocity = RigidBody_horizontalVelocity.normalized * MaximumAllowedVelocity;
            RB.MovePosition(new Vector3(RigidBody_horizontalVelocity.x, RB.velocity.y, RigidBody_horizontalVelocity.z));
        } 
        */
        #endregion
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
            Vector3 slopeForce = Vector3.down * Vector3.Dot(-SlopeCounterVectorNORMALIZED, PlayerGravity*mult);
            
            //RB.SimpleMove(SlopeCounterVector*mult+ (SlopeCounterVector.normalized * slopeForce.magnitude));
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
        if (!Physics.Raycast(towardsGround, out info, 0.3f, GroundLayers))
        {
            SlopeCounterVector = Vector3.zero;
            IsOnSlope = false;
            return;
        }
        slopeNormal = info.normal;
        SlopeCounterVectorNORMALIZED = Vector3.ProjectOnPlane(Vector3.up, info.normal).normalized;
        SlopeCounterVector = SlopeCounterVectorNORMALIZED * Vector3.Dot(-SlopeCounterVectorNORMALIZED, Physics.gravity);
        slopeAngle = VectorOps.AngleVec(Vector3.up, SlopeCounterVector.normalized);
        //Debug.Log(slopeAngle);


        IsOnSlope = slopeAngle < 90f;
        //Debug.Log(slopeAngle);


    }
    private void JumpAndGravity()
    {

        if (SoftGrounded)
        {
            //SlopeDetector();
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

                //Debug.Log("SLIDE");
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
        if (IC.isJump && jumpCooledDown && CanJumpInTutorial)
        {
            GroundcheckRepeater.StopRepeater(200);
            SoftGrounded = false;
            HardGrounded = false;
            jump = true;

            _fallTimeOut = FallTimeOutMilliseconds;
            //GroundcheckTimer.StartTimer();
            //RB.drag = AirbornStillDrag;
            //RB.Move(Mathf.Sqrt(JumpHeight * -2 * Physics.gravity.y));
            //RB.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            IC.isJump = false;

            JumpTimer.StartTimer();
        }


    }
    //public void PlayerKill
    private void StartAirborne()
    {
        //_jumpTimeOut = JumpTimeout;

        if (WantsToMove || IsDashing)
        {
            //RB.drag = 0;
            currentArtificialDrag = Vector3.zero;
        }
        else
        {
            currentArtificialDrag = AirborneDragVector;
        }
        velocityMultiplier = AirborneVelocityMul;

        //canSlide = false;
        //SlideTimer.StopTimer();


        if (IC.isJump && DoubleJumpPossible && CanJumpInTutorial)
        {
            //RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
            float jumpForce = Mathf.Sqrt(JumpHeight * -2 * Physics.gravity.y);
            //RB.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            DoubleJumpPossible = false;
            jump = true;
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
