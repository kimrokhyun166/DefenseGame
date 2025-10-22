using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;  // 소환할 적 프리팹
    [SerializeField] private Transform spawnPoint;    // 스폰 위치
    [SerializeField] private Transform target;        // 목표 (예: Nexus)
    [SerializeField] private float moveSpeed = 2f;

    public void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("[EnemySpawner] 프리팹 또는 스폰 위치가 설정되지 않았습니다!");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // 적에게 목표 설정
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null && target != null)
        {
            // Enemy 내부에 target 변수가 있다면 여기서 연결
            // enemyScript.SetTarget(target);  ← 필요 시 구현
        }

        Debug.Log($"[EnemySpawner] {enemy.name} 생성 완료 ({spawnPoint.name})");
    }
}
