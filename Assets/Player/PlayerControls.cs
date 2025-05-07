using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

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
    Vector2 groundTangent = Vector2.right;


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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (groundLayerMask == (groundLayerMask | (1 << collision.gameObject.layer)))
        {
            Vector2 normal = collision.contacts[0].normal.normalized;

            if (Mathf.Abs(normal.x) <= 0.5f)
            {
                groundTangent = new Vector2(normal.y, -normal.x);
            } else
            {
                groundTangent = Vector2.right;
            }

            //Vector2 normal = collision.contacts[0].normal;
            //normal = new(Mathf.Abs(normal.x), Mathf.Abs(normal.y));
            //groundTangent = Vector2.Perpendicular(normal);
            //groundTangent.Normalize();
            //print(groundTangent);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        groundTangent = Vector2.right;
    }

    public void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, jumpRayCastDistance, groundLayerMask);
        bool newGrounded = hit;

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
        Vector2 velocity = rb.velocity;
        velocity += groundTangent * inputs.InGame.Move.ReadValue<float>() * accel * Time.deltaTime;
        velocity.x = Mathf.Clamp(velocity.x, -speedMax, speedMax);
        rb.velocity = velocity;
        print(groundTangent);

        //float velocityX = rb.velocity.x;
        //velocityX += inputs.InGame.Move.ReadValue<float>() * accel * Time.deltaTime;
        //velocityX = Mathf.Clamp(velocityX, -speedMax, speedMax);
        //rb.velocity = new(velocityX, rb.velocity.y);
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