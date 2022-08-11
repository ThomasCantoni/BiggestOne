using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WallParkour : MonoBehaviour
{
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallClimbSpeed;
    public float maxWallRunForce;
    private float wallRunTimer;

    private float horizontalInput;
    private float verticalInput;
    public KeyCode upwardsRunKey = KeyCode.Q;
    public KeyCode downwardsRunKey = KeyCode.E;

    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;
    private bool upwardsRunning;
    private bool downwardsRunning;

    public Transform orientation;
    private FirstPersonController fps;
    private InputCooker ic;
    private Rigidbody rb;
    private Vector3 wallNormal, wallForward;
    private bool checkWall
    {
        get { return checkWallCurrentCooldown <= 0; }
        
    }
    public float checkWallCooldown, checkWallCurrentCooldown;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fps = GetComponent<FirstPersonController>();
        ic = GetComponent<InputCooker>();
    }
    private void Update()
    {
        if(!checkWall)
        { 
            checkWallCurrentCooldown -= Time.unscaledDeltaTime;
            return;
        }
            CheckForWall();
        StateMachine();
    }
    private void FixedUpdate()
    {
        if (fps.wallRunning)
            WallRunningMovement();
    }
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }
    private void StateMachine()
    {
        horizontalInput = ic.moveValue.x;
        verticalInput = ic.moveValue.z;
        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        if ((wallLeft || wallRight) && verticalInput > 0 && !fps.Grounded)
        {
            if (!fps.wallRunning)
                StartWallRun();
        }
        else
        {
            if (fps.wallRunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        fps.wallRunning = true;
        ic.PlayerJump += JumpOffWall;
    }
    public void JumpOffWall()
    {
        
        rb.AddForce(wallNormal*10f, ForceMode.VelocityChange);
        StopWallRun();
    }
    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (upwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        if (downwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);


        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);

        
    }

    private void StopWallRun()
    {
        checkWallCurrentCooldown = checkWallCooldown;
        fps.wallRunning = false;
        rb.useGravity = true;
        ic.PlayerJump -= JumpOffWall;
    }
}

