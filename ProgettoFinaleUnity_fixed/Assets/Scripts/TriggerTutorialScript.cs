using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorialScript : MonoBehaviour
{
    public GameObject Trigger;
    public GameObject Object;
    public EnemyMeleeAI EM;
    public Animator anim;
    public bool TutorialAnim;
    public bool TutorialShop;

    private void OnTriggerEnter(Collider other)
    {
        if (Object != null)
        {
            Trigger.SetActive(true);
        }
    }
    public void NextTrigger()
    {
        Trigger.SetActive(true);
    }
    private void Update()
    {
        if (anim == null)
        {
            return;
        }
        if (TutorialAnim)
        {
            anim.SetBool("Tutorial", true);
        }
        if(EM.IsDead)
        {
            anim.SetBool("Tutorial", false);
            TutorialAnim = false;
        }
    }
}
