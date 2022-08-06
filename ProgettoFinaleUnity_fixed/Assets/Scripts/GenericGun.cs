using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class GenericGun : MonoBehaviour
{
    [SerializeField]
    public IHittableInformation HitInfo;

    public LayerMask Mask;
    public bool IsAutomatic;
    public float FireRate;
    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 1f;
    public bool isReloading = false;
    public Animator anim;
    public int Magazine;

    protected InputCooker InputCooker;
    public GameObject ToInstantiate;
    //public UnityEvent ShootEvent;
   
    protected bool hasShotOnce, shooting;
    protected float currentShootCD = 0;

    public bool CanShoot
    {
        get { return currentShootCD <= 0f; }
    }
    public bool HasShotOnce
    {
        get { return hasShotOnce; }
    }

    protected float shootCD
    {
        get { return Mathf.Clamp( 1f / FireRate, 0f, 9999f); }
        set
        {
            FireRate = 1f / Mathf.Clamp(value, 0f, 9999f);
        }
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        InputCooker = transform.GetComponentInParent<InputCooker>();
        InputCooker.PlayerPressedShoot += Shoot;
        currentAmmo = maxAmmo;
        if (HitInfo.sender == null)
        {
            HitInfo.sender = this.gameObject;
        }
    }
    private void OnEnable()
    {
        isReloading = false;
        anim.SetBool("Reloading", false);
    }

    public virtual void Shoot()
    {
    }
    public virtual void Update()
    {
        
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        anim.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - .25f);
        anim.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    public virtual void ShootRay()
    {
        currentAmmo--;
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (!CanShoot) return;
        Vector2 screenCenterPoint = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Camera cam = InputCooker.MainCamera;
        Ray ray = cam.ScreenPointToRay(screenCenterPoint);
        RaycastHit info;
        if (Physics.Raycast(ray, out info, 100f, Mask.value))
        {
            if (ToInstantiate != null)
                Destroy(Instantiate(ToInstantiate, info.point, Quaternion.identity), 2f);
            if (info.collider.GetComponent<IHittable>() != null)
            {
                HitInfo.raycastInfo = info;
                info.collider.GetComponent<HitEvent>().OnHit(HitInfo);
            }
        }
        currentShootCD = shootCD;
    }
}
