using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRespawn : MonoBehaviour
{
    public Transform player;
    private void OnTriggerEnter(Collider other)
    {
        player.position = new Vector3(0, 1, 0);
    }
}
