using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnDistance = 10f;
    public int maxEnemies = 20;

    private float spawnTimer;
    private Transform player;
    private int currentEnemyCount;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        // 플레이어 주변 랜덤 위치
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnDistance;

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        Debug.Log("적 생성! 위치: " + spawnPos);
    }
}