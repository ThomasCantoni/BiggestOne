using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public ChainableAttack toApply;
    public bool GetRandom = false;
    public static List<ChainableAttack> attackList;
    public float duration = 5f;
    
    public ChainableAttack GetRandomChainableAttack()
    {
        ChainableAttack[] chainableAttacks = Resources.LoadAll<ChainableAttack>("");
        int RandomIndex = Random.Range(0, chainableAttacks.Length - 1);
        return chainableAttacks[RandomIndex];
    }
    private void OnTriggerEnter(Collider other)
    {

        if (GetRandom)
        {
            toApply = GetRandomChainableAttack();
        }

        PlayerAttackEffects PAE = other.GetComponent<PlayerAttackEffects>();
        PAE.Add(toApply);
        StartCoroutine(removePowerUps(PAE));
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
    }
    public IEnumerator removePowerUps(PlayerAttackEffects PAE)
    {
        yield return new WaitForSeconds(duration);
        PAE.Remove(toApply);
        Destroy(this.gameObject);
    }
}
