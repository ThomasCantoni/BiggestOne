using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
   
    public CharacterController Controller;
    public InputCooker InputCooker;
    public float Speed = 5f;
    
   
    void Update()
    {
       
        Controller.Move(InputCooker.RotatedMoveValue * Time.deltaTime);
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    Controls = new Controls();
    //    Controls.Enable();
    //    Controls.Player.Movement.performed += OnMove;
    //    Controls.Player.Movement.canceled += StopMovement;
    //    Controls.Player.Look.performed += RotateCamera;

    //}

    // Update is called once per frame
   /* public void MoveUpdate()
    {
        moveValue = new Vector3(inputDirection.x, 0.0f, inputDirection.y);

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (inputDirection != Vector2.zero)
        {
            // move
            moveValue = transform.rotation * moveValue;
        }
        Controller.Move(moveValue * Time.deltaTime);

    }
    public void OnMove(InputAction.CallbackContext value)
    {
        inputDirection =  value.ReadValue<Vector2>();
        
    }

    public void StopMovement(InputAction.CallbackContext value)
    {
        inputDirection = Vector3.zero;
    }
    public void RotateCamera(InputAction.CallbackContext value)
    {
        Vector2 val = value.ReadValue<Vector2>();
       
        CameraTargetPitch += val.y * 1f;
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
   */
}
