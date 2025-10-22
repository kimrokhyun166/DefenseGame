using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<GameObject> OnDeath;
    [Header("스탯")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float attackCooldown = 1.2f;

    [Header("보상")]
    [SerializeField] public int moneyValue = 10;

    private float currentHealth;
    private float lastAttackTime;

    private Animator animator;
    private SpriteRenderer sr;
    private Transform nexusTarget;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // Nexus 태그로 타겟 찾기
        GameObject nexusObj = GameObject.FindGameObjectWithTag("Nexus");
        if (nexusObj != null)
            nexusTarget = nexusObj.transform;
    }

    void Update()
    {
        if (nexusTarget == null) return;

        // 공격 대상 탐색 및 공격
        bool foundTarget = DetectAndAttack();

        // 공격 대상이 없을 때 Nexus로 이동
        if (!foundTarget)
            MoveTowardsNexus();

        // z좌표 고정
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    // Nexus 방향으로 이동
    private void MoveTowardsNexus()
    {
        if (nexusTarget == null) return;

        Vector3 dir = (nexusTarget.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        if (sr != null)
            sr.flipX = dir.x < 0;


    }


    private bool DetectAndAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("MyUnit") || hit.CompareTag("Nexus"))
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;

                    if (sr != null)
                        sr.flipX = hit.transform.position.x < transform.position.x;

                    if (animator != null)
                    {
                        animator.SetTrigger("Hit");
                        animator.SetBool("Move", false);
                    }

                    if (hit.CompareTag("MyUnit"))
                    {
                        Unit unit = hit.GetComponent<Unit>();
                        if (unit != null)
                            unit.TakeDamage(attackDamage);
                    }
                    else if (hit.CompareTag("Nexus"))
                    {
                        Nexus nexus = hit.GetComponent<Nexus>();
                        if (nexus != null)
                            nexus.TakeDamage(attackDamage);
                    }
                }
                return true;
            }
        }
        return false;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        // 웨이브 매니저에게 사망 알림
        OnDeath?.Invoke(gameObject);

        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}