using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlideCharacter : MonoBehaviour
{
    [Header("Slide")]
    [Tooltip("Time before the Player can slide again.")]
    public float SlideInitialTime = 0.15f;
    public float SlideCooldown = 200f;
    public float slideDuration;
    public float reduceHeight;
    public float slideSpeed = 10f;
    public float DistanceFactor;
    public float CheckRadius;
    public LayerMask CollisionCheck;
    public LayerMask BreakableCheck;

    bool canSlide = true;
    float originalHeight;
    private float _SlideCD;
    CapsuleCollider capsColl;
    protected SimpleTimer SlidingTimer;
    protected SimpleTimer CDTimer;
    public InputCooker IC;
    public FirstPersonController FPS;
    public bool isSliding = false;
    Vector3 slideDir;
    public delegate void SlideEvents();
    public event SlideEvents StartedSLiding, StoppedSliding;
    public bool NoWallAhead
    {
        get
        {
            return !Physics.CheckSphere(transform.position + slideDir * DistanceFactor, CheckRadius, CollisionCheck.value);
        }
    }
    private void Start()
    {
        capsColl = GetComponent<CapsuleCollider>();
        IC = GetComponent<InputCooker>();
        FPS = GetComponent<FirstPersonController>();
        originalHeight = capsColl.height;
        _SlideCD = SlideCooldown;

        IC.PlayerStartSliding += () => StartCoroutine(StartSliding());
        IC.PlayerStopSliding += () => StopCoroutine(StartSliding());

        //set the actions of timers 
        SlidingTimer = new SimpleTimer(slideDuration);
        SlidingTimer.TimerCompleteEvent += GoUp;

        CDTimer = new SimpleTimer(_SlideCD);
        CDTimer.TimerCompleteEvent += () => canSlide = true;
    }
    public IEnumerator StartSliding()
    {
        slideDir = IC.RelativeDirection;
        yield return new WaitForSeconds(SlideInitialTime);
        if (canSlide && FPS.RB.velocity.magnitude > 1 && FPS.SoftGrounded)
        {
            StartedSLiding?.Invoke();
            canSlide = false;
            this.isSliding = true;
            FPS.RB.velocity = new Vector3(0,FPS.RB.velocity.y,0);
            FPS.ApplyDrag = false;
            FPS.ClampSpeed = false;
            SlidingTimer.StartTimer();
        }
    }
    public void FixedUpdate()
    {
        if(isSliding && NoWallAhead)
        {
            capsColl.height = Mathf.Lerp(capsColl.height, reduceHeight, 0.2f);
            FPS.RB.velocity = new Vector3(slideDir.x * slideSpeed,FPS.RB.velocity.y, slideDir.z * slideSpeed);
            Collider[] thingsHit = Physics.OverlapSphere(transform.position + slideDir * DistanceFactor, CheckRadius, BreakableCheck.value);
            foreach (Collider x in thingsHit)
            {
                HitEventFracture checkIfFracture = x.GetComponent<HitEventFracture>();
                if (checkIfFracture != null)
                {
                    if (checkIfFracture.FractureType == FractureType.Slide)
                    {
                        // if the wall has the same fractureMask, then keep going


                        HitInfo hitInfo = new HitInfo();
                        hitInfo.FractureInfo.collisionPoint = this.transform.position;
                        hitInfo.FractureInfo.FractureType = FractureType.Slide;
                        checkIfFracture.OnHit(hitInfo);
                        Debug.Log("WALL SLIDE!");

                    }
                    else // else stop sliding 
                    {
                        GoUp();
                        Debug.Log("STOP SLIDING");

                    }

                }
            }
        }
        else
        {
            capsColl.height = Mathf.Lerp(capsColl.height, originalHeight, 0.2f);
        }
    }
    private void GoUp()
    {
        if (isSliding)
        {
            FPS.ApplyDrag = true;
            FPS.ClampSpeed = true;
            SlidingTimer.StopTimer(false);
            this.isSliding = false;
            StoppedSliding?.Invoke();
            CDTimer.StartTimer();
        }
        
    }

    private void OnDrawGizmos()
    {
        bool wall = NoWallAhead;
        if (!wall) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + slideDir*DistanceFactor,CheckRadius);
        

    }
}
