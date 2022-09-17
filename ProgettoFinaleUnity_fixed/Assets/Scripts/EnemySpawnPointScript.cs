using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnData
{
    public GameObject EnemyToSpawn;
    public int NumberToSpawn;
}
public class EnemySpawnPointScript : MonoBehaviour
{
    
    public List<EnemySpawnData> SpawnList = new List<EnemySpawnData>();
    
    private bool Recycle = false;
    private Queue<GameObject> toInstantiate;
    public Repeater SpawnRepeater;
    private List<Transform> spawnPoints;
    private Queue<GameObject> spawnQueue;
    // Start is called before the first frame update
    void Start()
    {
        toInstantiate = new Queue<GameObject>();
       
        SpawnRepeater.RepeaterTickEvent += SpawnEnemy;
        spawnPoints = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints.Add(transform.GetChild(i));
        }
        
            spawnQueue = new Queue<GameObject>();
            for (int i = 0; i < SpawnList.Count; i++)
            {
                EnemySpawnData current = SpawnList[i];
                for (int j = 0; j < current.NumberToSpawn; j++)
                {
                    spawnQueue.Enqueue(current.EnemyToSpawn);
                    //EnemyClass toRespawn = current.EnemyToSpawn.GetComponent<EnemyClass>();
                    //toRespawn.OnEnemyDeath += EnqueueEnemy()
                }
            }
       

        SpawnRepeater.StartRepeater();
    }
    public void EnqueueEnemy()
    {
        
    }
    public void SpawnEnemy()
    {

        Transform spawnPointSelected = spawnPoints[ Random.Range(0, spawnPoints.Count)];
        if(spawnQueue.Count >0)
        {
            GameObject enemySelected = spawnQueue.Dequeue();

            GameObject newEnemy = Instantiate(enemySelected, spawnPointSelected.position, spawnPointSelected.rotation);

        }
        else
        {
            Debug.Log("Out of enemies to spawn! Stopping repeater!");
            Destroy(this.gameObject);
            SpawnRepeater.StopRepeater();
        }
    }
    
    
   
}
