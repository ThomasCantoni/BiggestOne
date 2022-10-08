using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class SlideCharacter : MonoBehaviour
{
    [Header("Slide")]
    public FMODUnity.StudioEventEmitter SoundEmitter;
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
    public Transform SlidingCameraHolder,VCamFollow;
    public Vector3 VCamOriginalFollowPosition ;
    bool canSlide = true;
    float originalHeight;
   // private float _SlideCD;
    CapsuleCollider capsColl;
    protected SimpleTimer SlidingTimer;
    protected SimpleTimer CDTimer;
    public InputCooker IC;
    public FirstPersonController FPS;
    public bool isSliding = false;
    Vector3 slideDir;
    public delegate void SlideEvents();
    public event SlideEvents StartedSLiding, StoppedSliding;
    private bool hasSubscribed=false;
    public bool NoWallAhead
    {
        get
        {
            return !Physics.CheckSphere(transform.position + slideDir * DistanceFactor, CheckRadius, CollisionCheck.value);
        }
    }
    private void OnEnable()
    {
        Subscribe();
    }
   
    private void Start()
    {
        //capsColl = GetComponent<CapsuleCollider>();
        IC = GetComponent<InputCooker>();
        FPS = GetComponent<FirstPersonController>();
        originalHeight = FPS.RB.height;
        //_SlideCD = SlideCooldown;
        
        VCamFollow = IC.VirtualCamera.Follow;
        VCamOriginalFollowPosition = VCamFollow.localPosition;
        if(!hasSubscribed)
        {
            Subscribe();
        }
    }
    public void Subscribe(bool unsubscribe=false)
    {
        if(!unsubscribe)
        {
            if (hasSubscribed)
                return;
            if(IC == null)
            {
                IC = GetComponent<InputCooker>();
            }
            IC.PlayerStartSliding += StartCoroutineSlide;
            IC.PlayerStopSliding += StopCoroutineSlide;

            //set the actions of timers 
            SlidingTimer = new SimpleTimer(slideDuration);
            SlidingTimer.TimerCompleteEvent += GoUp;

            CDTimer = new SimpleTimer(SlideCooldown);
            CDTimer.TimerCompleteEvent += SetCanSlideTrue;
            hasSubscribed = true;
        }
        else
        {
            IC.PlayerStartSliding -= StartCoroutineSlide;
            IC.PlayerStopSliding -= StopCoroutineSlide;

            //set the actions of timers 
            if(SlidingTimer != null)
                SlidingTimer.TimerCompleteEvent -= GoUp;
            if(CDTimer != null)
            CDTimer.TimerCompleteEvent -= SetCanSlideTrue;
            
            hasSubscribed = false;
        }
    }
    private void SetCanSlideTrue()
    {
        canSlide = true;
    }
    private void SetCanSlideFalse()
    {
        canSlide = false;
    }
    private void StartCoroutineSlide()
    {
        StartCoroutine(StartSliding());
    }
    private void StopCoroutineSlide()
    {
        StopCoroutine(StartSliding());
    }
    private void OnDisable()
    {
        Subscribe(true);
    }
    public IEnumerator StartSliding()
    {
        slideDir = IC.RelativeDirection;
        
        yield return new WaitForSeconds(SlideInitialTime);
        if (canSlide && FPS.WantsToMove && FPS.SoftGrounded)
        {
            StartedSLiding?.Invoke();
            SoundEmitter.Play();
            canSlide = false;
            this.isSliding = true;
            //FPS.RB.velocity = new Vector3(0,FPS.RB.velocity.y,0);
            FPS.ApplyDrag = false;
            //FPS.ClampSpeed = false;
            SlidingTimer.StartTimer();
        }
    }
    public void FixedUpdate()
    {
        if(isSliding && NoWallAhead)
        {
            float progress = SlidingTimer.Progress;
            FPS.RB.height = Mathf.Lerp(FPS.RB.height, reduceHeight, progress);
            FPS.RB.center = Vector3.Lerp(FPS.RB.center,new Vector3(0, -reduceHeight*0.5f, 0),progress);

            VCamFollow.localPosition = Vector3.Lerp(VCamFollow.localPosition, SlidingCameraHolder.localPosition, progress);
            Debug.Log(SlidingTimer.Progress);

            FPS.RB.Move(new Vector3(slideDir.x * slideSpeed,0, slideDir.z * slideSpeed)*Time.fixedDeltaTime);
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
            FPS.RB.height = Mathf.Lerp(FPS.RB.height, originalHeight, 0.2f);
            FPS.RB.center = Vector3.Lerp(FPS.RB.center, new Vector3(0, 0, 0),0.2f);
            VCamFollow.localPosition = Vector3.Lerp(VCamFollow.localPosition, VCamOriginalFollowPosition, 0.2f);
        }
    }
    private void GoUp()
    {
        if (isSliding)
        {
            FPS.ApplyDrag = true;
            //FPS.ClampSpeed = true;
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
