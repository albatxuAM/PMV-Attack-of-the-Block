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

    [SerializeField]
    private int  maxLives = 3;

    private int currentLives;
    private bool invincible = false;
    [SerializeField]
    private float invincibleTime = 3.0f;

    //parpadeo sprite
    private float blinkInterval = 0.2f;
    private SpriteRenderer spriteRenderer;

    //sounds
    public AudioClip playerSound, enemyHitSound, powerUpSound, wallSound;

    public UIManager uiManager;

    void Start()
    {
        //Set Cursor to not be visible
        UnityEngine.Cursor.visible = false;
        currentLives = maxLives;
        uiManager.updateLives(currentLives);

        // Obtén el SpriteRenderer para poder parpadear
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // convert mouse’s screen coordinates to a real-world position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Move player smoothly to mousePos
        transform.position = Vector2.MoveTowards(transform.position, mousePosition, moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Debug.Log(collisionInfo.gameObject.tag);

        if (collisionInfo.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }

        if (collisionInfo.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collisionInfo.gameObject);
            Heal(1);
        }

        if (collisionInfo.gameObject.CompareTag("Wall"))
        {
            //play sound
            GetComponent<AudioSource>().clip = wallSound;
            GetComponent<AudioSource>().Play();
        }
    }

    void TakeDamage(int damage)
    {
        if(!invincible) { 
            currentLives -= damage;
            if(currentLives < 0) currentLives = 0;
            uiManager.updateLives(currentLives);

            //play sound
            GetComponent<AudioSource>().clip = enemyHitSound;
            GetComponent<AudioSource>().Play();

            Invencivility();
            StartCoroutine(Blinking());
            Invoke(nameof(Invencivility), invincibleTime);
        }
    }

    void Heal(int healAmount)
    {
        currentLives += healAmount;
        if (currentLives > maxLives) currentLives = maxLives;
        uiManager.updateLives(currentLives);

        //play sound
        GetComponent<AudioSource>().clip = powerUpSound;
        GetComponent<AudioSource>().Play();
    }

    void Invencivility()
    {
        invincible = !invincible;
    }

    // Coroutine para parpadear mientras el jugador es invencible
    IEnumerator Blinking()
    {
        float elapsedTime = 0f;

        while (invincible)
        {
            // Alternar la visibilidad del SpriteRenderer
            spriteRenderer.enabled = !spriteRenderer.enabled;
            elapsedTime += blinkInterval;

            // Esperar el intervalo de parpadeo
            yield return new WaitForSeconds(blinkInterval);
        }

        // Asegurarse de que el sprite esté visible al terminar la invencibilidad
        spriteRenderer.enabled = true;
    }

    void InvincibilityOff()
    {
        // Desactivar invulnerabilidad
        invincible = false;  
    }
}
