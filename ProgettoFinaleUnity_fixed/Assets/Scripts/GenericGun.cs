using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericGun : MonoBehaviour
{
    [SerializeField]
    public IHittableInformation HitInfo;
    public LayerMask Mask;
    public bool IsAutomatic;
    public float FireRate;
    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 1f;
    public bool IsReloading
    {
        get
        {
            return isReloading;
        }
        set
        {
            isReloading = value;
            anim.SetBool("Reloading", isReloading);

        }
    }
    public bool isReloading = false;
    public Animator anim;
    public int Magazine;
    public WeaponSwitching WS;
    protected InputCooker InputCooker;
    public GameObject ToInstantiate;
    //public UnityEvent ShootEvent;
    protected GameObject player;
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
        get { return Mathf.Clamp(1f / FireRate, 0f, 9999f); }
        set
        {
            FireRate = 1f / Mathf.Clamp(value, 0f, 9999f);
        }
    }
    private void OnEnable()
    {
        Initialize();
        IsReloading = false;
        anim.SetFloat("ReloadTime", 1f/reloadTime);
        Subscribe(true);
    }
    private void OnDisable()
    {
        Subscribe(false);
    }
   
    void Initialize()
    {
        if(player == null || WS == null || InputCooker == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            WS = gameObject.GetComponentInParent<WeaponSwitching>();
            InputCooker = player.GetComponentInChildren<InputCooker>();

        }
    }
    public virtual void Start()
    {
        currentAmmo = maxAmmo;
        if (HitInfo.sender == null)
        {
            HitInfo.sender = this.gameObject;
        }
        //OnEnable();
    }
    private void Subscribe(bool subscribe)
    {
        if(subscribe)
        {
            InputCooker.PlayerPressedShoot += Shoot;
            
                WS.ReloadEvent += EndReload;
           // Debug.Log("Subbed " +gameObject.name);

        }
        else
        {
            // Debug.Log("Unsubbed "+gameObject.name);
            InputCooker.PlayerPressedShoot -= Shoot;
            
            WS.ReloadEvent -= EndReload;
        }
    }

    public virtual void Shoot()
    {
    }
    public virtual void Update()
    {
    }
    public void EndReload()
    {
        currentAmmo = maxAmmo;
        IsReloading = false;
        
    }

    public void StartReload()
    {
        isReloading = true;
        
        anim.SetBool("Reloading", true);
        
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
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartReload();
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
                HitInfo.EnemyHit = info.collider.gameObject;
                info.collider.GetComponent<HitEvent>().OnHit(HitInfo);
                foreach(ChainableAttack x in HitInfo.PlayerAttackEffects.Attacks)
                {
                    x.Apply(HitInfo);
                }
                //Debug.Log("Shoot");
            }
        }
        currentAmmo--;
        currentShootCD = shootCD;
    }
}
