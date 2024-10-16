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
    private int maxLives = 3;

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

    public GameObject shieldVisual;
    private bool hasShield = false;
    public float shieldDuration = 5f;

    public float freezeDuration = 5f;

    private Rigidbody2D rb;

    private Animator animator;

    public float stopDistance = 0.5f;

    void Start()
    {
        //Set Cursor to not be visible
        UnityEngine.Cursor.visible = false;
        currentLives = maxLives;
        uiManager.updateLives(currentLives);

        // Obt�n el SpriteRenderer para poder parpadear
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();

        // Obtiene la referencia al componente Animator del Player
        animator = GetComponent<Animator>();

        animator.SetBool("Flying", true);
    }

    void Update()
    {
        // Convertir las coordenadas de la pantalla del mouse a una posici�n en el mundo
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calcular la direcci�n hacia el mouse
        Vector2 direction = (mousePosition - rb.position).normalized;

        // Calcular la distancia al mouse
        float distance = Vector2.Distance(rb.position, mousePosition);

        // Mover solo si est� m�s lejos que la distancia de parada
        if (distance > stopDistance)
        {
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero; // Detenerse
        }

        // Mantener la rotaci�n en 0
        rb.rotation = 0f;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Debug.Log(collisionInfo.gameObject.tag);

        if (collisionInfo.gameObject.CompareTag("Enemy"))
        {
            if (!invincible)
            {
                if (hasShield)
                {
                    // Si el escudo est� activo, se desactiva al recibir un golpe
                    DeactivateShield();
                }
                else
                {
                    // C�digo para reducir vida al jugador
                    TakeDamage(1);
                }
            }
        }

        if (collisionInfo.gameObject.CompareTag("HearthPowerUp"))
        {
            Destroy(collisionInfo.gameObject);
            Heal(1);
        }

        if (collisionInfo.gameObject.CompareTag("ShieldPowerUp"))
        {
            ActivateShield();
            Destroy(collisionInfo.gameObject);
        }

        if (collisionInfo.gameObject.CompareTag("Wall"))
        {
            //play sound
            GetComponent<AudioSource>().clip = wallSound;
            GetComponent<AudioSource>().Play();
        }

        if (collisionInfo.gameObject.CompareTag("FreezePowerUp"))
        {
            FreezeEnemies();
            Destroy(collisionInfo.gameObject);
        }
    }

    void TakeDamage(int damage)
    {
        if (!invincible)
        {
            currentLives -= damage;
            if (currentLives < 0) currentLives = 0;
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

        // Asegurarse de que el sprite est� visible al terminar la invencibilidad
        spriteRenderer.enabled = true;
    }

    void InvincibilityOff()
    {
        // Desactivar invulnerabilidad
        invincible = false;
    }

    void ActivateShield()
    {
        hasShield = true;
        shieldVisual.SetActive(true);
        // Desactiva el escudo despu�s de X segundos
        Invoke(nameof(DeactivateShield), shieldDuration);
    }

    void DeactivateShield()
    {
        hasShield = false;
        shieldVisual.SetActive(false);
    }
    void FreezeEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().Freeze(freezeDuration);
        }
    }
}
