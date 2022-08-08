using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fps = GetComponent<FirstPersonController>();
        ic = GetComponent<InputCooker>();
    }
    private void Update()
    {
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
        horizontalInput = ic.inputDirection.x;
        verticalInput = ic.inputDirection.y;
        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
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
    }
    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

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
        fps.wallRunning = false;
        rb.useGravity = true;
    }
}

