
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TeleportGem : MonoBehaviour
{
    [Header("Configuración de Teletransporte")]
    [Tooltip("Las coordenadas (X, Y) a donde irá el jugador")]
    public Vector2 teleportPosition = new Vector2(0f, 0f);

    [Header("Configuración de Música")]
    [Tooltip("La música que sonará al llegar a la nueva zona")]
    public AudioClip musicForNewZone;

    private Collider2D triggerCollider;

    void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
        triggerCollider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (musicForNewZone == null)
            {
                Debug.LogError("¡Gema de teletransporte no tiene musica asignada!", this);
                return;
            }
            if (AudioManager.Instance == null)
            {
                Debug.LogError("Tiene que ejecutarse desde menu", this);
                return;
            }

            AudioManager.Instance.PlayNewTrack(musicForNewZone);

         

            Transform playerTransform = other.transform.root;
            Rigidbody2D rb = playerTransform.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
               
                rb.position = teleportPosition;
                rb.linearVelocity = Vector2.zero; 
            }
            else
            {
                playerTransform.position = teleportPosition;
            }

            Debug.Log("¡Teletransporte y música activados!");
        }
    }
}