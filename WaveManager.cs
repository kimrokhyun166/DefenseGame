using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [Header("웨이브 설정")]
    [SerializeField] private GameObject enemyPrefab;     // 생성할 적 프리팹
    [SerializeField] private Transform[] spawnPoints;    // 스폰 위치들
    [SerializeField] private int enemiesPerWave = 5;     // 웨이브당 적 수
    [SerializeField] private float spawnDelay = 1.5f;    // 적 생성 간격
    [SerializeField] private int maxWaves = 10;          // 최대 웨이브 수

    [Header("UI 설정")]
    [SerializeField] private Text waveText;              // 웨이브 표시용 UI
    [SerializeField] private float waveTextDuration = 2f;// UI 표시 시간 (초)
    [SerializeField] private Text moneyText;

    public int currentMoney = 0;

    private int currentWave = 0;                         // 현재 웨이브
    private List<GameObject> activeEnemies = new List<GameObject>(); // 현재 남은 적

    private bool waveInProgress = false;                 // 현재 웨이브 진행 중인지 체크

    private void Start()
    {
        if (waveText != null)
            waveText.gameObject.SetActive(false);

        if (moneyText != null)
        {
            UpdateMoneyUI();
        }

        StartNextWave();
    }

    // 다음 웨이브 시작
    private void StartNextWave()
    {
        if (currentWave >= maxWaves)
        {
            ShowWaveUI("🎉 모든 웨이브 완료!");
            Debug.Log("🎉 모든 웨이브 완료!");
            return;
        }

        currentWave++;
        StartCoroutine(WaveRoutine());
    }

    // ✅ 웨이브 실행 루틴
    private IEnumerator WaveRoutine()
    {
        waveInProgress = true;
        ShowWaveUI($"Wave {currentWave} 시작!");
        Debug.Log($"🌊 웨이브 {currentWave} 시작!");

        yield return StartCoroutine(SpawnWave(currentWave));

        //  모든 적이 죽을 때까지 대기
        yield return new WaitUntil(() => activeEnemies.Count == 0);

        Debug.Log($" 웨이브 {currentWave} 완료!");

        waveInProgress = false;

        // 다음 웨이브 자동 시작
        yield return new WaitForSeconds(2f);
        StartNextWave();
    }

    //  적 스폰
    private IEnumerator SpawnWave(int waveNumber)
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[WaveManager] 적 프리팹 또는 스폰 포인트가 설정되지 않음!");
            yield break;
        }

        int enemiesToSpawn = enemiesPerWave + (waveNumber - 1) * 2;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            activeEnemies.Add(enemy);

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.OnDeath += (deadEnemy) =>
                {
                    activeEnemies.Remove(deadEnemy);

                    Enemy diedEnemyScript = deadEnemy.GetComponent<Enemy>();
                    if (diedEnemyScript != null)
                    {
                        currentMoney += diedEnemyScript.moneyValue;
                        UpdateMoneyUI();
                        Debug.Log($"적을 처치하여 {diedEnemyScript.moneyValue} 골드를 얻었습니다! 현재 골드: {currentMoney}");
                    }
                };
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    //  웨이브 UI 표시
    private void ShowWaveUI(string message)
    {
        if (waveText == null) return;

        StopCoroutine(nameof(HideWaveUIText));
        waveText.text = message;
        waveText.gameObject.SetActive(true);
        StartCoroutine(HideWaveUIText());
    }

    private IEnumerator HideWaveUIText()
    {
        yield return new WaitForSeconds(waveTextDuration);
        waveText.gameObject.SetActive(false);
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"Gold: {currentMoney}";
        }
    }
}