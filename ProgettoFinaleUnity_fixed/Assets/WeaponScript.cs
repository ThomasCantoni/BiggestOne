using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public ParticleSystem ToActivate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnShoot2()
    {
        Debug.Log("BOOM2");
    }
}
