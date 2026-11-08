using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveGroup
{
    public GameObject enemyPrefab;
    public int count = 5;
    public float spawnInterval = 0.5f;
}

[System.Serializable]
public class Wave
{
    public string waveName = "Wave 1";
    public List<WaveGroup> waveGroups;
    public float delayBeforeNextWave = 5f;
}

public class WaveManager : MonoBehaviour
{
    [Header("Spawners")]
    public Transform[] spawnPoints;

    [Header("Wave List")]
    public List<Wave> waves;

    private int currentWaveIndex = 0;
    private int activeCoroutines = 0;

    private void Start()
    {
        if (waves.Count > 0)
        {
            StartCoroutine(StartNextWave());
        }
    }

    private IEnumerator StartNextWave()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) yield break;
        Wave currentWave = waves[currentWaveIndex];
        Debug.Log($"<color=yellow>=== {currentWave.waveName} BAŞLADI! ===</color>");

        activeCoroutines = currentWave.waveGroups.Count;

        foreach (WaveGroup group in currentWave.waveGroups)
        {
            StartCoroutine(SpawnGroup(group));
            yield return new WaitForSeconds(0.1f);
        }

        while (activeCoroutines > 0)
        {
            yield return null;
        }

        currentWaveIndex++;
        if (currentWaveIndex < waves.Count)
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver) yield break;
            Debug.Log($"<color=cyan>{currentWave.delayBeforeNextWave} saniye sonra sonraki dalga gelecek...</color>");
            yield return new WaitForSeconds(currentWave.delayBeforeNextWave);
            StartCoroutine(StartNextWave());
        }
        else
        {
            Debug.Log("<color=cyan>Tüm dalgalar doğruldu, sahnemizdeki düşmanların temizlenmesi bekleniyor...</color>");

            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                if (GameManager.Instance?.IsGameOver == true) yield break;
                yield return new WaitForSeconds(0.5f);
            }

            if (GameManager.Instance?.IsGameOver == false)
            {
                GameManager.Instance.WinGame();
            }
            Debug.Log("<color=green>TÜM DALGALAR BİTTİ!</color>");
        }
    }

    private IEnumerator SpawnGroup(WaveGroup group)
    {
        for (int i = 0; i < group.count; i++)
        {
            SpawnEnemy(group.enemyPrefab);
            yield return new WaitForSeconds(group.spawnInterval);
        }

        activeCoroutines--;
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;


        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Vector2 randomOffset = Random.insideUnitCircle * 0.4f;
        Vector3 spawnPosition = randomSpawnPoint.position + (Vector3)randomOffset;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}