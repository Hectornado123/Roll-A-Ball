using UnityEngine;

public class PlataformaSimple : MonoBehaviour
{
    public float tiempoReaparecer = 3f;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Invoke("Reaparecer", tiempoReaparecer);
        }
    }

    void Reaparecer()
    {
        gameObject.SetActive(true);
    }
}

