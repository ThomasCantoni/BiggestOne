using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<FirstPersonController>();
        Destroy(this.gameObject);
    }
    
}
