using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Referencias de UI (Barra de Vida)")]
    public Image healthBarFill;

    [Header("Configuración de Animación (Opcional)")]
    public bool useTakeHitAnimation = true;
    public bool useDeathAnimation = true;
    public float destroyDelay = 2f;

    [Header("Recompensas (Opcional)")]
    public bool healPlayerOnDeath = false;
    public int healthToRestore = 1;
    public bool increasePlayerSpeedOnDeath = false;
    public float speedToRestore = 0.1f;

    [Tooltip("¡Marca esto SOLO si este es el Jefe Final del juego!")]
    public bool isFinalBoss = false;
    public string victorySceneName = "Victoria"; 

 
    private Animator animator;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    public bool isDead = false;
    private GameObject healthCanvas;
    private EnemyAI enemyAI;

    void Start()
    {
        currentHealth = maxHealth;
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();

        if (animator == null)
        {
            useTakeHitAnimation = false;
            useDeathAnimation = false;
        }

        if (healthBarFill != null)
        {
            healthCanvas = healthBarFill.GetComponentInParent<Canvas>()?.gameObject;
            healthBarFill.fillAmount = 1;
            if (healthCanvas != null)
                healthCanvas.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (enemyAI != null)
        {
            enemyAI.CancelAttack();
        }

        currentHealth -= damage;

        if (healthBarFill != null)
        {
            if (healthCanvas != null)
                healthCanvas.SetActive(true);
            float fillAmount = (float)currentHealth / (float)maxHealth;
            healthBarFill.fillAmount = fillAmount;
        }

        if (currentHealth > 0)
        {
            if (useTakeHitAnimation && animator != null)
            {
                animator.SetTrigger("TakeHit");
            }
            else
            {
                StartCoroutine(BlinkEffect());
            }
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Enemigo ha muerto.");

       
        if (healthCanvas != null)
            healthCanvas.SetActive(false);
        if (enemyAI != null)
            enemyAI.enabled = false;
        col.enabled = false;
        this.enabled = false;

        // Es el jefe final?
        if (isFinalBoss)
        {
            Debug.Log("¡JEFE FINAL DERROTADO! Iniciando secuencia final...");

            if (useDeathAnimation && animator != null)
            {
                animator.SetTrigger("death");
                // ¡Iniciamos la corrutina para cargar la escena DESPUÉS!
                StartCoroutine(LoadVictorySceneAfterDelay());
            }
            else
            {
                // Si no tiene animación, cargar al instante
                Time.timeScale = 1f;
                SceneManager.LoadScene(victorySceneName);
            }

            Destroy(gameObject, destroyDelay);
            return;
        }


        // Dar recompensas (si las tiene)
        if (healPlayerOnDeath)
        {
            PlayerHealth.Instance?.Heal(healthToRestore);
        }
        if (increasePlayerSpeedOnDeath)
        {
            PlayerHealth.Instance?.ApplySpeedBoost(speedToRestore);
        }

        // Ejecutar lógica de muerte normal
        if (useDeathAnimation && animator != null)
        {
            animator.SetTrigger("death");
            Destroy(gameObject, destroyDelay);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator BlinkEffect()
    {
        if (spriteRenderer == null) yield break;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    IEnumerator LoadVictorySceneAfterDelay()
    {
        // Espera el tiempo que dura la animación de muerte
        yield return new WaitForSeconds(destroyDelay * 0.9f);

        // Ahora, carga la escena de victoria
        Debug.Log("Animación de muerte terminada. Cargando VictoryScene...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(victorySceneName);
    }
}