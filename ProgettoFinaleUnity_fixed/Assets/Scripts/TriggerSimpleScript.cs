using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerSimpleScript : MonoBehaviour
{
    public UnityEvent Events;

    public virtual void OnTriggerEnter(Collider other)
    {
        Events?.Invoke();
    }
}
