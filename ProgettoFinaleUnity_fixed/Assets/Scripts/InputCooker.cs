using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

public class InputCooker : MonoBehaviour
{
    public Controls Controls;
    public FirstPersonController FPS_Ctrl;

    public CinemachineVirtualCamera VirtualCamera;
    public Camera MainCamera;
    public CinemachineBrain VCameraBrain;
    public float Speed = 5f;

    [Range(1f,100f)]

    public float AimSensitivity = 1f;

    public float SprintMultiplier = 1.3f;
    private float sprintMul; 
    public Vector3 RotatedMoveValue;

    public Vector3 moveValue;
    public Vector2 inputDirection;
    public float CameraTargetPitch,PlayerTargetY;
    public bool IsShooting = false;
    public bool isJump = false;
    public bool isSprint = false;
    public delegate void PlayerShootEvent();
    public delegate void ChangeWeaponEvent();
    public delegate void PlayerRotatedCameraEvent();
    public delegate void PlayerMovementEvent();
 
    public PlayerShootEvent PlayerPressedShoot,PlayerReleasedShoot;
    public ChangeWeaponEvent NextWeapon, PreviousWeapon;
    public PlayerRotatedCameraEvent PlayerRotatedCamera;
    public PlayerMovementEvent PlayerMoved,PlayerStopped,PlayerStartedJump, PlayerStoppedJump;
    public Transform CameraHolder;
    // Start is called before the first frame update
    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public Quaternion TargetCameraRotation , TargetPlayerRotation;
    

    void Awake()
    {

        Controls = new Controls();
        Controls.Enable();
        Controls.Player.Movement.performed += OnMove;
        Controls.Player.Movement.canceled += StopMovement;
        Controls.Player.Look.performed += RotateCamera;
        Controls.Player.Shoot.started += OnShootStart;
        Controls.Player.Shoot.canceled += OnShootStop;
        Controls.Player.Jump.started += OnJump;
        Controls.Player.Jump.canceled += OnJumpCanceled;
        Controls.Player.Sprint.started += OnSprintStart;
        Controls.Player.Sprint.canceled += OnSprintStop;
        Controls.Player.WeaponScroll.performed += (context) => ChangeWeapon(context.ReadValue<float>());
        VCameraBrain = MainCamera.GetComponent<CinemachineBrain>();
        PlayerTargetY = this.transform.eulerAngles.y;
        CameraTargetPitch = VirtualCamera.transform.eulerAngles.x;
        Debug.Log("Controls initialized");
       
    }

    private void OnJumpCanceled(InputAction.CallbackContext obj)
    {
        PlayerStoppedJump.Invoke();
        isJump = false;
    }

    //// Update is called once per frame
    void Update()
    {
        RotatedMoveValue = transform.rotation * moveValue * (Speed *(1 + sprintMul));
    }
    
    public void UpdateCameras()
    {
        GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0f, PlayerTargetY, 0f));
        VirtualCamera.transform.rotation = Quaternion.Euler(CameraTargetPitch, PlayerTargetY, 0.0f);
        VCameraBrain.ManualUpdate();
    }
    public void UpdateMovement()
    {
        RotatedMoveValue = transform.rotation * moveValue * Speed;
    }
    public void OnMove(InputAction.CallbackContext value)
    {
        inputDirection = value.ReadValue<Vector2>();
        moveValue = new Vector3(inputDirection.x, 0.0f, inputDirection.y);
        PlayerMoved?.Invoke();
    }
    public void StopMovement(InputAction.CallbackContext value)
    {
        moveValue = Vector3.zero;
        PlayerStopped?.Invoke();
    }
    public void RotateCamera(InputAction.CallbackContext value)
    {
        Vector2 val = value.ReadValue<Vector2>();
        CameraTargetPitch += val.y * AimSensitivity;
        CameraTargetPitch = ClampAngle(CameraTargetPitch, -90, 90);
        //_yRotationHelper.Rotate(Vector3.right * val.y);
        //_yRotationHelper.rotation = Quaternion.Euler(new Vector3(ClampAngle(_yRotationHelper.eulerAngles.x,-180, 180),0, 0));
        //VirtualCamera.transform.rotation = Quaternion.Euler(
        //    Mathf.SmoothDampAngle(VirtualCamera.transform.eulerAngles.x, _yRotationHelper.eulerAngles.x, ref _yAngularVelocity, 0f),
        //    0f,
        //    0f);
        //Quaternion targetQuat = VirtualCamera.transform.rotation;
        //targetQuat = Quaternion.AngleAxis(val.y, Vector3.right);
        //targetQuat.eulerAngles = new Vector3(ClampAngle(targetQuat.eulerAngles.x, -90,90),0,0);

         PlayerTargetY += val.x*AimSensitivity;
       

        // VirtualCamera.transform.localRotation = Quaternion.Euler(CameraTargetPitch, 0.0f, 0.0f);
        //transform.rotation = Quaternion.Euler(0f, PlayerTargetY, 0f);




        //transform.rotation = Quaternion.Euler(
        //    0f,
        //    Mathf.SmoothDampAngle(transform.eulerAngles.y, _xRotationHelper.eulerAngles.y, ref _xAngularVelocity, 0f),
        //    0f);
        ////Quaterni

        PlayerRotatedCamera?.Invoke();
    }
   
    public void OnShootStart(InputAction.CallbackContext value)
    {
        IsShooting = true;
        if(PlayerPressedShoot!=null)
        {
            PlayerPressedShoot.Invoke();
            Debug.Log("Player started shooting");
        }
    }
    public void OnShootStop(InputAction.CallbackContext value)
    {
        IsShooting = false;
        if (PlayerReleasedShoot != null)
        {
            PlayerReleasedShoot.Invoke();
            Debug.Log("Player stopped shooting");
        }
    }
    public void OnSprintStart(InputAction.CallbackContext value)
    {
        isSprint = true;
        sprintMul = SprintMultiplier;
    }
    public void OnSprintStop(InputAction.CallbackContext value)
    {
        isSprint = false;
        sprintMul = 0;
    }
    public void OnJump(InputAction.CallbackContext value)
    {
        PlayerStartedJump?.Invoke();
        isJump = true;
    }
    public void ChangeWeapon(float value)
    {
        //Debug.Log("Scrolled?");
        if(value < 0)
        {
            Debug.Log("Next Weapon " + value);
            if(NextWeapon != null)
            NextWeapon.Invoke();
        }
        if(value >0)
        {
            Debug.Log("Prev Weapon " + value);
            PreviousWeapon?.Invoke();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }
    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }


    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


}
