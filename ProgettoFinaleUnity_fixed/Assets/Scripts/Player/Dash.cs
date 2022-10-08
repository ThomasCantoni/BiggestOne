using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

public class Dash : MonoBehaviour
{
    InputCooker IC;
    CharacterController RB;
    FirstPersonController FPS;
    public LayerMask CollisionCheck;
    public LayerMask BreakableCheck;
    public float CheckRadius=1f,DistanceFactor=2f,DashForce;
    //public ForceMode DashForceMode;
    public int DashDurationMs = 200;
    public int DashRechargeTimeMs = 1000;
    [SerializeField]
    private SimpleTimer DashRechargeTimer;
    
    public int DashMaxCharges = 1;
    public int CurrentDashCharges = 1;
    private bool updateDash = false;
    public bool canRechargeTimer=true, canRechargeGrounded;
    public bool DashEnabled = true;
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
        RB = GetComponent<CharacterController>();

        FPS = GetComponent<FirstPersonController>();
        FPS.PlayerStartedGrounded += () => canRechargeGrounded  = true;
        FPS.PlayerStartedGrounded += () => CheckRecharge();
        FPS.PlayerStartedAirborne += () => canRechargeGrounded = false;
        DashRechargeTimer = new SimpleTimer(DashRechargeTimeMs);
        DashRechargeTimer.TimerStartEvent += () => canRechargeTimer = false;
        DashRechargeTimer.TimerCompleteEvent += () => canRechargeTimer = true;
        DashRechargeTimer.TimerCompleteEvent += () => CheckRecharge();
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
        if (!DashEnabled)
        {
            StopDashingImmediately();
            return;
        }
        if (updateDash && NoWallAhead)
        {
            FPS.ApplyDrag = false;
            FPS.ApplyGravity = false;
            //RB.useGravity = false;
            RB.Move(direction * DashForce*Time.fixedDeltaTime);
            //RB.AddForce(Vector3.up, ForceMode.VelocityChange);
            //RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
            
            Collider[] thingsHit = Physics.OverlapSphere(transform.position + direction * DistanceFactor, CheckRadius, BreakableCheck.value);
            foreach (Collider x in thingsHit)
            {
                HitEventFracture checkIfFracture = x.GetComponent<HitEventFracture>();
                if(checkIfFracture != null )
                {
                    if(checkIfFracture.FractureType == FractureType.Shot_and_dash)
                    {// if the wall has the same fractureMask, then keep going

                        
                            HitInfo hitInfo = new HitInfo();
                            hitInfo.FractureInfo.collisionPoint = this.transform.position;
                            hitInfo.FractureInfo.FractureType = FractureType.Shot_and_dash;
                            checkIfFracture.OnHit(hitInfo);

                        
                    }
                    else // else stop dashing (fixes clipping into the object)
                    {
                        StopDashingImmediately();
                        Debug.Log("STOP DASHING");
                        
                    }

                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + direction * DistanceFactor, CheckRadius);
    }
    public void ResetDash()
    {
        DashRechargeTimer.StopTimer(false);
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
            if (!DashRechargeTimer.IsActive)
                DashRechargeTimer.StartTimer();
        }
    }
    public IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(DashDurationMs*0.001f);
        FPS.ApplyDrag = true;
        FPS.ClampSpeed = true;
        direction = Vector3.zero;
        updateDash = false;
        FPS.ApplyGravity= true;
    }
    public void StopDashingImmediately()
    {
        FPS.ApplyDrag = true;
        FPS.ClampSpeed = true;
        direction = Vector3.zero;
        updateDash = false;
        FPS.ApplyGravity = true;
    }
}
