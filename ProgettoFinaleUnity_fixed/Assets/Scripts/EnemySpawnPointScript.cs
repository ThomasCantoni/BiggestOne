using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EnemySpawnPointScript : MonoBehaviour
{
    //[Tooltip("Wether or not  the next enemy")]
    //public bool Randomize;
    public int MaxEnemiesActive;
    [Tooltip("The maximum frequency reachable by the SpawnRepeater")]
    public float MinInterval;
    [Tooltip("The amount of frequency added to the SpawnRepeater everytime an Enemy is spawned.")]
    public float IntervalDecreaseMs;
    public int SpawnInterval;
    [Tooltip("This repeater is responsible for spawning the enemies. Should only be modified to change the initial value of the frequency")]
    public SimpleTimer SpawnTimer;
    public UnityEvent<GameObject> OnSpawned;
    public List<GameObject> SpawnList = new List<GameObject>();
    private List<Transform> spawnPoints;
    

    private Dictionary<Type,List<GameObject>> enemiesActive;
    private Dictionary<Type, Queue<EnemyClass>> Graveyard;
    private int enemiesAlive;
    /*
    private bool Recycle = false;
    private Queue<GameObject> toInstantiate;
    private Queue<GameObject> spawnQueue;
  
    */
    void Start()
    {
        enemiesActive = new Dictionary<Type, List<GameObject>>();
        Graveyard = new Dictionary<Type, Queue<EnemyClass>>();
        for(int i=0;i<SpawnList.Count;i++)
        {
            GameObject x = SpawnList[i];
            EnemyClass toCheck = x.GetComponentInChildren<EnemyClass>();
            if(toCheck == null)
            {
                Debug.LogError("This spawn list element: " + x.name + "  doesn't have EnemyClass component!");
                return;
            }
           
            enemiesActive[toCheck.GetType()] = new List<GameObject>();
            Graveyard[toCheck.GetType()] = new Queue<EnemyClass>();
        }


        SpawnTimer = new SimpleTimer(SpawnInterval);
        SpawnTimer.TimerCompleteEvent += SpawnEnemy;

        spawnPoints = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints.Add(transform.GetChild(i));
        }





        SpawnTimer.StartTimer();
    }
    private void increaseFreq()
    {
        float newIntervalValue = SpawnTimer.TimeMs - IntervalDecreaseMs;
        newIntervalValue = Mathf.Clamp(newIntervalValue, MinInterval,1000000);
        SpawnTimer.ChangeTime((int)newIntervalValue);
        SpawnInterval = (int)newIntervalValue;
        
    }
    public void SpawnEnemy()
    {
        Debug.Log(SpawnTimer.TimeMs);
        

        Transform spawnPointSelected = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
        int chosenIndex = UnityEngine.Random.Range(0, SpawnList.Count);
        if(enemiesAlive >= MaxEnemiesActive)
        {
            SpawnTimer.StopTimer();
            
            return;
        }
        if(chosenIndex < SpawnList.Count)
        {
            GameObject selected = SpawnList[chosenIndex];
            EnemyClass enemyClassSelected = selected.GetComponentInChildren<EnemyClass>();
            Queue<EnemyClass> correctQueue = Graveyard[enemyClassSelected.GetType()];
            if (correctQueue.Contains(enemyClassSelected))
            {
                selected = correctQueue.Dequeue().gameObject;
                EnemyClass ec = selected.GetComponentInChildren<EnemyClass>();
                ec.HP_Value = ec.MaxHP;
                ec.gameObject.SetActive(true);
                enemiesAlive++;
            }
            else if( enemiesActive.Count<MaxEnemiesActive)
            {
                GameObject newEnemy = Instantiate(selected, spawnPointSelected.position, spawnPointSelected.rotation);
                OnSpawned?.Invoke(newEnemy);
                
                EnemyClass EC = newEnemy.GetComponentInChildren<EnemyClass>();
                List<GameObject> correctList = enemiesActive[EC.GetType()];
                correctList.Add(newEnemy);
                EC.IsInSpawnQueue = true;
                EC.OnEnemyDeathParam += EnemyDiedCallback;
                enemiesAlive++;
            }
            increaseFreq();
        }
        Debug.Log("SPAWNED WITH " + SpawnTimer.TimeMs + " MS OF INTERVAL");
        //newEnemy.GetComponent<>
        
        //else
        //{
        //    Debug.Log("Out of enemies to spawn! Stopping repeater!");
        //    Destroy(this.gameObject);
        //    SpawnRepeater.StopRepeater();
        //}
    }

    public void EnemyDiedCallback(IKillable justDied)
    {
        enemiesAlive--;
        Type key = ((EnemyClass)justDied.Mono).GetType();
        Queue<EnemyClass> correctQueue = Graveyard[key];
        correctQueue.Enqueue((EnemyClass)justDied.Mono);
        SpawnTimer.StartTimer();
        
    }

}
