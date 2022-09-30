using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Cinemachine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField]
    public HitInfo HitInfo;
    InputCooker InputCooker;
    public GameObject ToInstantiate;
    public UnityEvent ShootEvent;
    public bool AutoFire = false;



    public bool CanShoot
    {
        get { return currentShootCD <= 0f; }
    }
    public bool HasShotOnce
    {
        get { return hasShotOnce; }
    }

    public float FireRate
    {
        get { return 1f / shootCD; }
        set
        {
            shootCD = 1f / value;
        }
    }
    private bool hasShotOnce, shooting;
    private float shootCD = 1f, currentShootCD = 0;
    // Start is called before the first frame update
    void Start()
    {
        FireRate = 1f;
        InputCooker = transform.GetComponentInParent<InputCooker>();
        InputCooker.Controls.Player.Shoot.performed += OnShoot;
        InputCooker.PlayerPressedShoot += Shoot;
        //if (HitInfo.sender == null)
        //{
        //    HitInfo.sender = this.gameObject;
        //}
    }

    public virtual void Shoot()
    {
    }
    private void Update()
    {
        if (!CanShoot)
        {
            currentShootCD -= Time.deltaTime;
            return;
        }
        if (AutoFire && InputCooker.IsShooting)
        {
            if (currentShootCD <= 0f)
            {
                ShootRay();
            }
        }

    }

    public void ShootRay()
    {
        if (!CanShoot)
        {
            return;
        }
        Camera cam = InputCooker.MainCamera;
        RaycastHit info;
        Vector2 screenCenterPoint = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray ray = cam.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out info, 100f, 3))
        {
            if (ToInstantiate != null)
                Destroy(Instantiate(ToInstantiate, info.point, Quaternion.identity), 2f);

            if (info.collider.GetComponent<IHittable>() != null)
            {
                HitInfo.FractureInfo.collisionPoint = info.point;
                info.collider.GetComponent<HitEvent>().OnHit(HitInfo);
            }
        }
        currentShootCD = shootCD;

    }
    public void OnShoot(InputAction.CallbackContext ctx)
    {
        ShootEvent.Invoke();
        hasShotOnce = true;
        shooting = true;
    }
}
