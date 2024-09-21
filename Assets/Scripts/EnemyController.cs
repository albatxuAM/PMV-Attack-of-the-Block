using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Movement Speed
    [SerializeField]
    private float speed;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        rb.velocity = randomDirection * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is called whenever the ball
    // collides with something
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.name == "Wall")
        {

        }
    }
}
