using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

public class Dash : MonoBehaviour
{
    InputCooker IC;
    Rigidbody RB;
    FirstPersonController FPS;
    public LayerMask CollisionCheck;
    public float CheckRadius=1f,DistanceFactor=2f,DashForce;
    public ForceMode DashForceMode;
    public int DashDurationMs = 200;
    public int DashRechargeTimeMs = 1000;
    private SimpleTimer dashTimer;
    private Repeater rechargeRepeater;
    public int DashMaxCharges = 1;
    public int CurrentDashCharges = 1;
    private bool updateDash = false;
    public bool canRechargeTimer=true, canRechargeGrounded;
    public bool IsDashing
    {
        get { return updateDash; }
    }
    Vector3 direction;
    // Start is called before the first frame update
    public bool CanDash
    {
        get
        {
            return CurrentDashCharges >0 && updateDash && !Physics.CheckSphere(transform.position + direction * DistanceFactor, CheckRadius, CollisionCheck.value);
        }
    }
      
    void Start()
    {
        IC = GetComponent<InputCooker>();
        IC.PlayerPressedShift += StartDashing;
        RB = GetComponent<Rigidbody>();

        FPS = GetComponent<FirstPersonController>();
        FPS.PlayerStartedGrounded += () => canRechargeGrounded  = true;
        FPS.PlayerStartedAirborne += () => canRechargeGrounded = false;
        dashTimer = new SimpleTimer(DashRechargeTimeMs);
        dashTimer.TimerStartEvent += () => canRechargeTimer = false;
        dashTimer.TimerCompleteEvent += () => canRechargeTimer = true;
        dashTimer.TimerCompleteEvent += () => ResetDash();

        //rechargeRepeater = new Repeater(DashRechargeTimeMs);
        //rechargeRepeater.RepeaterTickEvent += () => RechargeDash(1);
    }
    private void Update()
    {
        if(canRechargeTimer && canRechargeGrounded)
        {
            ResetDash();
        }
    }
    private void FixedUpdate()
    {  
        if (CanDash)
        {
            RB.drag = 0;
            RB.AddForce(direction * DashForce , DashForceMode);
            RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + direction * DistanceFactor, CheckRadius);
    }
    public void ResetDash()
    {
        dashTimer.StopTimer(false);
        CurrentDashCharges =  DashMaxCharges;
    }
    public void RechargeDash(int chargesToRecharge)
    {
        CurrentDashCharges += chargesToRecharge;
        CurrentDashCharges = Mathf.Clamp(CurrentDashCharges, 0, DashMaxCharges);
    }
    private void StartDashing()
    {
        updateDash = true;
        direction = IC.RelativeDirection;
        StartCoroutine(StopDashing());
        CurrentDashCharges--;
        if (!dashTimer.IsActive)
            dashTimer.StartTimer();
    }
    public IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(DashDurationMs*0.001f);
        
        direction = Vector3.zero;
        updateDash = false;
    }
}
