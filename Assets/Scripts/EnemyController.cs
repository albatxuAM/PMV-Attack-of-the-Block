using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Movement Speed
    [SerializeField]
    public float speed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
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
            // Calculate hit Factor
            float x = hitFactor(transform.position,
                            collisionInfo.transform.position,
                            collisionInfo.collider.bounds.size.x);

            // Calculate direction, set length to 1
            Vector2 dir = new Vector2(x, 1).normalized;

            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketWidth)
    {
        //
        // 1  -0.5  0  0.5   1  <- x value
        // ===================  <- wall
        //
        return (ballPos.x - racketPos.x) / racketWidth;
    }
}
