using System.Collections;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public float speed = 10f;
    public ParticleSystem hitEffect;
    public AudioSource hitSound1;
    public AudioSource hitSound2;
    
    private Rigidbody rb;
    private GameObject lastHitPaddle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(speed,0,0);
        AudioSource[] audioSources = GetComponents<AudioSource>();
        hitSound1 = audioSources[0];
        hitSound2 = audioSources[1];
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
            lastHitPaddle = other.gameObject;
            
            if (hitEffect != null)
            {
                hitEffect.Play();
            }

            float paddleHeight = 4f;
            float factor = (transform.position.z - other.transform.position.z) / (paddleHeight/2);
            Debug.Log("Factor=" + factor);
            
            Vector3 paddle = other.contacts[0].normal;
            Vector3 newDirection = Vector3.Reflect(rb.linearVelocity, paddle);

            newDirection.z += factor * speed;
            
            float currentSpeed = rb.linearVelocity.magnitude;
            float newSpeed = currentSpeed * 1.3f;
            Debug.Log("Ball speed:" + newSpeed);
            
            rb.linearVelocity = newDirection.normalized * newSpeed;
            
            if (factor >= 0f)
            {
                hitSound1.Play();
                Debug.Log("hitSound1");
            }
            else
            {
                hitSound2.Play();
                Debug.Log("hitSound2");
            }
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

    private IEnumerator ResetPaddleSize(GameObject paddle)
    {
        yield return new WaitForSeconds(10f);
        
        if (lastHitPaddle != null)
        {
            paddle.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    
    private IEnumerator ResetTime()
    {
        yield return new WaitForSeconds(10f);
        Time.timeScale = 1f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp") && lastHitPaddle != null)
        {
            if (other.gameObject.name == "PaddleSize")
            {
                Vector3 currentScale = lastHitPaddle.transform.localScale;
                lastHitPaddle.transform.localScale = new Vector3(currentScale.x, currentScale.y, 2f);
                other.gameObject.SetActive(false);
                StartCoroutine(ResetPaddleSize(lastHitPaddle));
            }
            else if (other.gameObject.name == "FastTime")
            {
                other.gameObject.SetActive(false);
                Time.timeScale = 1.75f;
                StartCoroutine(ResetTime());
            }
        }
    }
}

