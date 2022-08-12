using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
public class Dash : MonoBehaviour
{
    InputCooker IC;
    Rigidbody RB;
    private bool isDashing = false;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        IC = GetComponent<InputCooker>();
        IC.PlayerPressedShift += StartDashing;
        RB = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            RB.AddForce(direction * 150 * Time.fixedDeltaTime, ForceMode.Acceleration);
            RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
        }
    }
    
        private void StartDashing()
        {
            isDashing = true;
            direction = IC.RotatedMoveValue;
            StartCoroutine(StopDashing());
        }
        public IEnumerator StopDashing()
        {
            yield return new WaitForSeconds(0.2f);
            direction = Vector3.zero;
            isDashing = false;
        }

       
    
    

}
