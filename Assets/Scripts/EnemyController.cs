using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Movement Speed
    [SerializeField]
    private float speed;

    private Rigidbody2D rb;

    private bool isFrozen = false;
    private Vector2 originalVelocity;

    private Vector2 randomDirection;
    private float aceleration;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float angle = Random.Range(0f, 360f);
        randomDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        //randomDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));

        rb.velocity = randomDirection * speed;

        aceleration = speed;
    }

    void FixedUpdate()
    {
        aceleration += speed * Time.fixedDeltaTime;
        rb.velocity = randomDirection * aceleration;
        // rb.velocity += rb.velocity * Time.fixedDeltaTime;
        Debug.Log(aceleration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Al chocar, calcula la nueva dirección reflejada
        Vector2 normal = collision.contacts[0].normal;
        randomDirection = Vector2.Reflect(randomDirection, normal).normalized;
        rb.velocity = randomDirection * aceleration;
    }

    // Método para congelar el enemigo
    public void Freeze(float duration)
    {
        if (!isFrozen)
        {
            isFrozen = true;
            originalVelocity = rb.velocity;
            // Reduce la velocidad a un 20% de la original
            rb.velocity = originalVelocity * 0.2f;
            GetComponent<SpriteRenderer>().color = Color.blue;

            StartCoroutine(Unfreeze(duration));
        }
    }

    // Coroutine para descongelar al enemigo
    private IEnumerator Unfreeze(float duration)
    {
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        GetComponent<SpriteRenderer>().color = Color.white;

        // Reactivar la lógica de movimiento del enemigo
        rb.velocity = originalVelocity;
    }
}
