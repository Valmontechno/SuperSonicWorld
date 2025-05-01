using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    Inputs inputs;

    Rigidbody2D rb;

    [Header("Move")]
    [SerializeField] float accel;
    [SerializeField] float speedMax;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] int jumpsMax;
    int jumpsCount;
    [SerializeField] float coyoteTime;
    Coroutine coyoteTimeCoroutine;
    bool coyoteCanJump = false;
    [SerializeField] float jumpRayCastDistance;
    [SerializeField] LayerMask groundLayerMask;
    bool isGrounded;

    void Awake()
    {
        inputs = new Inputs();
        inputs.InGame.Enable();

        inputs.InGame.Jump.started += JumpStart;
        inputs.InGame.Jump.canceled += JumpEnd;
    }
    void OnDestroy()
    {
        inputs.InGame.Jump.started -= JumpStart;
        inputs.InGame.Jump.canceled -= JumpEnd;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGrounded();
        Move();
    }

    public void CheckGrounded()
    {
        bool newGrounded = Physics2D.Raycast(transform.position, Vector2.down, jumpRayCastDistance, groundLayerMask);

        if (newGrounded != isGrounded)
        {
            if (newGrounded) // OnCollisionEnter2D
            {
                jumpsCount = 0;
                if (coyoteTimeCoroutine != null)
                {
                    StopCoroutine(coyoteTimeCoroutine);
                }

                inputs.InGame.Move.Enable();
                inputs.InGame.Jump.Enable();
            }
            else if (gameObject.activeInHierarchy) // OnCollisionExit2D
            {
                coyoteTimeCoroutine = StartCoroutine(CoyoteTime());
            }
        }

        isGrounded = newGrounded;
    }

    void Move()
    {
        float velocityX = rb.velocity.x;
        velocityX += inputs.InGame.Move.ReadValue<float>() * accel * Time.deltaTime;
        velocityX = Mathf.Clamp(velocityX, -speedMax, speedMax);
        rb.velocity = new(velocityX, rb.velocity.y);
    }

    void JumpStart(InputAction.CallbackContext context)
    {
        // Si touche le sol, ou coyoteTime, ou double saut
        if (isGrounded || coyoteCanJump || (0 < jumpsCount && jumpsCount < jumpsMax))
        {
            jumpsCount++;
            coyoteCanJump = false;
            rb.velocity = new(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void JumpEnd(InputAction.CallbackContext context)
    {
        // Arrêtez de sauter quand le bouton est relâché
        //if (rb.velocity.y > 0f)
        //{
        //    rb.velocity = new(rb.velocity.x, 0f);
        //}
    }

    IEnumerator CoyoteTime()
    {
        if (jumpsCount == 0)
        {
            coyoteCanJump = true;
            yield return new WaitForSeconds(coyoteTime);
            coyoteCanJump = false;
        }
    }

    public void OnSpring()
    {
        inputs.InGame.Move.Disable();
        inputs.InGame.Jump.Disable();
    }
}