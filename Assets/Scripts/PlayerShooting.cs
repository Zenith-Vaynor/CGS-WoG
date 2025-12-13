using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public float range = 20f;
    public LayerMask toBeHit;

    private Vector2 mousePos;
    private Vector2 aimDirection;
    void OnShoot(InputValue value)
    {
        if (value.isPressed)
        {
            RunRaycasts();
        }
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        aimDirection = (mousePos - (Vector2)firePoint.position).normalized;
        Debug.DrawRay(firePoint.position, aimDirection * range, Color.red);
    }

    void RunRaycasts()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, aimDirection, range, toBeHit);
        if (hit)
        {
            Debug.Log("Name of the collision : " + hit.collider.name);
        }
        else
            Debug.Log("No collision");

        RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, aimDirection, range, toBeHit);
        Debug.Log("Number of hits are :" + hits.Length);

    }
}

