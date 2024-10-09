using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu; 

    private bool isPaused = false;

    private PauseControls inputControls;

    private void Awake()
    {
        // Crear una instancia del archivo Input Actions
        inputControls = new PauseControls();

        // Desactivar el menú de pausa al inicio
        pauseMenu.SetActive(false);
    }

    private void OnEnable()
    {
        // Activa el Action Map "PauseInput"
        inputControls.PauseInput.Enable();

        // Suscribirse a la acción de Pausa
        inputControls.PauseInput.Pause.performed += OnPause;

    }

    private void OnDisable()
    {
        // Desactiva el Action Map "PauseInput"
        inputControls.PauseInput.Disable();

        // Desuscribirse a la acción de Pausa
        inputControls.PauseInput.Pause.performed -= OnPause;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        // Pausa el tiempo
        Time.timeScale = 0f; 
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        // Reanuda el tiempo
        Time.timeScale = 1f;
        pauseMenu.SetActive(false); 
    }

    public void LoadMainMenu()
    {
        // Asegúrate de reanudar el tiempo
        Time.timeScale = 1f;
        //Hacer cursor visible
        UnityEngine.Cursor.visible = false;
        SceneManager.LoadScene("MainMenu_Level");
    }
}
