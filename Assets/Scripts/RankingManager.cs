using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingManager : MonoBehaviour
{
    public List<TextMeshProUGUI> rankingTexts;  // Asigna aquí los campos de texto para los 5 mejores tiempos

    private void Start()
    {
        LoadBestTimes();
    }

    private void LoadBestTimes()
    {
        for (int i = 0; i < 5; i++)
        {
            float bestTime = PlayerPrefs.GetFloat("BestTime" + i, Mathf.Infinity);

            if (bestTime != Mathf.Infinity)
            {
                int minutes = Mathf.FloorToInt(bestTime / 60F);
                int seconds = Mathf.FloorToInt(bestTime % 60F);
                int milliseconds = Mathf.FloorToInt((bestTime * 100F) % 100F);

                rankingTexts[i].text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
            }
            else
            {
                rankingTexts[i].text = "---";  // No hay tiempo guardado
            }
        }
    }
}
