using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Movement Speed
    [SerializeField]
    private float speed;

    private Rigidbody2D rb;

    private bool isFrozen = false;
    private Vector2 originalVelocity; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        rb.velocity = randomDirection * speed;
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
