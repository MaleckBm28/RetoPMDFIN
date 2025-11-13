
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;

    [Header("Audio (Loop)")]
    public AudioSource footstepAudioSource;

 
    [Header("Audio (SFX)")]
    [Tooltip("El AudioSource que reproducirá los sonidos de 'un solo golpe' (OneShot)")]
    public AudioSource sfxAudioSource; 
    [Tooltip("El clip de sonido para el salto")]
    public AudioClip jumpSound;
   

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    //private bool m_combatIdle = false;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }

    void Update()
    {
        
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        
        float inputX = Input.GetAxis("Horizontal");

        
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

       
        m_body2d.linearVelocity = new Vector2(inputX * m_speed, m_body2d.linearVelocity.y);
        m_animator.SetFloat("AirSpeed", m_body2d.linearVelocity.y);

  
       // if (Input.GetKeyDown("f"))
       //    m_combatIdle = !m_combatIdle;

        //Jump
        if (Input.GetKeyDown("space") && m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.linearVelocity = new Vector2(m_body2d.linearVelocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);

            // Parar sonido de pasos al saltar
            if (footstepAudioSource != null && footstepAudioSource.isPlaying)
                footstepAudioSource.Stop();

           
            if (sfxAudioSource != null && jumpSound != null)
            {
                
                sfxAudioSource.Stop();
                sfxAudioSource.clip = jumpSound;
                sfxAudioSource.Play();
            }
        }

        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            m_animator.SetInteger("AnimState", 2);
            // sonido de pasos
            if (m_grounded && footstepAudioSource != null && !footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Play();
            }
        }



        //Idle
        else
        {
            m_animator.SetInteger("AnimState", 0);
            // cancela pasos
            if (footstepAudioSource != null && footstepAudioSource.isPlaying)
                footstepAudioSource.Stop();
        }
    }

    public void IncreaseSpeed(float amount)
    {
        m_speed += amount;
        Debug.Log($"¡VELOCIDAD AUMENTADA! Nueva velocidad: {m_speed}");
    }
}