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
        if (canSlide && FPS.RB.velocity.magnitude > 1)
        {
            canSlide = false;
            this.isSliding = true;
            capsColl.height = reduceHeight;
            FPS.RB.velocity = Vector3.zero;
            FPS.ApplyDrag = false;
            FPS.ClampSpeed = false;
            SlidingTimer.StartTimer();
        }
    }
    public void FixedUpdate()
    {
        if (!isSliding)
            return;
        FPS.RB.velocity = slideDir * slideSpeed;


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
            capsColl.height = originalHeight;
        }
        
    }



}
