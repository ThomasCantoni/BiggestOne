using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInstance
{
    public GenericGun SourceGun;
    private List<HitInfo> hits;
    public List<HitInfo> Hits
    {
        get { return hits; }
        set
        {
            hits = value;
            EnemiesHit = FilterDistinct(hits);
        }
    }
    private List<GameObject> enemiesHit;
    public List<GameObject> EnemiesHit
    {
        get { return enemiesHit; }
        private set
        {
            enemiesHit = value;
        }
    }

    public DamageInstance(GenericGun sourceGun)
    {
        SourceGun = sourceGun;
    }
    public List<GameObject> FilterDistinct(List<HitInfo> toFilter)
    {
        
        List<GameObject> toReturn = new List<GameObject>(toFilter.Count);
        foreach(HitInfo collided in toFilter)
        {
            if(!toReturn.Contains(collided.GameObjectHit))
            {
                toReturn.Add(collided.GameObjectHit);
            }
        }
        toReturn.TrimExcess();
        return toReturn;
    }
    public void Deploy()
    {
        for (int i = 0; i < Hits.Count; i++)
        {
            Hits[i].SourceDamageInstance = this;
            //ThingsHit[i].Damage = SourceGun.Damage;
            Hits[i]?.GameObjectHit.GetComponent<IHittable>().OnHit(Hits[i]);
            

        }
        for (int i = 0; i < EnemiesHit.Count; i++)
        {
            foreach (ChainableAttack x in SourceGun.PlayerAttackEffects.Attacks)
            {
                x.Apply(EnemiesHit[i]);
            }

        }
        //for (int i = 0; i < EnemiesHit.Count; i++)
        //{

        //}
    }
}

public class GenericGun : MonoBehaviour
{
    //[SerializeField]
    //public HitInfo HitInfo;
    public LayerMask Mask;
    public bool IsAutomatic;
    public float FireRate;
    public int Multishot=1;
    public float Damage;
    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 1f;
    public Transform cam;
    public Animator anim;
    public int Magazine;
    public WeaponSwitching WS;
    public GameObject ToInstantiate;
    //public UnityEvent ShootEvent;
    public GameObject Player;
    public PlayerAttackEffects PlayerAttackEffects;
    public InputCooker InputCooker;
    protected bool hasShotOnce, shooting;
    protected float currentShootCD = 0;
    public bool shotgun = false;
    public delegate void WeaponHitSomething(HitInfo info);
    public event WeaponHitSomething WeaponHitSomethingEvent;
    public float range = 50f;
    public float inaccuracyDistance = 5f;
    public bool isReloading = false;
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
        get { return currentShootCD <= 0f && !isReloading; }
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
        cam = Camera.main.transform;
    }
    private void OnDisable()
    {
        Subscribe(false);
    }
   
    void Initialize()
    {
        if(Player == null || WS == null || InputCooker == null || PlayerAttackEffects == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            WS = gameObject.GetComponentInParent<WeaponSwitching>();
            InputCooker = Player.GetComponentInChildren<InputCooker>();
            PlayerAttackEffects = Player.GetComponent<PlayerAttackEffects>();
        }
    }
    public virtual void Start()
    {
        currentAmmo = maxAmmo;
        //if (HitInfo.sender == null)
        //{
        //    HitInfo.sender = this.gameObject;
        //}
        //OnEnable();
    }
    protected virtual void Subscribe(bool subscribe)
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
        //if (isReloading) return;
        if (!CanShoot) return ;
        DamageInstance newDamageInstance = new DamageInstance(this);
        
        newDamageInstance.Hits = ShootRays();
        
        newDamageInstance.Deploy();
        currentAmmo--;
        currentShootCD = shootCD;
        if (currentAmmo <= 0)
        {
            StartReload();
            
        }
        else
        {
            anim.SetTrigger("Shooting");
        }

        //info.EnemyHit.GetComponent<HitEvent>().OnHit(HitInfo);

    }
    public virtual void Update()
    {
        shotgun = false;
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
    public virtual List<HitInfo> ShootRays()
    {
        RaycastHit info;
        List<HitInfo> thingsHit = new List<HitInfo>(Multishot);
        for (int i = 0; i < Multishot; i++)
        {
            Ray ray = new Ray(InputCooker.MainCamera.transform.position, GetShootingDirection());
            //Ray ray2 = .ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
                
            HitInfo hitInfo = new HitInfo(this);
            if (Physics.Raycast(ray, out info, 100f, Mask.value))
            {
                if (ToInstantiate != null)
                    Destroy(Instantiate(ToInstantiate, info.point, Quaternion.LookRotation(-info.normal)), 2f);

                if (info.collider.GetComponent<IHittable>() != null)
                {
                    hitInfo.collisionPoint = info.point;
                    hitInfo.GameObjectHit = info.collider.gameObject;
                    hitInfo.IsChainableAttack = false;

                    thingsHit.Add(hitInfo);
                }
            }

        }
        
        return thingsHit;
       
    }
    Vector3 GetShootingDirection()
    {
        Vector3 targetPos = cam.position + cam.forward * range;
        targetPos = new Vector3(
            targetPos.x + Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.y + Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.z + Random.Range(-inaccuracyDistance, inaccuracyDistance));

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
