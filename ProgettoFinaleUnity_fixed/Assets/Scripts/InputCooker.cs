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
    public Vector3 RotatedMoveValue;
    
    Vector3 moveValue;
    Vector2 inputDirection;
    float CameraTargetPitch;
    public bool IsShooting = false;
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
        Debug.Log("Controls initialized");
    }

    //// Update is called once per frame
    void Update()
    {
        RotatedMoveValue = transform.rotation * moveValue;

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

        CameraTargetPitch += val.y * 1f;
        CameraTargetPitch = ClampAngle(CameraTargetPitch, -90, 90);
        VirtualCamera.transform.localRotation = Quaternion.Euler(CameraTargetPitch, 0.0f, 0.0f);

        float rotationVelocity = val.x * 1f;
        transform.Rotate(Vector3.up * rotationVelocity);
       

    }

    public void OnShootStart(InputAction.CallbackContext value)
    {
        IsShooting = true;
    }
    public void OnShootStop(InputAction.CallbackContext value)
    {
        IsShooting = false;
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
