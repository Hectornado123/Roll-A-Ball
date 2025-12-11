using UnityEngine;

public class PlataformaSimpleToggle : MonoBehaviour
{
    public float intervalo = 2f;
    Renderer rend;
    Collider col;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        InvokeRepeating("Toggle", intervalo, intervalo);
    }

    void Toggle()
    {
        bool estado = !rend.enabled;
        rend.enabled = estado;
        col.enabled = estado;
    }
}
