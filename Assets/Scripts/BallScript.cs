using UnityEngine;

public class BallScript : MonoBehaviour
{
    public float speed = 10f;
    public ParticleSystem hitEffect;
    public AudioSource hitSound;
    
    private Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(speed,0,0);
        hitSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocity.magnitude < speed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        } 
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Paddle"))
        {
            if (hitEffect != null)
            {
                hitEffect.Play();
            }

            float paddleHeight = 4f;
            float factor = (transform.position.z - other.transform.position.z) / (paddleHeight/2);

            Vector3 paddle = other.contacts[0].normal;
            Vector3 newDirection = Vector3.Reflect(rb.linearVelocity, paddle);

            newDirection.z += factor * speed;
            
            float currentSpeed = rb.linearVelocity.magnitude;
            float newSpeed = currentSpeed * 1.3f;
            Debug.Log("Ball speed:" + newSpeed);
            
            rb.linearVelocity = newDirection.normalized * newSpeed;

            if (newSpeed > 8f && !hitSound.isPlaying)
            {
                hitSound.pitch = 1f;
                hitSound.volume = 1f;
            }
            hitSound.Play();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, -rb.linearVelocity.z).normalized *
                                rb.linearVelocity.magnitude;
        }
    }

    public void ResetBall(string direction)
    {
        if (direction == "left")
        {
            transform.position = new Vector3(-10, 1, 0);
            rb.linearVelocity = new Vector3(speed, 0, 0);
        }
        else if (direction == "right")
        {
            transform.position = new Vector3(10, 1, 0);
            rb.linearVelocity = new Vector3(-speed, 0, 0);
        }
    }

    public void StopBall()
    {
        rb.linearVelocity = Vector3.zero;
        Debug.Log("Ball stopped.");
    }
}

