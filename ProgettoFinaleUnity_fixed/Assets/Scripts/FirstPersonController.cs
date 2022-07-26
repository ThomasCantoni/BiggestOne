using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    public Controls Controls;
    public CharacterController Controller;
    public GameObject MainCamera;
    public float Speed = 5f;
    Vector3 moveValue;
    float CameraTargetPitch;
    // Start is called before the first frame update
    void Start()
    {
        Controls = new Controls();
        Controls.Enable();
        Controls.Player.Movement.performed += MoveUpdate;
        Controls.Player.Movement.canceled += StopMovement;
        Controls.Player.Look.performed += RotateCamera;

    }

    // Update is called once per frame
    void Update()
    {
        Controller.Move(moveValue);
        
    }
    public void MoveUpdate(InputAction.CallbackContext value)
    {
        Vector2 val = MainCamera.transform.rotation * value.ReadValue<Vector2>();
        moveValue = new Vector3(val.x, 0, val.y) * Time.deltaTime * Speed;
    }
    public void StopMovement(InputAction.CallbackContext value)
    {
        moveValue = Vector3.zero;
    }
    public void RotateCamera(InputAction.CallbackContext value)
    {
        Vector2 val = value.ReadValue<Vector2>();
       
        CameraTargetPitch += -val.y * 1f;
        CameraTargetPitch= ClampAngle(CameraTargetPitch, -90, 90);
        MainCamera.transform.localRotation = Quaternion.Euler(CameraTargetPitch, 0.0f, 0.0f);
    
        float rotationVelocity = val.x * 1f ;
        transform.Rotate(Vector3.up * rotationVelocity);

        
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
