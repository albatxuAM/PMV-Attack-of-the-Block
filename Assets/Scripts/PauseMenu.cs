using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPausa;
    private bool juegoPausado = false;

    private PauseControls inputControls;

    private void Awake()
    {
        // Crear una instancia del archivo Input Actions
        inputControls = new PauseControls();

        Reanudar();
    }

    private void OnEnable()
    {
        // Activa el Action Map "PauseInput"
        inputControls.PauseInput.Enable();

        // Suscribirse a la acci�n de Pausa
        inputControls.PauseInput.Pause.performed += OnPause;

    }

    private void OnDisable()
    {
        // Desactiva el Action Map "PauseInput"
        inputControls.PauseInput.Disable();

        // Desuscribirse a la acci�n de Pausa
        inputControls.PauseInput.Pause.performed -= OnPause;
    }

    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;  // Reanuda el tiempo
        juegoPausado = false;
    }

    public void Pausar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0f;  // Pausa el tiempo
        juegoPausado = true;
    }

    public void SalirDelJuego()
    {
        // Si est�s en el editor, detenemos la simulaci�n
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Si es un build, cerramos la aplicaci�n
            Application.Quit();
#endif
    }

    // Funci�n llamada cuando se presiona la tecla de pausa
    private void OnPause(InputAction.CallbackContext context)
    {
        if (juegoPausado)
        {
            Reanudar();
        }
        else
        {
            Pausar();
        }
    }
}
