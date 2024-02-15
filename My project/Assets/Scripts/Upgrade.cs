using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.Examples;

public class Upgrade : MonoBehaviour
{
    public TextMeshProUGUI goldHut;
    public TextMeshProUGUI well;
    public TextMeshProUGUI mage;
    public TextMeshProUGUI cannon;
    public TextMeshProUGUI wall;
    public TextMeshProUGUI level;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI elixir;

    public static int currentlevel = 1;

    public static int goldHutRequirement = 2;
    public static int wellRequirement = 2;
    public static int mageRequirement = 2;
    public static int cannonRequirement = 2;
    public static int wallRequirement = 20;

    public static int goldRequirement = 100;
    public static int elixirRequirment = 100;

    public static int goldHutCurrent = 0;
    public static int wellCurrent = 0;
    public static int mageCurrent = 0;
    public static int cannonCurrent = 0;
    public static int wallCurrent = 0;

    private bool Availability = false;

    void Start()
    {
        InvokeRepeating("UpdateTHCanvas",0f, 5f);
        InvokeRepeating("ColourChange",0f, 5f);
    }

    public static void Built(string buildingName)
    {
        if (buildingName == "gold")
        {
            goldHutCurrent += 1;
        }
        else if (buildingName == "well")
        {
            wellCurrent += 1;
        }
        else if (buildingName == "mage")
        {
            mageCurrent += 1;
        }
        else if (buildingName == "cannon")
        {
            cannonCurrent += 1;
        }
        else if (buildingName == "wall")
        {
            wallCurrent += 1;
        }

    }

    public void UpdateTHCanvas()
    {
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
        if (goldHutCurrent == goldHutRequirement)
        {
            goldHut.color = Color.green;
            Availability = true;
        }
        else
        {
            goldHut.color = Color.red;
            Availability = false;
        }


        if (wellCurrent==wellRequirement)
        {
            well.color = Color.green;
            Availability = true;
        }
        else
        {
            well.color = Color.red;
            Availability = false;
        }

        if (mageCurrent==mageRequirement)
        {
            mage.color = Color.green;
            Availability = true;
        }
        else
        {
            mage.color = Color.red;
            Availability = false;
        }

        if (cannonCurrent==cannonRequirement)
        {
            cannon.color = Color.green;
            Availability = true;
        }
        else
        {
            cannon.color = Color.red;
            Availability = false;
        }

        if (wallCurrent==wallRequirement)
        {
            wall.color = Color.green;
            Availability = true;
        }
        else
        {
            wall.color = Color.red;
            Availability = false;
        }
    }

    public void UpgradeTH()
    {
        if(Availability == true && goldRequirement <= ResourcesScript.currentGold && elixirRequirment <=ResourcesScript.currentElixir)
        {
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
        if (objectToDelete.name.Contains("Gold Hut"))
        {
            goldHutCurrent -= 1;
        }
        else if (objectToDelete.name.Contains("Wizard Hut"))
        {
            wellCurrent -= 1;
        }
        else if (objectToDelete.name.Contains("Mage"))
        {
            mageCurrent -= 1;
        }
        else if (objectToDelete.name.Contains("Cannon"))
        {
            cannonCurrent -= 1;
        }
        else if (objectToDelete.name.Contains("Wall"))
        {
            wallCurrent -= 1;
        }

    }
}
