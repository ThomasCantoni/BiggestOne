using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class InputCooker : MonoBehaviour
{
    public Controls Controls;
    public FirstPersonController FPS_Ctrl;

    public CinemachineVirtualCamera VirtualCamera;
    public Camera MainCamera;
    public float Speed = 5f;

    [Range(1f,50f)]

    public float AimSensitivity = 1f;

    public float SprintMultiplier = 1.3f;
    private float sprintMul; 
    public Vector3 RotatedMoveValue;

    Vector3 moveValue;
    Vector2 inputDirection;
    float CameraTargetPitch;
    public bool IsShooting = false;
    public bool isJump = false;
    public bool isSprint = false;
    public delegate void PlayerShootEvent();
    public delegate int ChangeWeaponEvent();
    public PlayerShootEvent PlayerPressedShoot,PlayerReleasedShoot;
    public ChangeWeaponEvent NextWeapon, PreviousWeapon;
    // Start is called before the first frame update
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
        Controls.Player.Sprint.started += OnSprintStart;
        Controls.Player.Sprint.canceled += OnSprintStop;
        Controls.Player.WeaponScroll.performed += (context) => ChangeWeapon(context.ReadValue<float>());
        Debug.Log("Controls initialized");
    }

    //// Update is called once per frame
    void Update()
    {
        RotatedMoveValue = transform.rotation * moveValue * (Speed *(1 + sprintMul));
    }
    public void UpdateMovement()
    {
        RotatedMoveValue = transform.rotation * moveValue * Speed;
    }
    public void OnMove(InputAction.CallbackContext value)
    {
        inputDirection = value.ReadValue<Vector2>();
        moveValue = new Vector3(inputDirection.x, 0.0f, inputDirection.y); 
    }
    public void StopMovement(InputAction.CallbackContext value)
    {
        moveValue = Vector3.zero;
    }
    public void RotateCamera(InputAction.CallbackContext value)
    {
        Vector2 val = value.ReadValue<Vector2>();

        CameraTargetPitch += val.y * AimSensitivity;
        CameraTargetPitch = ClampAngle(CameraTargetPitch, -90, 90);
        VirtualCamera.transform.localRotation = Quaternion.Euler(CameraTargetPitch, 0.0f, 0.0f);

        float rotationVelocity = val.x * AimSensitivity;
        transform.Rotate(Vector3.up * rotationVelocity);
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
        isJump = true;
    }
    public void ChangeWeapon(float value)
    {
        Debug.Log("Scrolled?");
        if(value < 0)
        {
            Debug.Log("Next Weapon");
        }
        if(value >0)
        {
            Debug.Log("Prev Weapon");

        }
    }
    

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


}
