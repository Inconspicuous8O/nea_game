using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.Examples;

public class Upgrade : MonoBehaviour
{
    [Header("UI elements")]
    public TextMeshProUGUI goldHut;
    public TextMeshProUGUI well;
    public TextMeshProUGUI mage;
    public TextMeshProUGUI cannon;
    public TextMeshProUGUI wall;
    public TextMeshProUGUI level;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI elixir;

    public static int currentlevel = 1;

    [Header("Requirements")]
    public static int goldHutRequirement = 2;
    public static int wellRequirement = 2;
    public static int mageRequirement = 2;
    public static int cannonRequirement = 2;
    public static int wallRequirement = 20;

    public static int goldRequirement = 100;
    public static int elixirRequirment = 100;

    [Header("Currents")]
    public static int goldHutCurrent = 0;
    public static int wellCurrent = 0;
    public static int mageCurrent = 0;
    public static int cannonCurrent = 0;
    public static int wallCurrent = 0;

    private bool Availability = false;

    void Start()
    {
        /// there 2 functions are called in intervals to not waste processing power
        InvokeRepeating("UpdateTHCanvas",0f, 2f);
        InvokeRepeating("ColourChange",0f, 2f);
    }

    public static void Built(string buildingName)
    {
        /// Switch case statement to decide what building was placed and what variable to increment
        switch (buildingName)
        {
            case "gold":
                goldHutCurrent++;
                break;
            case "well":
                wellCurrent++;
                break;
            case "mage":
                mageCurrent++;
                break;
            case "cannon":
                cannonCurrent++;
                break;
            case "wall":
                wallCurrent++;
                break;
        }
    }

    public void UpdateTHCanvas()
    {
        /// Changes the value of the UI text
        goldHut.text = goldHutCurrent.ToString() + "/" + goldHutRequirement.ToString();
        well.text = wellCurrent.ToString() + "/" + wellRequirement.ToString();
        mage.text = mageCurrent.ToString() + "/" + mageRequirement.ToString();
        cannon.text = cannonCurrent.ToString() + "/" + cannonRequirement.ToString();
        wall.text = wallCurrent.ToString() + "/" + wallRequirement.ToString();
        gold.text = goldRequirement.ToString();
        elixir.text = elixirRequirment.ToString();
        level.text = "Level " + currentlevel;
    } 

    public void ColourChange()
    {
        /// calling of the same function with different parameters for the different values
        UpdateBuildingColor(goldHut, goldHutCurrent, goldHutRequirement);
        UpdateBuildingColor(well, wellCurrent, wellRequirement);
        UpdateBuildingColor(mage, mageCurrent, mageRequirement);
        UpdateBuildingColor(cannon, cannonCurrent, cannonRequirement);
        UpdateBuildingColor(wall, wallCurrent, wallRequirement);
    }

    private void UpdateBuildingColor(TextMeshProUGUI buildingText, int currentCount, int requiredCount)
    {
        /// if statement to display if the requirement has been fulfilled
        if (currentCount == requiredCount)
        {
            buildingText.color = Color.green;
            Availability = true;
        }
        else
        {
            buildingText.color = Color.red;
            Availability = false;
        }
    }

    public void UpgradeTH()
    {
        /// If statement to decide if the user can progress
        if(Availability == true && goldRequirement <= ResourcesScript.currentGold && elixirRequirment <=ResourcesScript.currentElixir)
        {
            /// increase requirements, increasing the amount of buildings placeable
            goldHutRequirement = Mathf.FloorToInt((float)goldHutRequirement * 1.5f);
            wellRequirement = Mathf.FloorToInt((float)wellRequirement * 1.5f);
            mageRequirement = Mathf.FloorToInt((float)mageRequirement * 1.5f);
            cannonRequirement = Mathf.FloorToInt((float)cannonRequirement * 1.5f);
            wallRequirement = Mathf.FloorToInt((float)wallRequirement * 1.5f);
            goldRequirement = Mathf.FloorToInt((float)goldRequirement * 1.5f);
            elixirRequirment = Mathf.FloorToInt((float)elixirRequirment * 1.5f);
            
            currentlevel +=1;
        }
        else
        {
            Debug.Log("CANT upgrade");
        }

    }

    public void RequirmentChange(GameObject objectToDelete)
    {
        /// creating a local variable to store the name of the object to delete
        string objectName = objectToDelete.name;

        /// switch case statement when a building is deleted to change the appropriate variable
        switch (objectName)
        {
            /// only triggers if the string contains the word
            case string n when n.Contains("Gold Hut"):
                goldHutCurrent --;
                break;
            case string n when n.Contains("Wizard Hut"):
                wellCurrent --;
                break;
            case string n when n.Contains("Mage"):
                mageCurrent --;
                break;
            case string n when n.Contains("Cannon"):
                cannonCurrent --;
                break;
            case string n when n.Contains("Wall"):
                wallCurrent --;
                break;
        }
    }
}
