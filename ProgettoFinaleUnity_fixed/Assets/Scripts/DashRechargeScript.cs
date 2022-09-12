using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRechargeScript : MonoBehaviour
{
    private Dash PlayerDashScript;
    public int ChargesToReplenish = 1;
    private void OnTriggerEnter(Collider other)
    {
        PlayerDashScript = other.gameObject.GetComponent<Dash>();
        if(PlayerDashScript != null)
        {
            PlayerDashScript.RechargeDash(ChargesToReplenish);
            Debug.Log("Dash Recharged!");
        }
    }

}
