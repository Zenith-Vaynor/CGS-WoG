using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float sprintMultiplier = 2f;

    [Header("Boundary Settings")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = moveSpeed * sprintMultiplier; 
        }
        else
        {
            currentSpeed = moveSpeed; 
        }
        
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.sqrMagnitude > 0.01f)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
    }

    void FixedUpdate()
    {
        Vector2 nextPosition = rb.position + movement * currentSpeed * Time.fixedDeltaTime;

        float clampedX = Mathf.Clamp(nextPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(nextPosition.y, minY, maxY);

        rb.MovePosition(new Vector2(clampedX, clampedY));
    }
}