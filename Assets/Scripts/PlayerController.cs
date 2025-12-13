using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator anim;

    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private Vector2 dashDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    public void OnDash(InputValue value)
    {
        if (value.isPressed)
        {
            Dash();
            Debug.Log("dash activated");
        }
    }

    void Dash()
    {
        if (isDashing)
            return;
        if (dashTimer > 0f)
            return;

        if (moveInput.sqrMagnitude > 0.001f)
        {
            isDashing = true;
            dashTimer = dashCooldown;
            dashDirection = moveInput.normalized;
        }
    }
    void Update()
    {
        if (dashCooldown > 0f)
        {
            dashCooldown -= Time.deltaTime;
        }

        // Setting up a mechanism to update the Sprite's Horizontal and Vertical positionings and actions only when the player is actually moving, i.e, the magnitude (sq(x)+sq(y)) is greater than 0
        anim.SetFloat("Speed", moveInput.sqrMagnitude * moveSpeed);
        if (moveInput.sqrMagnitude > 0.01f)
        {
            anim.SetFloat("Horizontal", moveInput.x);
            anim.SetFloat("Vertical", moveInput.y);
        }
    } 

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer < 0f)
            {
                isDashing = false;
                dashTimer = dashCooldown;
            }
            else
            {
                if (dashTimer > 0f)
                {
                    dashTimer -= Time.fixedDeltaTime;
                }
            }
        }
        else
        {
            Vector2 nextPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

            float wallX = Mathf.Clamp(nextPosition.x, minX, maxX);
            float wallY = Mathf.Clamp(nextPosition.y, minY, maxY);
            // This says If the value is lower than min, return min. If higher than max, return max.
            rb.MovePosition(new Vector2(wallX, wallY));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "box")
        {
            Debug.Log("enter collision");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "box")
        {
            Debug.Log("exit collision");
        }
    }




}