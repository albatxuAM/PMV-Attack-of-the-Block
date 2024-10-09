using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI Score;
    public List<GameObject> Hearts;

    private float Timer;
    private bool isGameOver = false;

    private void Update()
    {
        if (!isGameOver)
        {
            Timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(Timer / 60F);
            int seconds = Mathf.FloorToInt(Timer % 60F);
            int milliseconds = Mathf.FloorToInt((Timer * 100F) % 100F);
            Score.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
        }
    }

    public void updateLives(int lifeCount)
    {
        for (int i = 0; i < Hearts.Count; i++)
        {
            if (i < lifeCount)
            {
                Hearts[i].SetActive(true);
            }
            else
            {
                Hearts[i].SetActive(false);
            }
        }

        if (lifeCount == 0)
        {
            isGameOver = true;
            SaveScore(Timer);  // Guardar el tiempo cuando el juego termina
            SceneManager.LoadScene("GameOver_Level");
        }
    }

    private void SaveScore(float playerTime)
    {
        List<float> bestTimes = new List<float>();

        // Cargar los 5 mejores tiempos actuales de PlayerPrefs
        for (int i = 0; i < 5; i++)
        {
            bestTimes.Add(PlayerPrefs.GetFloat("BestTime" + i, 0)); // Cambia Mathf.Infinity a 0 para que los mejores tiempos sean mayores
        }

        // Añadir el tiempo del jugador actual
        bestTimes.Add(playerTime);

        // Ordenar la lista en orden descendente (de mayor a menor)
        bestTimes.Sort((a, b) => b.CompareTo(a));
        bestTimes = bestTimes.GetRange(0, Mathf.Min(5, bestTimes.Count));

        // Guardar los 5 mejores tiempos actualizados
        for (int i = 0; i < bestTimes.Count; i++)
        {
            PlayerPrefs.SetFloat("BestTime" + i, bestTimes[i]);
        }

        // Guardar la última puntuación si está en el ranking
        if (bestTimes.Contains(playerTime))
        {
            PlayerPrefs.SetFloat("LastScore", playerTime);
        }
        else
        {
            PlayerPrefs.DeleteKey("LastScore"); // Eliminar la última puntuación si no está en el ranking
        }
    }
}
