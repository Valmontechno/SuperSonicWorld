using UnityEngine;

public class SpringBehaviour : MonoBehaviour
{
    [SerializeField] private float launchForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up.normalized * launchForce, ForceMode2D.Impulse);

            PlayerControls playerControls = collision.GetComponent<PlayerControls>();
            playerControls.OnSpring();
        }
    }
}
