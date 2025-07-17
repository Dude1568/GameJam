using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public GameObject enemyPrefab;
    public int enemyCount;
    public float spawnInterval;
}

public class WaveSpawner : MonoBehaviour
{
    public List<Wave> waves;
    public Transform[] spawnPoints;

    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private bool waveReady = true;

    void Update()
    {
        if (waveReady && !isSpawning && currentWaveIndex < waves.Count)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            }
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning Wave: " + wave.waveName);
        isSpawning = true;
        waveReady = false;
            
        for (int i = 0; i < wave.enemyCount; i++)
        {
            
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            UnityEngine.Vector3 door =spawnPoint.position *UnityEngine.Vector2.right * 4;
            Instantiate(wave.enemyPrefab,spawnPoint.position , UnityEngine.Quaternion.identity);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        isSpawning = false;
        currentWaveIndex++;
        waveReady = true;

        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("All waves completed!");
        }
        else
        {
            Debug.Log("Wave complete! Press Enter to start the next one.");
        }
    }
}
