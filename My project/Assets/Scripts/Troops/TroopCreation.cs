using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopCreation : MonoBehaviour
{
    public GameObject barbarian;
    public GameObject archer;

    public void CreateBarb()
    {
        if (RecruitmentCost()) /// if function returns true
        {
            /// create barbarian
            GameObject newBarb = Instantiate(barbarian, Vector3.zero, Quaternion.identity);
        }
    }

    public void CreateArch()
    {
        if(RecruitmentCost()) /// if function returns true
        {
            /// create archer
            GameObject newArch = Instantiate(archer, Vector3.zero, Quaternion.identity);
        }
    }

    public bool RecruitmentCost()
    {
        if (ResourcesScript.currentGold - 50 < 0)
        {
            Debug.Log("Not enough to train troop"); /// if user doesn't have enough resources
            return false;
        }
        else
        {
            /// alter resources and income
            ResourcesScript.currentGold -= 50;
            ResourcesScript.goldPerSec -= 5;
            return true;
        }
    }

}
