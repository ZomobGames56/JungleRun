using UnityEngine;

public class GhostMovement_Horror : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 targetPosition;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position + Vector3.back * 20f;
    }

    void FixedUpdate()
    {
        Vector3 newPos = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }
}
