using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using TMPro;

public class ResourcesScript : MonoBehaviour
{
    public static int currentGold;
    public static int currentElixir;

    public static int goldPerSec;
    public static int elixirPerSec;

    [Header("Base")]
    public TextMeshProUGUI baseGold;
    public TextMeshProUGUI baseElixir;

    [Header("Build")]
    public TextMeshProUGUI buildGold;
    public TextMeshProUGUI buildElixir;

    [Header("Stats")]
    public TextMeshProUGUI statsGold;
    public TextMeshProUGUI statsElixir;

    [Header("Troop")]
    public TextMeshProUGUI troopGold;
    public TextMeshProUGUI troopElixir;

    private float timer = 0.0f;

    public void Update(){
        /// Updates the UI text every frame
        baseGold.text = currentGold.ToString();
        baseElixir.text = currentElixir.ToString();

        buildGold.text = currentGold.ToString();
        buildElixir.text = currentElixir.ToString();

        troopGold.text = currentGold.ToString();
        troopElixir.text = currentElixir.ToString();

        statsGold.text = goldPerSec.ToString();
        statsElixir.text = elixirPerSec.ToString();

        ResourceIncrease();
    }

    public void ResourceIncrease(){
        timer += Time.deltaTime; /// Increase timer

        if(timer >= 1.0f ){

            currentGold += goldPerSec; /// Increases gold
            currentElixir += elixirPerSec; /// Increases elixir

            timer = 0.0f; /// Resets timer
        }
    }

    public void ResourceChange(GameObject objectToDelete)
    {
        if (objectToDelete.name.Contains("Gold Hut"))
        {
            goldPerSec -= 5; /// reduce gold income if name includes gold hut
        }
        else if (objectToDelete.name.Contains("Wizard Hut"))
        {
            elixirPerSec -= 5; /// reduce gold income if name includes wiazrd hut
        }
    }

    private static Dictionary<string, BuildingInfo> buildingInfoDictionary;

    void Start()
    {
        // Initialises the building info dict
        InitialiseBuildingInfoDict();

        currentGold = 1000;
        currentElixir = 1000;

        goldPerSec = 0;
        elixirPerSec = 0;
        
    }

    void InitialiseBuildingInfoDict()
    {
        buildingInfoDictionary = new Dictionary<string, BuildingInfo> /// defines dictionary
        {
            {"Gold Hut", new BuildingInfo(50, 100, "gold", 5, 0)},
            {"Wizard Hut", new BuildingInfo(100, 50, "well", 0, 5)},
            {"Mage Tower", new BuildingInfo(100, 200, "mage", 0, 0)},
            {"Cannon", new BuildingInfo(200, 100, "cannon", 0, 0)},
            {"Wall", new BuildingInfo(50, 50, "wall", 0, 0)}
        };
    }

    /// class used to store values and act as a dictionary
    public class BuildingInfo
    {
        public int GoldUsage { get; }
        public int ElixirUsage { get; }
        public string UpgradeType { get; }
        public int B_GoldPerSec { get; }
        public int B_ElixirPerSec { get; }

        ///initialising of class and its data
        public BuildingInfo(int goldUsage, int elixirUsage, string upgradeType, int goldPerSec, int elixirPerSec)
        {
            GoldUsage = goldUsage;
            ElixirUsage = elixirUsage;
            UpgradeType = upgradeType;
            B_GoldPerSec = goldPerSec;
            B_ElixirPerSec = elixirPerSec;
        }
    }

    public static bool ResourceUsage(GameObject objectToPlace)
    {
        string key = objectToPlace.name.Replace("(Clone)", "").Trim(); /// removes “clone” if it is included in the object's name
        if (key.Contains("Wall")) 
        {
            key = "Wall"; /// if key contains wall, change key to wall
        }

        /// get values from dictionary
        int goldUsage = buildingInfoDictionary[key].GoldUsage;
        int elixirUsage = buildingInfoDictionary[key].ElixirUsage;
        string upgradeType = buildingInfoDictionary[key].UpgradeType;
        int b_goldPerSec = buildingInfoDictionary[key].B_GoldPerSec;
        int b_elixirPerSec = buildingInfoDictionary[key].B_ElixirPerSec;

        if (((currentGold - goldUsage) < 0)||((currentElixir - elixirUsage) < 0)) /// Testing if the player has enough resources
        {
            Debug.Log("Not enough resources");
            return false;
        }
        else
        {
            /// resources are deducted, income increased and upgrade requirement updated
            currentGold -= goldUsage;
            currentElixir -= elixirUsage;

            elixirPerSec += b_elixirPerSec;
            goldPerSec += b_goldPerSec;

            Upgrade.Built(upgradeType);
            return true;
        }
    }

}
