using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WallParkour : MonoBehaviour
{
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float JumpOffForce;
    public float wallRunForce;
    public float wallClimbSpeed;
    public float maxWallRunForce;
    private float wallRunTimer;

    private float horizontalInput;
    private float verticalInput;
    public bool IsWallRunning = false;

    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;

    private RaycastHit pushHit;

    private bool wallLeft;
    private bool wallRight;
    private bool towardsWall;
    private bool upwardsRunning;
    private bool downwardsRunning;

    public Transform PlayerTransform;
    private FirstPersonController fps;
    private InputCooker ic;
    private Rigidbody rb;
    private Vector3 wallNormal, wallForward, towardsWallVector;
    private bool playerIsHoldingSpace;
    private bool pushingTowardsWall;
    private bool hitWall;
    private SimpleTimer gainDragTimer;
    private bool checkWall
    {
        get { return checkWallCurrentCooldown <= 0; }

    }
    private bool isJumpingOffWall = false;
    public float checkWallCooldown, checkWallCurrentCooldown;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fps = GetComponent<FirstPersonController>();
        ic = GetComponent<InputCooker>();
        ic.PlayerStartedJump += () => playerIsHoldingSpace = true;
        ic.PlayerStoppedJump += () => playerIsHoldingSpace = false;

        gainDragTimer = new SimpleTimer(1000);
        gainDragTimer.TimerStartEvent += () => isJumpingOffWall = true;
        gainDragTimer.TimerStartEvent += () => fps.ApplyDrag = false;
        gainDragTimer.TimerCompleteEvent += () => fps.ApplyDrag = true;
        gainDragTimer.TimerCompleteEvent += () => isJumpingOffWall = false;


        fps.PlayerStartedGrounded += gainDragTimer.StopTimer;
    }
    private void Update()
    {
        ic.VirtualCamera.m_Lens.Dutch = Mathf.Lerp(ic.VirtualCamera.m_Lens.Dutch, 0f, 0.05f);
        if (!checkWall)
        {
            checkWallCurrentCooldown -= Time.unscaledDeltaTime;
            return;
        }
        
        CheckForWall();
        StateMachine();
    }
    private void FixedUpdate()
    {
        if (IsWallRunning)
            WallRunningMovement();
        if(isJumpingOffWall)
        {
            Vector3 trimmedForward = ic.VirtualCamera.transform.forward;
            trimmedForward.y = 0;
            rb.AddForce(trimmedForward * JumpOffForce, ForceMode.Acceleration);
        }

        
    }
    private void CheckForWall()
    {
        Vector2 direction = ic.RelativeDirection.normalized;
        if (playerIsHoldingSpace && !IsWallRunning)
        {
            pushingTowardsWall = Physics.Raycast(transform.position, direction, out pushHit, 1f, whatIsWall);
            wallRight = Physics.Raycast(transform.position, PlayerTransform.right, out rightWallHit, wallCheckDistance, whatIsWall);
            wallLeft = Physics.Raycast(transform.position, -PlayerTransform.right, out leftWallHit, wallCheckDistance, whatIsWall);

            if (pushingTowardsWall)
                towardsWallVector = -pushHit.normal;
            if(wallRight)
                towardsWallVector = -rightWallHit.normal;

            if (wallLeft)
                towardsWallVector = -leftWallHit.normal;

            hitWall = pushingTowardsWall || wallRight || wallLeft;

            //towardsWall = Physics.Raycast(transform.position, towardsWallVec, wallCheckDistance, whatIsWall);

            wallNormal = -towardsWallVector;
            wallForward = Vector3.Cross(wallNormal, transform.up);
            if (VectorOps.AngleVec(PlayerTransform.forward, wallForward) > 90f)
                wallForward *= -1f;
        }
        if (IsWallRunning)
        {
            towardsWall = Physics.Raycast(transform.position,towardsWallVector, wallCheckDistance, whatIsWall);
        }
    }

    private void StateMachine()
    {
        //horizontalInput = ic.moveValue.x;
        //verticalInput = ic.moveValue.z;
        // upwardsRunning = Input.GetKey(upwardsRunKey);
        // downwardsRunning = Input.GetKey(downwardsRunKey);
        if (fps.SoftGrounded || !playerIsHoldingSpace)
        {
            //Debug.Log("PLAYER NOT GROUNDED, ABORTING WALL RUN");
            StopWallRun();
            return;
        }


        if (!IsWallRunning && hitWall)
        {

            StartWallRun();
            return;
        }
        else if (IsWallRunning && !towardsWall)
        {

            StopWallRun();
        }
        if (IsWallRunning && wallRight)
        {
            ic.VirtualCamera.m_Lens.Dutch = Mathf.Lerp(ic.VirtualCamera.m_Lens.Dutch, 35f, 0.05f);
        }
        if (IsWallRunning && wallLeft)
        {
            ic.VirtualCamera.m_Lens.Dutch = Mathf.Lerp(ic.VirtualCamera.m_Lens.Dutch, -35f, 0.05f);
        }
    }

    private void StartWallRun()
    {
        IsWallRunning = true;
        ic.PlayerStoppedJump += JumpOffWall;
    }
    
    public void JumpOffWall()
    {
        
        gainDragTimer.StartTimer();
        fps.DoubleJumpPossible = true;



        StopWallRun();
    }
    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(0, 0, 0);
        // wallNormal = playerPushesAgainstWall ? pushHit.normal : pushHit.normal;
        //wallForward = Vector3.Cross(wallNormal, transform.up);

        //if the player stops looking at the wall...
        //if ((PlayerTransform.forward - wallForward).magnitude > (PlayerTransform.forward - -wallForward).magnitude)
        //    wallForward = -wallForward;

        //if player is looking in the opposite direction of the wall forward, then change the force accordingly
        //if (VectorOps.AngleVec(PlayerTransform.forward, wallForward) > 90f)
        //    wallForward *= -1f;

        //rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // rb.AddForce(ic.VirtualCamera.transform.forward*200f,ForceMode.Force);

        //    if (upwardsRunning)
        //rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        //if (downwardsRunning)
        //rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);

        //push towards wall 
        if (playerIsHoldingSpace)
            rb.AddForce(-wallNormal * 100, ForceMode.Force);


    }

    private void StopWallRun()
    {
        checkWallCurrentCooldown = checkWallCooldown;
        IsWallRunning = false;
        rb.useGravity = true;
        ic.PlayerStoppedJump -= JumpOffWall;
    }
}

