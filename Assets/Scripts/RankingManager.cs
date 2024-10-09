using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingManager : MonoBehaviour
{
    public List<TextMeshProUGUI> rankingTexts;
    public TextMeshProUGUI lastScoreText; 
    private float lastScore;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();

        LoadBestTimes();
        LoadLastScore();
    }

    private void LoadBestTimes()
    {
        float lastScore = PlayerPrefs.GetFloat("LastScore", 0); // Obtener la última puntuación
        int lastScoreIndex = -1; // Variable para almacenar el índice de lastScore en el ranking

        for (int i = 0; i < 5; i++)
        {
            float bestTime = PlayerPrefs.GetFloat("BestTime" + i, 0);

            if (bestTime > 0) // Verifica si el tiempo es mayor a 0
            {
                int minutes = Mathf.FloorToInt(bestTime / 60F);
                int seconds = Mathf.FloorToInt(bestTime % 60F);
                int milliseconds = Mathf.FloorToInt((bestTime * 100F) % 100F);

                rankingTexts[i].text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";

                // Comprobar si el mejor tiempo actual es igual a lastScore
                if (bestTime == lastScore)
                {
                    lastScoreIndex = i; // Almacenar el índice
                }
            }
            else
            {
                rankingTexts[i].text = "---";  // No hay tiempo guardado
            }
        }

        // Resaltar el lastScore en el ranking si se encontró
        if (lastScoreIndex != -1)
        {
            // Cambiar el color y estilo de texto para el ranking
            rankingTexts[lastScoreIndex].color = Color.red; // Cambiar el color del texto a rojo
            rankingTexts[lastScoreIndex].fontStyle = FontStyles.Bold; // Hacer el texto en negrita
        }
    }

    private void LoadLastScore()
    {
        lastScore = PlayerPrefs.GetFloat("LastScore", 0);

        if (lastScore > 0)
        {
            int lastMinutes = Mathf.FloorToInt(lastScore / 60F);
            int lastSeconds = Mathf.FloorToInt(lastScore % 60F);
            int lastMilliseconds = Mathf.FloorToInt((lastScore * 100F) % 100F);
            lastScoreText.text = $"Last Score {lastMinutes:00}:{lastSeconds:00}:{lastMilliseconds:00}";
        }
        else
        {
            lastScoreText.text = "No hay puntuación anterior.";
        }
    }
}
