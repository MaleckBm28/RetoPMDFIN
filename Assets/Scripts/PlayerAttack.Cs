
using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 1;

    [Header("Tiempos de Ataque")]
    public float attackCooldown = 1.0f;
    public float attackHitDelay = 0.5f;

    [Header("Detección")]
    public LayerMask enemyLayer;

    [Header("Audio (SFX)")]
    public AudioSource sfxAudioSource;
    public AudioClip attackSound;

    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("PlayerAttack no pudo encontrar el componente Animator.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;

        if (sfxAudioSource != null && attackSound != null)
        {
            // Detenemos sonidos que estuviera antes
            sfxAudioSource.Stop();
            // Asignamos el sonido de ataque
            sfxAudioSource.clip = attackSound;
            // hace el sonido
            sfxAudioSource.Play();
        }

        // Activar la animación de ataque
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Esperar el tiempo de delay
        yield return new WaitForSeconds(attackHitDelay);

        // detecta enemigos en su layer
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        if (hitEnemies.Length > 0)
        {
            Debug.Log($"¡Golpe ejecutado (con delay)! {hitEnemies.Length} enemigos en rango.");
        }

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyHealth enemy = enemyCollider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }

        // Esperar el cd
        float remainingTime = attackCooldown - attackHitDelay;
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}