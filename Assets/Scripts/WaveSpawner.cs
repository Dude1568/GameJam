using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class WaveEnemy
{
    public GameObject enemyPrefab;
    public float spawnRatio; // e.g., 0.5 = 50%
}

public class WaveSpawner : MonoBehaviour
{
    public static event Action<GameObject> RAIDACTIVE;
    public enum WaveSpawnerState { SPAWNING, ACTIVERAID, BUILDING }
    public static WaveSpawnerState gameState = WaveSpawnerState.BUILDING;

    public List<WaveEnemy> enemyTypes;
    public Transform[] spawnPoints;

    private int currentWave = 0;
    private bool isSpawning = false;
    private bool waveReady = true;

    public float baseEnemyCount = 5;
    public float enemyGrowthRate = 1.5f;
    public float spawnInterval = 0.5f;
    [SerializeField]private GameObject buildMenu;
    [SerializeField] Transform buttons;
    private List<GameObject> aliveEnemies = new List<GameObject>();
    void Awake()
    {
       buildMenu = GameObject.FindGameObjectWithTag("UI");
    } 
    void Update()
    {
        // Clean up dead enemies
        aliveEnemies.RemoveAll(enemy => enemy == null);



        // Transition to BUILDING once all enemies are dead
        if (gameState == WaveSpawnerState.ACTIVERAID && aliveEnemies.Count == 0)
        {
            buildMenu.SetActive(true);

            gameState = WaveSpawnerState.BUILDING;
            waveReady = true;
            GridManager.Instance.InBetweenWaveProcess();
            buttons.gameObject.SetActive(true);
            Debug.Log("All enemies defeated! Press Enter to start next wave.");
        }
    }

    public void StartWave()
    {
        StartCoroutine(SpawnWave(currentWave));
    }

    IEnumerator SpawnWave(int waveNumber)
    {
        if (GridManager.treasuryPlaced==false)
            yield break;
        buttons.gameObject.SetActive(false);
        buildMenu.SetActive(false);
        isSpawning = true;
        waveReady = false;
        gameState = WaveSpawnerState.SPAWNING;
        GameObject.FindGameObjectWithTag("Player").GetComponent<EnemyHealth>().health=30;
        GameObject.FindGameObjectWithTag("Player").GetComponent<EnemyStateController>().SetState(EnemyState.IDLE);
        int totalEnemies = Mathf.RoundToInt(baseEnemyCount + waveNumber * enemyGrowthRate);
        Debug.Log($"Spawning Wave {waveNumber + 1} - {totalEnemies} enemies");

        for (int i = 0; i < totalEnemies; i++)
        {
            GameObject enemyToSpawn = ChooseEnemyType();
            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
            aliveEnemies.Add(enemy);
            RAIDACTIVE?.Invoke(enemy);
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
        currentWave++;
        gameState = WaveSpawnerState.ACTIVERAID;
        
        Debug.Log("Wave spawned! Fight off the enemies.");
    }

    GameObject ChooseEnemyType()
    {
        float totalWeight = 0f;
        foreach (var e in enemyTypes) totalWeight += e.spawnRatio;

        float randomPoint = UnityEngine.Random.value * totalWeight;
        float current = 0f;

        foreach (var enemy in enemyTypes)
        {
            current += enemy.spawnRatio;
            if (randomPoint <= current)
                return enemy.enemyPrefab;
        }

        return enemyTypes[0].enemyPrefab;
    }
}
