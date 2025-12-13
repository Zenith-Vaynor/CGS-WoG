using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveInput;

    public float dashCooldown;
    public float dashDistance;
    public float safetyBuffer = 0.5f;
    private float dashTimer = 0f;

    public LayerMask wallLayer;

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
        }
    }

    void Dash()
    {
        if (dashTimer > 0f) return;

        if (moveInput.sqrMagnitude > 0.001f)
        {
            Vector2 direction = moveInput.normalized;
            Vector2 finalSafePosition = CalculateSafeDashPosition(direction);

            transform.position = finalSafePosition;

            dashTimer = dashCooldown;

            Debug.Log("Dash executed to: " + finalSafePosition);
        }
    }

    public Vector2 CalculateSafeDashPosition(Vector2 dashDirection)
    {
        Vector2 startPos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(startPos, dashDirection, dashDistance, wallLayer);

        if (hit.collider != null)
        {
            Debug.Log("Wall detected!");
            Debug.Log("Hit: " + hit.collider.name);
            Debug.DrawLine(startPos, hit.point, Color.red, 1f);
            return hit.point - (dashDirection * safetyBuffer);
        }
        else
        {
            Debug.Log("Path clear.");
            Vector2 targetPos = startPos + (dashDirection * dashDistance);
            Debug.DrawLine(startPos, targetPos, Color.green, 1f);
            return targetPos;
        }
    }

    void Update()
    {
        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
        }

        if (anim != null)
        {
            anim.SetFloat("Speed", moveInput.sqrMagnitude * moveSpeed);
            if (moveInput.sqrMagnitude > 0.01f)
            {
                anim.SetFloat("Horizontal", moveInput.x);
                anim.SetFloat("Vertical", moveInput.y);
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 nextPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPosition);
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