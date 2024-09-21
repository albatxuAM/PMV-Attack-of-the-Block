using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Set Cursor to not be visible
        UnityEngine.Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // convert mouse’s screen coordinates to a real-world position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Move player smoothly to mousePos
        transform.position = Vector2.MoveTowards(transform.position, mousePosition, moveSpeed * Time.deltaTime);
    }

    // This function is called whenever the ball
    // collides with something
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Debug.Log(collisionInfo.gameObject.tag);
        if (collisionInfo.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(collisionInfo.gameObject.tag);
            UnityEngine.Cursor.visible = true;
            //Application.Quit();
            SceneManager.LoadScene("GameOver_Level");
        }
    }
}
