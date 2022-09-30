using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnPointScript : MonoBehaviour
{
    //[Tooltip("Wether or not  the next enemy")]
    //public bool Randomize;
    public int MaxEnemiesActive;
    [Tooltip("The maximum frequency reachable by the SpawnRepeater")]
    public float MaxFrequency;
    [Tooltip("The amount of frequency added to the SpawnRepeater everytime the EscalationRepeater fires.")]
    public float FrequencyIncrease;
    [Tooltip("When this repeater fires, the spawner will start to increase the frequency of spawns depending on the value of FrequencyIncrease, " +
        "up to MaxFrequency")]
    public Repeater EscalationRepeater = new Repeater();
    [Tooltip("This repeater is responsible for spawning the enemies. Should only be modified to change the initial value of the frequency")]
    public Repeater SpawnRepeater = new Repeater();
    public UnityEvent<GameObject> OnSpawned;
    public List<GameObject> SpawnList = new List<GameObject>();
    private List<Transform> spawnPoints;
    /*
    private bool Recycle = false;
    private Queue<GameObject> toInstantiate;
    private Queue<GameObject> spawnQueue;
  
    */
    void Start()
    {

        foreach(GameObject x in SpawnList)
        {
            EnemyClass toCheck = x.GetComponentInChildren<EnemyClass>();
            if(toCheck == null)
            {
                Debug.LogError("This spawn list element: " + x.name + "  doesn't have EnemyClass component!");
                return;
            }

        }
        //EscalationRepeater = new Repeater();
        EscalationRepeater.RepeaterTickEvent += increaseFreq;

        //SpawnRepeater = new Repeater();
        SpawnRepeater.RepeaterTickEvent += SpawnEnemy;
        spawnPoints = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints.Add(transform.GetChild(i));
        }

        //    spawnQueue = new Queue<GameObject>();
        //    for (int i = 0; i < SpawnList.Count; i++)
        //    {
        //        EnemySpawnData current = SpawnList[i];
        //        for (int j = 0; j < current.NumberToSpawn; j++)
        //        {
        //            spawnQueue.Enqueue(current.EnemyToSpawn);
        //            //EnemyClass toRespawn = current.EnemyToSpawn.GetComponent<EnemyClass>();
        //            //toRespawn.OnEnemyDeath += EnqueueEnemy()
        //        }
        //    }

        EscalationRepeater.StartRepeater();
        SpawnRepeater.StartRepeater();
    }
    private void increaseFreq()
    {
        float newFrequencyValue = SpawnRepeater.Frequency +=FrequencyIncrease;
        newFrequencyValue = Mathf.Clamp(newFrequencyValue, 0, MaxFrequency);
        SpawnRepeater.Frequency = newFrequencyValue;
        
    }
    public void SpawnEnemy()
    {

        Transform spawnPointSelected = spawnPoints[Random.Range(0, spawnPoints.Count)];
        int chosenIndex = Random.Range(0, SpawnList.Count);
        if(chosenIndex < SpawnList.Count)
        {
            GameObject selected = SpawnList[chosenIndex];
        
            GameObject newEnemy = Instantiate(selected, spawnPointSelected.position, spawnPointSelected.rotation);
            OnSpawned?.Invoke(newEnemy);

        }
        Debug.Log("SPAWNED WITH " + SpawnRepeater.Frequency + " HZ OF FREQUENCY");
        //newEnemy.GetComponent<>
        
        //else
        //{
        //    Debug.Log("Out of enemies to spawn! Stopping repeater!");
        //    Destroy(this.gameObject);
        //    SpawnRepeater.StopRepeater();
        //}
    }



}
