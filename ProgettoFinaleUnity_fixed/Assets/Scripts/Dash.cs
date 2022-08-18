using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

public class Dash : MonoBehaviour
{
    InputCooker IC;
    Rigidbody RB;

    public LayerMask CollisionCheck;
    public float CheckRadius=1f,DistanceFactor=2f,DashForce;
    public ForceMode DashForceMode;
    private bool isDashing = false;

    Vector3 direction;
    // Start is called before the first frame update
    public bool CanDash
    {
        get
        {
            return isDashing && !Physics.CheckSphere(transform.position + direction * DistanceFactor, CheckRadius, CollisionCheck.value);
        }
    }
      
    void Start()
    {
        IC = GetComponent<InputCooker>();
        IC.PlayerPressedShift += StartDashing;
        RB = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        
        if (CanDash)
        {
            RB.AddForce(direction * DashForce * Time.fixedDeltaTime, DashForceMode);
            RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + direction * DistanceFactor, CheckRadius);
    }
    private void StartDashing()
    {
            isDashing = true;
            direction = IC.RelativeDirection;
            StartCoroutine(StopDashing());
    }
        public IEnumerator StopDashing()
        {
            yield return new WaitForSeconds(0.2f);
            direction = Vector3.zero;
            isDashing = false;
        }
}
