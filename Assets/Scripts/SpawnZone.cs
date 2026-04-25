using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnZone : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<GameObject> enemyPrefabs;     // 소환할 몬스터 리스트
    public int maxEnemies = 5;                // 최대 몬스터 수
    public float spawnInterval = 3f;          // 소환 간격
    public float spawnRadius = 2f;            // 소환 범위

    [Header("Zone Info")]
    public string zoneName = "Zone";          // Zone 이름

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private float nextSpawnTime = 0f;

    void Start()
    {
        Debug.Log(zoneName + " 시작!");
    }

    void Update()
    {
        // 죽은 몬스터 리스트에서 제거
        spawnedEnemies.RemoveAll(enemy => enemy == null);

        // 소환 조건: 시간 + 최대 수 미만
        if (Time.time >= nextSpawnTime && spawnedEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        // Prefab 리스트가 비어있으면 리턴
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.LogWarning(zoneName + ": 소환할 Prefab이 없습니다!");
            return;
        }

        // 랜덤 Prefab 선택
        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // Zone 위치 기준 랜덤 위치 계산
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        // 몬스터 생성
        GameObject enemy = Instantiate(randomPrefab, spawnPosition, Quaternion.identity);

        // 부모 설정 (선택사항 - Hierarchy 정리용)
        enemy.transform.parent = transform;

        // 리스트에 추가
        spawnedEnemies.Add(enemy);

        Debug.Log(zoneName + "에서 " + enemy.name + " 소환! (현재: " + spawnedEnemies.Count + "/" + maxEnemies + ")");
    }

    // Zone 범위 시각화
    void OnDrawGizmos()
    {
        // Zone 중심 (초록색)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        // 소환 범위 (하늘색)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        // Zone 이름 표시
#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1f, zoneName);
#endif
    }

    void OnDrawGizmosSelected()
    {
        // 선택 시 더 진하게
        Gizmos.color = new Color(0, 1, 1, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
}