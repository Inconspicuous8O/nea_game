using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro.Examples;
using TMPro;

public class ScoreSaver : MonoBehaviour
{
    [Header("Scores")]
    public int currentScore;
    public int highestScore;

    [Header("UI")]
    public TextMeshProUGUI HIGHSCORE;
    public TextMeshProUGUI CURRENTSCORE;

    void Start()
    {
        currentScore = PlayerPrefs.GetInt("CurrentScore", 0); /// gets saved current score
        if (PlayerPrefs.HasKey("HighestScore"))
        {
            // Key exists, get it
            highestScore = PlayerPrefs.GetInt("HighestScore");
        }
        else
        {
            // Key doesn't exist
            highestScore = 0;
        }

        if (currentScore > highestScore)
        {
            highestScore = currentScore; /// change the high score to the current score
            /// save the high score
            PlayerPrefs.SetInt("HighestScore", (highestScore));
            PlayerPrefs.Save();
        }

        HIGHSCORE.text = "HIGHEST SCORE: " + highestScore.ToString(); /// change highest score UI
        CURRENTSCORE.text = "SCORE:" +currentScore.ToString(); /// change current score UI
    }
}
