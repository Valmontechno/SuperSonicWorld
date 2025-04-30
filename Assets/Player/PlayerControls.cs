using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private Inputs inputs;

    private Rigidbody2D rb;

    [Header("Player")]
    [SerializeField] private GameObject standingPlayer;
    [SerializeField] private GameObject balledPlayer;

    [Header("Move")]
    [SerializeField] private float accel;
    [SerializeField] private float speedMax;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpsMax;
    private int jumpsCount;
    [SerializeField] private float coyoteTime;
    private bool isGrounded;
    private Coroutine coyoteTimeCoroutine;
    [SerializeField] private float jumpRayCastDistance;

    private void Awake()
    {
        inputs = new Inputs();
        inputs.InGame.Enable();

        inputs.InGame.Jump.started += JumpStart;
        inputs.InGame.Jump.canceled += JumpEnd;
    }

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGrounded();
        Move();
    }

    public void CheckGrounded()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, jumpRayCastDistance);
    }

    private void Move()
    {
        float velocityX = rb.velocity.x;
        velocityX += inputs.InGame.Move.ReadValue<float>() * accel * Time.deltaTime;
        velocityX = Mathf.Clamp(velocityX, -speedMax, speedMax);
        rb.velocity = new(velocityX, rb.velocity.y);
    }

    private void OnDestroy()
    {
        inputs.InGame.Jump.started -= JumpStart;
        inputs.InGame.Jump.canceled -= JumpEnd;
    }

    private void JumpStart(InputAction.CallbackContext context)
    {
        if (isGrounded && jumpsCount == 0)
        {
            jumpsCount++;
            rb.velocity = new(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else if (0 < jumpsCount && jumpsCount < jumpsMax)
        {
            jumpsCount++;
            rb.velocity = new(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void JumpEnd(InputAction.CallbackContext context)
    {
        //if (rb.velocity.y > 0f)
        //{
        //    rb.velocity = new(rb.velocity.x, 0f);
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 3)
        {
            jumpsCount = 0;
            if (coyoteTimeCoroutine != null)
            {
                StopCoroutine(coyoteTimeCoroutine);
            }

            inputs.InGame.Move.Enable();
            inputs.InGame.Jump.Enable();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (gameObject.activeInHierarchy && collision.collider.gameObject.layer == 3)
        {
            coyoteTimeCoroutine = StartCoroutine(CoyoteTime());
        }
    }

    private IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(coyoteTime);
    }

    public void OnSpring()
    {
        inputs.InGame.Move.Disable();
        inputs.InGame.Jump.Disable();
    }

    //public void Balled(InputAction.CallbackContext context)
    //{
    //    standingPlayer.SetActive(false);
    //    balledPlayer.SetActive(true);
    //}

    //public void Stand(InputAction.CallbackContext context)
    //{
    //    standingPlayer.SetActive(true);
    //    balledPlayer.SetActive(false);
    //}
}