using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class TroopsPossessing : MonoBehaviour
{
    public GameObject canvas;
    public GameObject crosshairCanvas;
    public GameObject builderCam;

    private GameObject currentTroop;

    public BuildingSystem buildingSystem;

    public void Update()
    {
        if (currentTroop != null)
        {
            EscKeyPress();
        }
    }

    public void EscKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPossession();
        }
    }

    public void ObjectAndScriptOffSwitch()
    {
        builderCam.SetActive(false);
        canvas.SetActive(false);
        crosshairCanvas.SetActive(true);
        
        buildingSystem.enabled = false;
    }

    public void ObjectAndScriptOnSwitch()
    {
        builderCam.SetActive(true);
        canvas.SetActive(true);
        crosshairCanvas.SetActive(false);
        
        buildingSystem.enabled = true;
    }

    public void ActivateObjectScript(GameObject troop)
    {
        currentTroop = troop;

        Movement movement = currentTroop.GetComponentInChildren<Movement>();
        movement.enabled = true;
        movement.CustomStart();

        TroopAttack attackScript = currentTroop.GetComponentInChildren<TroopAttack>();
        attackScript.enabled = true;

        Transform cameraObject = currentTroop.transform.Find("Main Camera");
        cameraObject.gameObject.SetActive(true);

        Transform freeLookCam = cameraObject.Find("FreeLook Camera");
        BillboardScript[] billboardScripts = Resources.FindObjectsOfTypeAll<BillboardScript>();
        foreach(BillboardScript script in billboardScripts)
        {
            script.cam = freeLookCam;
        }

        BarbOrArchActivate();
    }

    public void DeactivateObjectScript()
    {
        if(currentTroop != null)
        {
            Movement movement = currentTroop.GetComponentInChildren<Movement>();
            movement.enabled = false;
            movement.rb.velocity = Vector3.zero;

            TroopAttack attackScript = currentTroop.GetComponentInChildren<TroopAttack>();
            attackScript.enabled = false;

            Transform cameraObject = currentTroop.transform.Find("Main Camera");
            cameraObject.gameObject.SetActive(false);

            BarbOrArchDeactivate();
        }

        GameObject builderCam = GameObject.Find("Builder cam");
        BillboardScript[] billboardScripts = Resources.FindObjectsOfTypeAll<BillboardScript>();
        foreach(BillboardScript script in billboardScripts)
        {
            script.cam = builderCam.transform;
        }
    }

    public void BarbOrArchActivate()
    {
        if (currentTroop.name.Contains("Archer"))
        {
            Cannon cannonScript = currentTroop.GetComponentInChildren<Cannon>();
            cannonScript.enabled = false;
        }
        else if (currentTroop.name.Contains("Barbarian"))
        {
            BarbarianAttack barbarianAttackScript = currentTroop.GetComponentInChildren<BarbarianAttack>();
            barbarianAttackScript.enabled = false;
        }
    }

    public void BarbOrArchDeactivate()
    {
        if (currentTroop.name.Contains("Archer"))
        {
            Cannon cannonScript = currentTroop.GetComponentInChildren<Cannon>();
            cannonScript.enabled = true;
        }
        else if (currentTroop.name.Contains("Barbarian"))
        {
            BarbarianAttack barbarianAttackScript = currentTroop.GetComponentInChildren<BarbarianAttack>();
            barbarianAttackScript.enabled = true;
        }
    }

    public void ExitPossession()
    {
        ObjectAndScriptOnSwitch();
        DeactivateObjectScript();
        currentTroop = null;
    }

    public void Possessing(GameObject troop)
    {
        ObjectAndScriptOffSwitch();
        ActivateObjectScript(troop);
    }
}
