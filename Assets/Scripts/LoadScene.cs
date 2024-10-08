using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private string newScene;
    public void LoadLevel()
    {
        SceneManager.LoadScene(newScene);
    }

    public void QuitApplication()
    {
        // Si est�s en el editor, detenemos la simulaci�n
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Si es un build, cerramos la aplicaci�n
            Application.Quit();
        #endif
    }
}
