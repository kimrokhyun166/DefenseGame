using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;  // ��ȯ�� �� ������
    [SerializeField] private Transform spawnPoint;    // ���� ��ġ
    [SerializeField] private Transform target;        // ��ǥ (��: Nexus)
    [SerializeField] private float moveSpeed = 2f;

    public void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("[EnemySpawner] ������ �Ǵ� ���� ��ġ�� �������� �ʾҽ��ϴ�!");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // ������ ��ǥ ����
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null && target != null)
        {
            // Enemy ���ο� target ������ �ִٸ� ���⼭ ����
            // enemyScript.SetTarget(target);  �� �ʿ� �� ����
        }

        Debug.Log($"[EnemySpawner] {enemy.name} ���� �Ϸ� ({spawnPoint.name})");
    }
}
