using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericAnimationEvent : MonoBehaviour
{
    public UnityEvent animtionEvent;
    public void InvokeAnimationEvent()
    {
        animtionEvent?.Invoke();
    }
}
