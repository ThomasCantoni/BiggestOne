using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum WeaponType
{
    Rifle,Shotgun,Launcher
}
public class GenericGun : MonoBehaviour,IDamager
{
    //[SerializeField]
    //public HitInfo HitInfo;
    public LayerMask Mask;
    public WeaponType WeaponType;
    public FractureInfo FractureInformation;
    [SerializeField][Tooltip("The base stats of the weapon, should only be modified to change the stats of the weapon alone")]
    public DamageStats WeaponBaseStats;
    [SerializeField][Tooltip("The final stats of the weapon after being modified by all equipped buffs and pickups")]
    public DamageStats weaponOutputStats;
    public DamageStats OutputStats 
    {
        get
        {
            return WeaponBaseStats + weaponOutputStats;
        }
        set
        {
            weaponOutputStats = value;
        }
    }
    public DamageStats BaseStats 
    { 
        get { return WeaponBaseStats; }
        set { WeaponBaseStats= value; }
    }
    public MonoBehaviour Mono
    {
        get { return this; }
    }
    public bool IsAutomatic;
    public float FireRate;
    public int Multishot=1;
    public int MaxPenetrations = 1;
    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 1f;
    public Transform cam;
    public Animator anim;
    public WeaponSwitching WS;
    public GameObject ToInstantiate;
    //public UnityEvent ShootEvent;
    public GameObject Player;
    public PlayerAttackEffects PlayerAttackEffects;
    public FirstPersonController FPS;
    public InputCooker InputCooker;
    public TakeDamage takeDmg;
    protected bool hasShotOnce, shooting;
    protected float currentShootCD = 0;
    public bool shotgun = false;
    public float range = 50f;
    public float inaccuracyDistance = 5f;
    public bool isReloading = false;
    public bool canReload = false;
    #region PROPERTIES
    public bool CanStartReload
    {
        get { return canReload && currentAmmo < maxAmmo && !IsReloading; }
    }
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

    public bool CanShoot
    {
        get { return currentShootCD <= 0f && !isReloading && currentAmmo > 0; }
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
    #endregion


    public delegate void WeaponHitSomething(DamageInstance instance);
    public delegate void WeaponKilledSomethingEvent(GenericGun weapon,IHittable victim);

    public delegate void BulletCreatedEvent(HitInfo justCreated);
    public delegate void BulletEvent(GenericBullet justCreated);
    public delegate void ProjectileBulletCreated();
    public delegate void WeaponBeforeAndAfterShootEvent(GenericGun gunOwner);
    public delegate void WeaponShootEvent(GenericGun gunOwner, DamageInstance aboutToDeploy);
    public WeaponKilledSomethingEvent WeaponKilledSomething;
    public event WeaponHitSomething WeaponHitSomethingEvent;
    public WeaponBeforeAndAfterShootEvent BeforeShoot, AfterShoot;
    public event WeaponShootEvent OnWeaponShooting;
    public BulletCreatedEvent HitInfoCreated;
    public BulletEvent BulletCreated,BulletHitListPopulated;

    private void OnEnable()
    {
        Initialize();
        IsReloading = false;
        anim.SetFloat("ReloadTime", 1f/reloadTime);
        Subscribe(true);
        cam = Camera.main.transform;
        canReload = true;
        WS.GunEquippedEvent?.Invoke(this);
        
    }
    private void OnDisable()
    {
        Subscribe(false);
        WS.GunUnequippedEvent?.Invoke(this);

    }

    void Initialize()
    {
        if(Player == null || WS == null || InputCooker == null || PlayerAttackEffects == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            FPS = Player.GetComponent<FirstPersonController>();
            WS = gameObject.GetComponentInParent<WeaponSwitching>();
            InputCooker = Player.GetComponentInChildren<InputCooker>();
            PlayerAttackEffects = Player.GetComponent<PlayerAttackEffects>();
        }
    }
    public virtual void Start()
    {
        currentAmmo = maxAmmo;
        
    }
    protected virtual void Subscribe(bool subscribe)
    {
        if(subscribe)
        {
            InputCooker.PlayerPressedShoot += Shoot;
            InputCooker.PlayerPressedReload += StartReload;
            //InputCooker.PlayerStoppedReload += StartReload;

            WS.ReloadEvent += EndReload;
            // Debug.Log("Subbed " +gameObject.name);

            WeaponKilledSomething += NotifyPlayerOfKill;
           
        }
        else
        {
            // Debug.Log("Unsubbed "+gameObject.name);
            InputCooker.PlayerPressedShoot -= Shoot;
            InputCooker.PlayerPressedReload -= StartReload;

            WS.ReloadEvent -= EndReload;
            WeaponKilledSomething -= NotifyPlayerOfKill;

        }
    }
    public virtual void NotifyPlayerOfKill(GenericGun gun, IHittable victim )
    {
            FPS.PlayerKilledSomething?.Invoke(gun.FPS, victim); ;
    }

    public virtual void Shoot()
    {
        //if (isReloading) return;
       
        if (CanShoot)
        {
            BeforeShoot?.Invoke(this);
            DamageInstance newDamageInstance = new DamageInstance(this);
            newDamageInstance.PlayerAttackEffects = this.PlayerAttackEffects;

            OnWeaponShooting?.Invoke(this,newDamageInstance);
            newDamageInstance.Hits = ShootBullets();

            newDamageInstance.Deploy();
            AfterShoot?.Invoke(this);
            anim.SetTrigger("Shooting");
            DeductAmmo();

        }
        if(currentAmmo == 0)
        {
            StartReload();
        }
        //info.EnemyHit.GetComponent<HitEvent>().OnHit(HitInfo);

    }
    public virtual void DeductAmmo()
    {
        currentAmmo--;
        currentShootCD = shootCD;
        WS.UIM.UpdateAmmo(currentAmmo);

        //if (currentAmmo == 0)
        //{
        //    StartReload();
        //}
       


    }
    public virtual void Update()
    {
        if (!CanShoot)
        {
            currentShootCD -= Time.deltaTime;
            return;
        }

        if (IsAutomatic)
        {
            if (InputCooker.IsShooting)
            {
                if (currentShootCD <= 0f)
                {
                    Shoot();
                }
            }
            else
            {
                if (IsReloading != false)
                {
                    anim.SetTrigger("Shooting");
                }
            }
        }
    }
    public void EndReload()
    {
        currentAmmo = maxAmmo;
        IsReloading = false;
        
    }

    public virtual void StartReload()
    {
        if(CanStartReload)
        {
            isReloading = true;
        
            anim.SetBool("Reloading", true);

        }
        
    }
    public IEnumerator Reload()
    {
        isReloading = true;
        
        anim.SetBool("Reloading", true);
       
        yield return new WaitForSeconds(reloadTime - .25f);
        anim.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);
        currentAmmo = maxAmmo;
        WS.UIM.UpdateAmmo(currentAmmo);
        isReloading = false;
    }

    #region OldShootSystem
    //public virtual List<HitInfo> ShootRays()
    //{
    //    RaycastHit info;
    //    List<HitInfo> thingsHit = new List<HitInfo>(Multishot);
    //    for (int i = 0; i < Multishot; i++)
    //    {
    //        Ray ray = new Ray(InputCooker.MainCamera.transform.position, GetShootingDirection());
    //        //Ray ray2 = .ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));

    //        HitInfo hitInfo = new HitInfo(this);
    //        if (Physics.Raycast(ray, out info, 100f, Mask.value))
    //        {
    //            if (ToInstantiate != null)
    //                Destroy(Instantiate(ToInstantiate, info.point, Quaternion.LookRotation(-info.normal)), 2f);

    //            if (info.collider.GetComponent<IHittable>() != null)
    //            {
    //                hitInfo.FractureInfo.collisionPoint = info.point;
    //                hitInfo.GameObjectHit = info.collider.gameObject;
    //                //hitInfo.IsChainableAttack = false;

    //                thingsHit.Add(hitInfo);
    //            }
    //        }

    //    }

    //    return thingsHit;

    //} 
    #endregion

    public virtual List<HitInfo> ShootBullets()
    {
        List<HitInfo> thingsHit = new List<HitInfo>(Multishot); 
        //technically speaking the list is always bigger than "Multishot" whenever the gun has penetrations, but who cares

        List<HitscanBullet> hitscanBullets = new List<HitscanBullet>(Multishot);
        for (int i = 0; i < Multishot; i++)
        {
            HitscanBullet newHitscanBullet = new HitscanBullet(this);
            BulletCreated?.Invoke(newHitscanBullet);
            newHitscanBullet.Deploy();
            
            if (!newHitscanBullet.HasHitSomething)
                continue; 
            //RaycastHit[] raycastInformation = Physics.RaycastAll(InputCooker.MainCamera.transform.position, GetShootingDirection(), 100f, Mask.value);
            foreach (HitInfo hit in newHitscanBullet.Hits)
            {
                thingsHit.Add(hit);
            }

        }

        return thingsHit;
    }
    
    public Vector3 GetShootingDirection()
    {
        Vector3 targetPos = cam.position + cam.forward * range;
        targetPos = new Vector3(
            targetPos.x + UnityEngine.Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.y + UnityEngine.Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.z + UnityEngine.Random.Range(-inaccuracyDistance, inaccuracyDistance));

        Vector3 direction = targetPos - cam.position;
        return direction.normalized;
    }
    //public virtual void ShootRay()
    //{
    //    if (isReloading)
    //        return;

    //    if (currentAmmo <= 0)
    //    {
    //        StartReload();
    //        return;
    //    }
    //    if (!CanShoot) return;
    //    Vector2 screenCenterPoint = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    //    Camera cam = InputCooker.MainCamera;
    //    Ray ray = cam.ScreenPointToRay(screenCenterPoint);
    //    RaycastHit info;
    //    if (Physics.Raycast(ray, out info, 100f, Mask.value))
    //    {
    //        if (ToInstantiate != null)
    //            Destroy(Instantiate(ToInstantiate, info.point, Quaternion.identity), 2f);
    //        if (info.collider.GetComponent<IHittable>() != null)
    //        {
    //            HitInfo.collisionPoint = info.point;
    //            HitInfo.GameObjectHit = info.collider.gameObject;
    //            info.collider.GetComponent<HitEvent>().OnHit(HitInfo);
    //            foreach(ChainableAttack x in HitInfo.PlayerAttackEffects.Attacks)
    //            {
    //                x.Apply(HitInfo);
    //            }
    //            //Debug.Log("Shoot");
    //        }
    //    }
    //    currentAmmo--;
    //    currentShootCD = shootCD;
    //}
}
