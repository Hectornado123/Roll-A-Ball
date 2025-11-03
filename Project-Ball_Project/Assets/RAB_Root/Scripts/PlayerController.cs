using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Editor References")]
    public Rigidbody playerRb; //Referencia al Rigidbody del player
    public AudioSource playerAudio; //Ref al emisor de sonidos del player

    [Header("Movement Parameters")]
    public float speed = 10;
    public Vector2 moveInput; //Almacén del input de movimiento de los periféricos que usamos para jugar

    [Header("Jump Parameters")]
    public float jumpForce = 6;
    public bool isGrounded = true;

    [Header("Respawn System")]
    public float fallLimit = -10;
    public Transform respawnPoint;

    [Header("Sound Configuration")]
    public AudioClip[] soundCollection;

    [Header("Shield System")]
    public bool tieneEscudo = false;          // si el jugador tiene escudo
    public GameObject escudoVisual;           // referencia al objeto visual del escudo
    public AudioClip shieldSound;             // sonido opcional al activar o romper el escudo

    void Start()
    {
        if (escudoVisual != null)
            escudoVisual.SetActive(false);
    }

    void Update()
    {
        //Respawn por altura
        if (transform.position.y <= fallLimit)
        {
            Respawn();
        }

        // Mostrar u ocultar el escudo visual
        if (escudoVisual != null)
            escudoVisual.SetActive(tieneEscudo);
    }

    private void FixedUpdate()
    {
        PhysicalMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detectar si toca el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Colisión con obstáculo o enemigo
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (tieneEscudo)
            {
                // El escudo bloquea el golpe y se desactiva
                tieneEscudo = false;

                if (escudoVisual != null)
                    escudoVisual.SetActive(false);

                if (shieldSound != null)
                    playerAudio.PlayOneShot(shieldSound);

                Debug.Log("Golpe bloqueado con el escudo.");
            }
            else
            {
                // Sin escudo → respawn normal
                Respawn();
            }
        }
    }

    void PhysicalMovement()
    {
        playerRb.AddForce(Vector3.right * speed * moveInput.x);
        playerRb.AddForce(Vector3.forward * speed * moveInput.y);
    }

    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        PlaySFX(0);
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;
        playerRb.linearVelocity = Vector3.zero;
        PlaySFX(2);
    }

    public void PlaySFX(int soundToPlay)
    {
        playerAudio.PlayOneShot(soundCollection[soundToPlay]);
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            isGrounded = false;
            Jump();
        }
    }

    #endregion
}


