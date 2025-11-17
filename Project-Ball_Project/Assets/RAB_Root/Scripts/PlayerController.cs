using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // ─────────────────────────────
    // REFERENCIAS DEL EDITOR
    // ─────────────────────────────
    public Rigidbody playerRb;
    public AudioSource playerAudio;

    // ─────────────────────────────
    // PARÁMETROS DE MOVIMIENTO
    // ─────────────────────────────
    public float speed = 10;
    public Vector2 moveInput;

    // ─────────────────────────────
    // SISTEMA DE SALTO
    // ─────────────────────────────
    public float jumpForce = 6;
    public bool isGrounded = true;

    // ► DOBLE SALTO
    public bool dobleSaltoActivo = false;        // si el power-up está activo
    private bool dobleSaltoDisponible = false;   // si puede hacer el doble salto

    // ► tiempo restante del power-up
    public float tiempoDobleSalto = 0f;

    // ─────────────────────────────
    // SISTEMA DE RESPAWN
    // ─────────────────────────────
    public float fallLimit = -10;
    public Transform respawnPoint;

    // ─────────────────────────────
    // SONIDOS
    // ─────────────────────────────
    public AudioClip[] soundCollection;

    // ─────────────────────────────
    // SISTEMA DE ESCUDO
    // ─────────────────────────────
    public bool tieneEscudo = false;
    public GameObject escudoVisual;
    public AudioClip shieldSound;


    void Start()
    {
        if (escudoVisual != null)
            escudoVisual.SetActive(false);
    }


    void Update()
    {
        // Respawn por caída
        if (transform.position.y <= fallLimit)
            Respawn();

        // Mostrar escudo visual
        if (escudoVisual != null)
            escudoVisual.SetActive(tieneEscudo);

        // ─────────────────────────────
        // CONTADOR DEL DOBLE SALTO
        // ─────────────────────────────
        if (dobleSaltoActivo)
        {
            tiempoDobleSalto -= Time.deltaTime;

            if (tiempoDobleSalto <= 0)
            {
                dobleSaltoActivo = false;
                dobleSaltoDisponible = false;
            }
        }
    }


    private void FixedUpdate()
    {
        PhysicalMovement();
    }


    private void OnCollisionEnter(Collision collision)
    {
        // ── DETECTAR SUELO ─────────────────
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            dobleSaltoDisponible = true;
        }

        // ── COLISIÓN CON OBSTÁCULOS ─────────
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (tieneEscudo)
            {
                tieneEscudo = false;

                if (escudoVisual != null)
                    escudoVisual.SetActive(false);

                if (shieldSound != null)
                    playerAudio.PlayOneShot(shieldSound);
            }
            else
            {
                Respawn();
            }
        }
    }


    // ─────────────────────────────
    // MOVIMIENTO FÍSICO
    // ─────────────────────────────
    void PhysicalMovement()
    {
        playerRb.AddForce(Vector3.right * speed * moveInput.x);
        playerRb.AddForce(Vector3.forward * speed * moveInput.y);
    }


    // ─────────────────────────────
    // SALTO
    // ─────────────────────────────
    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        PlaySFX(0);
    }


    // ─────────────────────────────
    // RESPAWN
    // ─────────────────────────────
    void Respawn()
    {
        transform.position = respawnPoint.position;
        playerRb.linearVelocity = Vector3.zero;
        PlaySFX(2);

        isGrounded = true;

        // Si el power-up sigue activo, permite doble salto
        dobleSaltoDisponible = dobleSaltoActivo;
    }


    // ─────────────────────────────
    // CHECKPOINT: ACTUALIZAR RESPAWN
    // ─────────────────────────────
    public void UpdateRespawn(Vector3 newRespawnPos)
    {
        respawnPoint.position = newRespawnPos;
    }


    // ─────────────────────────────
    // SONIDO
    // ─────────────────────────────
    public void PlaySFX(int soundToPlay)
    {
        playerAudio.PlayOneShot(soundCollection[soundToPlay]);
    }


    // ─────────────────────────────
    // INPUT DEL JUGADOR
    // ─────────────────────────────
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // Salto normal
        if (isGrounded)
        {
            isGrounded = false;
            dobleSaltoDisponible = true;
            Jump();
        }
        // Doble salto
        else if (dobleSaltoActivo && dobleSaltoDisponible)
        {
            dobleSaltoDisponible = false;
            Jump();
        }
    }


    // ─────────────────────────────
    // ACTIVAR POWER-UP DE DOBLE SALTO
    // ─────────────────────────────
    public void ActivarDobleSalto(float duracion)
    {
        dobleSaltoActivo = true;
        tiempoDobleSalto = duracion;
        dobleSaltoDisponible = true;
    }
}
