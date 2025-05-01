using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Collectible : MonoBehaviour
{
    protected new Collider2D collider;

    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
            Destroy(gameObject);
        }
    }

    protected abstract void Collect();
}
