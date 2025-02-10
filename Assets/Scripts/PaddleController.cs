using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;
    public string inputAxis;

    private Rigidbody rb;
    private float upperBoundary = 12.99f;
    private float lowerBoundary = -12.99f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float move = Input.GetAxisRaw(inputAxis) * speed * Time.fixedDeltaTime;
        Vector3 newPosition = rb.position + new Vector3(0, 0, move);

        newPosition.z = Mathf.Clamp(newPosition.z, lowerBoundary, upperBoundary);
        rb.MovePosition(newPosition);
    }
}