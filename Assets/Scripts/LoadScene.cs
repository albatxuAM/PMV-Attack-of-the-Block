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
}
