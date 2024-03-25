using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro.Examples;
using TMPro;

public class Points : MonoBehaviour
{
    private static int CurrentPoints;
    private static int ResourcePoints;
    private static int TotalPoints;
    public TextMeshProUGUI CurrentScore;

    void Start()
    {
        CurrentPoints = 0; /// reset current points
    }

    void Update()
    {
        /// resource point calculated from sum of gold and elixir
        ResourcePoints = ResourcesScript.currentElixir + ResourcesScript.currentGold;
        /// total resources calculated
        TotalPoints = (ResourcePoints - 2000)+ CurrentPoints;
        /// change current store UI
        if (TotalPoints < 0)
        {
            TotalPoints = 0;
        }

        CurrentScore.text = "Current Score: " + TotalPoints.ToString();
        
    }

    public static void SaveScore()
    {
        /// save the current score
        PlayerPrefs.SetInt("CurrentScore", TotalPoints);
        PlayerPrefs.Save();
    }

    public static void add_points(int PointsToAdd)
    {
        CurrentPoints += PointsToAdd; /// add points for when defeating enemies
    }
}
