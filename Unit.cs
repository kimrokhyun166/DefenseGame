using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    [Header("스탯")]
    public float maxHealth = 100f;
    public float moveSpeed = 5f;
    public float attackRange = 2f;
    public float attackDamage = 25f;
    public float attackCooldown = 1.2f;
    public float dashDistance = 0.5f;
    public float dashTime = 0.15f;
    public float returnTime = 0.2f;

    private float currentHealth;
    private float lastAttackTime;

    private bool isAttacking = false;
    private bool isMoving = false;
    private bool isSelected = false;
    private bool isDead = false; // 사망 여부

    private Vector3 targetPosition;

    private Animator animator;
    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
        targetPosition = transform.position;
    }

    void Update()
    {
        // 사망 상태면 아무 행동도 안 함
        if (isDead) return;

        if (!isAttacking)
            HandleMovement();

        if (!isAttacking)
            DetectAndAttackEnemy();
    }

    private void HandleMovement()
    {
        if (!isMoving) return;

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        if (sr != null)
            sr.flipX = targetPosition.x < transform.position.x;

        if (animator != null)
            animator.SetBool("Move", true);

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            isMoving = false;
            if (animator != null)
                animator.SetBool("Move", false);
        }
    }

    public void MoveTo(Vector3 position)
    {
        // 공격 중이거나 죽은 상태면 이동 불가
        if (isAttacking || isDead) return;

        position.z = 0f;
        targetPosition = position;
        isMoving = true;

        if (animator != null)
            animator.SetBool("Move", true);
    }

    public void Select()
    {
        if (isDead) return;
        isSelected = true;
        Debug.Log($"{name} 선택됨");
    }

    public void Deselect()
    {
        isSelected = false;
        Debug.Log($"{name} 선택 해제");
    }

    private void DetectAndAttackEnemy()
    {
        if (isDead) return;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
        Transform nearest = null;
        float nearestDist = Mathf.Infinity;

        foreach (Collider2D col in enemies)
        {
            if (col.CompareTag("Enemy"))
            {
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < nearestDist)
                {
                    nearest = col.transform;
                    nearestDist = dist;
                }
            }
        }

        if (nearest != null && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            StartCoroutine(DashAttack(nearest));
        }
    }

    private IEnumerator DashAttack(Transform target)
    {
        if (isDead) yield break;

        isAttacking = true;
        isMoving = false;

        if (animator != null)
            animator.SetBool("Move", false);

        Vector3 startPos = transform.position;
        Vector3 dir = (target.position - transform.position).normalized;
        dir.z = 0;
        Vector3 dashPos = startPos + dir * dashDistance;

        if (sr != null)
            sr.flipX = target.position.x < transform.position.x;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / dashTime;
            transform.position = Vector3.Lerp(startPos, dashPos, t);
            yield return null;
        }

        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(attackDamage);
            Animator enemyAnim = enemy.GetComponent<Animator>();
            if (enemyAnim != null)
                enemyAnim.SetTrigger("Hit");
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / returnTime;
            transform.position = Vector3.Lerp(dashPos, startPos, t);
            yield return null;
        }

        transform.position = startPos;
        isAttacking = false;
    }

    public void TakeDamage(float amount)
    {
        // 사망 시 추가 피해 무시
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{name} 피해: {amount}, 남은 체력: {currentHealth}");

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        isAttacking = false;
        isMoving = false;

        if (animator != null)
        {
            animator.SetBool("Move", false);
            animator.SetBool("Die", true);
        }

        Debug.Log($"{name} 사망 처리됨");

        StartCoroutine(DeathDelay(5f));
    }

    private IEnumerator FlashRed()
    {
        if (sr == null) yield break;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sr.color = originalColor;
    }

    private IEnumerator DeathDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
