using UnityEngine;

public class PowerUpDobleSalto : MonoBehaviour
{
    public float duracion = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().ActivarDobleSalto(duracion);
            gameObject.SetActive(false);
        }
    }
}
