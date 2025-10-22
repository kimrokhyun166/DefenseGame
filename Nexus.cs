using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Nexus : MonoBehaviour
{
    [Header("넥서스 스탯")]
    [SerializeField] private float maxHealth = 500f;
    private float currentHealth;

    private SpriteRenderer sr;

    private void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // 💥 피격 시 빨간색 깜빡임 효과
        if (sr != null)
            StartCoroutine(HitEffect());

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator HitEffect()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sr.color = Color.white;
    }

    private void Die()
    {
        Debug.Log("🏚️ Nexus 파괴! → GameOver 씬으로 이동");
        SceneManager.LoadScene("GameOver"); // ✅ 게임오버 씬 로드
    }
}
