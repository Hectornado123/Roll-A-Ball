using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.tieneEscudo = true;

            if (player.escudoVisual != null)
                player.escudoVisual.SetActive(true);

            if (player.shieldSound != null)
                player.playerAudio.PlayOneShot(player.shieldSound);

            Destroy(gameObject); // El power-up desaparece
        }
    }
}


