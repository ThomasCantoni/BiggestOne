using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Cinemachine;

public class WeaponScript : MonoBehaviour
{
    public ParticleSystem ToActivate;
    public InputCooker InputCooker;
    public UnityEvent ShootActivated;
    public GameObject ToInstantiate;
    public bool automatic = false;
    private bool shooting;
    public float FireRate
    {
        get { return 1f / shootCD; }
        set
        {
            shootCD = 1f / value;
        }
    }
    private float shootCD=0, currentShootCD=0;
    // Start is called before the first frame update
    void Start()
    {
        FireRate = 13f;
        InputCooker = transform.GetComponentInParent<InputCooker>();
        InputCooker.Controls.Player.Shoot.performed += OnShoot;
        
    }
    private void Update()
    {
        if(automatic && InputCooker.IsShooting)
        {
            if(currentShootCD <=0f)
            {
                ShootRay();
                currentShootCD = shootCD;
            }
            else
            {
                currentShootCD -= Time.deltaTime;
            }
        }
        

    }
    public void ShootRay()
    {
        Camera cam = InputCooker.MainCamera;
        RaycastHit info;
        Vector2 screenCenterPoint = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray ray = cam.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out info, 10f, 3))
            Destroy(Instantiate(ToInstantiate, info.point, Quaternion.identity), 2f);
    }
    public void OnShoot(InputAction.CallbackContext ctx)
    {
        ShootActivated.Invoke();
        shooting = true;
    }
}
