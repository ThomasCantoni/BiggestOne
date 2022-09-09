using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlideCharacter : MonoBehaviour
{
    [Header("Slide")]
    [Tooltip("Time before the Player can slide again.")]
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

        IC.PlayerStartSliding += StartSliding;
        IC.PlayerStopSliding += GoUp;

        //set the actions of timers 
        SlidingTimer = new SimpleTimer(slideDuration);
        SlidingTimer.TimerCompleteEvent += GoUp;

        CDTimer = new SimpleTimer(_SlideCD);
        CDTimer.TimerCompleteEvent += () => canSlide = true;
    }
    public void StartSliding()
    {
        if (canSlide && FPS.RB.velocity.magnitude > 1)
        {
            canSlide = false;
            slideDir = IC.RelativeDirection;
            this.isSliding = true;
            capsColl.height = reduceHeight;
            FPS.RB.drag = FPS.AirbornStillDrag;
            FPS.RB.AddForce(slideDir * slideSpeed, ForceMode.VelocityChange);
            SlidingTimer.StartTimer();
        }
    }
    private void GoUp()
    {
        if (isSliding)
        {
            SlidingTimer.StopTimer(false);
            this.isSliding = false;
            CDTimer.StartTimer();
            capsColl.height = originalHeight;
        }
        
    }
}
