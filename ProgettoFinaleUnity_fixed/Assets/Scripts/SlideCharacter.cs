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
        if (canSlide && FPS.RB.velocity.magnitude > 1 && FPS.Grounded)
        {
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
        if (!isSliding)
        {
            capsColl.height = Mathf.Lerp(capsColl.height, originalHeight, 0.2f);
        }
        else
        {
            capsColl.height = Mathf.Lerp(capsColl.height, reduceHeight, 0.2f);
            FPS.RB.velocity = new Vector3(slideDir.x * slideSpeed,FPS.RB.velocity.y, slideDir.z * slideSpeed);
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
            CDTimer.StartTimer();
        }
        
    }

}
