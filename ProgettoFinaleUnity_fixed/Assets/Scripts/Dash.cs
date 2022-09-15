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
    public LayerMask BreakableCheck;
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
            return CurrentDashCharges >0 && NoWallAhead;
        }
    }
    public bool NoWallAhead
    {
        get
        {
            return !Physics.CheckSphere(transform.position + direction * DistanceFactor, CheckRadius, CollisionCheck.value);
        }
    }
      
    void Start()
    {
        IC = GetComponent<InputCooker>();
        IC.PlayerPressedShift += StartDashing;
        RB = GetComponent<Rigidbody>();

        FPS = GetComponent<FirstPersonController>();
        FPS.PlayerStartedGrounded += () => canRechargeGrounded  = true;
        FPS.PlayerStartedGrounded += () => CheckRecharge();
        FPS.PlayerStartedAirborne += () => canRechargeGrounded = false;
        dashTimer = new SimpleTimer(DashRechargeTimeMs);
        dashTimer.TimerStartEvent += () => canRechargeTimer = false;
        dashTimer.TimerCompleteEvent += () => canRechargeTimer = true;
        dashTimer.TimerCompleteEvent += () => CheckRecharge();
        // dashTimer.TimerCompleteEvent += () => ResetDash();

        //rechargeRepeater = new Repeater(DashRechargeTimeMs);
        //rechargeRepeater.RepeaterTickEvent += () => RechargeDash(1);
    }
    private void CheckRecharge()
    {
        if(canRechargeTimer && canRechargeGrounded)
        {
            ResetDash();
        }
    }
    private void FixedUpdate()
    {  
        if (updateDash && NoWallAhead)
        {
            
            FPS.ApplyDrag = false;
            
            RB.useGravity = false;
            RB.AddForce(direction * DashForce , DashForceMode);
            //RB.AddForce(Vector3.up, ForceMode.VelocityChange);
            RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
            Collider[] thingsHit = Physics.OverlapSphere(transform.position + direction * DistanceFactor, CheckRadius, BreakableCheck.value);
            foreach (Collider x in thingsHit)
            {
                IHittable toBreak = x.gameObject.GetComponent<IHittable>();
                HitInfo hitInfo = new HitInfo();
                toBreak.OnHit(hitInfo);
            }
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
        if (CanDash && !IsDashing)
        {
            updateDash = true;
            FPS.ClampSpeed = false;
            if (IC.RelativeDirection != Vector3.zero)
                direction = IC.RelativeDirection;
            else
                direction = this.transform.forward;
            StartCoroutine(StopDashing());
            CurrentDashCharges--;
            if (!dashTimer.IsActive)
                dashTimer.StartTimer();
        }
    }
    public IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(DashDurationMs*0.001f);
        FPS.ApplyDrag = true;
        FPS.ClampSpeed = true;
        direction = Vector3.zero;
        updateDash = false;
        RB.useGravity = true;
    }
}
