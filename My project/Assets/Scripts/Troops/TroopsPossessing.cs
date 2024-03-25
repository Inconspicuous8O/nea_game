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
        if (currentTroop != null) /// checks that a current troop is being used 
        {
            EscKeyPress();
        }
    }

    public void EscKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  /// checks if user presses escape key
        {
            ExitPossession(); /// exit troop pov/control
        }
    }

    public void ObjectAndScriptOffSwitch()
    {
        builderCam.SetActive(false); /// turns off builder camera
        canvas.SetActive(false); /// turns off canvas system
        crosshairCanvas.SetActive(true); /// turns on canvas that acts as a crosshair
        
        buildingSystem.enabled = false; /// turns off building system
    }

    public void ObjectAndScriptOnSwitch()
    {
        builderCam.SetActive(true); /// turns on builder camera
        canvas.SetActive(true); /// turns on canvas system
        crosshairCanvas.SetActive(false); /// turns off canvas that acts as a crosshair
        
        buildingSystem.enabled = true; /// turns on building system
    }

    public void ActivateObjectScript(GameObject troop)
    {
        currentTroop = troop;

        /// grab movement script
        Movement movement = currentTroop.GetComponentInChildren<Movement>();
        movement.enabled = true; /// enable the movement script
        movement.CustomStart(); /// call CustomStart function from movement component

        /// grab movement script
        TroopAttack attackScript = currentTroop.GetComponentInChildren<TroopAttack>();
        attackScript.enabled = true; /// enable the attack script

        /// finds the camera object in the troop
        Transform cameraObject = currentTroop.transform.Find("Main Camera");
        /// activates the camera
        cameraObject.gameObject.SetActive(true);

        /// finds the free look camera 
        Transform freeLookCam = cameraObject.Find("FreeLook Camera");
        /// find all billboard scripts
        BillboardScript[] billboardScripts = Resources.FindObjectsOfTypeAll<BillboardScript>();
        foreach(BillboardScript script in billboardScripts) ///  iterates through all the scripts
        {
            script.cam = freeLookCam; /// sets camera to the freelook camera
        }

        BarbOrArchActivate(); /// calls script to take over troop
    }

    public void DeactivateObjectScript()
    {
        if(currentTroop != null) /// if troop still exists
        {
            Movement movement = currentTroop.GetComponentInChildren<Movement>(); /// grab movement script and deactivate it
            movement.enabled = false; 
            movement.rb.velocity = Vector3.zero; /// reset velocity

            /// grab attack script and deactivate it
            TroopAttack attackScript = currentTroop.GetComponentInChildren<TroopAttack>();
            attackScript.enabled = false;

            /// grab camera on troop and deactivate it
            Transform cameraObject = currentTroop.transform.Find("Main Camera");
            cameraObject.gameObject.SetActive(false);

            BarbOrArchDeactivate(); /// calls script to deactivate troop
        }

        /// find builder camera, grab all billboard script and set all cameras to builder camera
        GameObject builderCam = GameObject.Find("Builder cam");
        BillboardScript[] billboardScripts = Resources.FindObjectsOfTypeAll<BillboardScript>();
        foreach(BillboardScript script in billboardScripts)
        {
            script.cam = builderCam.transform;
        }
    }

    public void BarbOrArchActivate()
    {
        if (currentTroop.name.Contains("Archer")) /// if troop name contains archer
        {
            /// grab idle attack script and deactivate it
            Cannon cannonScript = currentTroop.GetComponentInChildren<Cannon>();
            cannonScript.enabled = false;
        }
        else if (currentTroop.name.Contains("Barbarian")) /// if troop name contains barbarian
        {
            /// grab idle attack script and deactivate it
            BarbarianAttack barbarianAttackScript = currentTroop.GetComponentInChildren<BarbarianAttack>();
            barbarianAttackScript.enabled = false;
        }
    }

    public void BarbOrArchDeactivate()
    {
        if (currentTroop.name.Contains("Archer")) /// if troop name contains archer
        {
            /// grab idle attack script and activate it
            Cannon cannonScript = currentTroop.GetComponentInChildren<Cannon>();
            cannonScript.enabled = true;
        }
        else if (currentTroop.name.Contains("Barbarian")) /// if troop name contains barbarian
        {
            /// grab idle attack script and activate it
            BarbarianAttack barbarianAttackScript = currentTroop.GetComponentInChildren<BarbarianAttack>();
            barbarianAttackScript.enabled = true;
        }
    }

    public void ExitPossession()
    {
        /// call required scripts
        ObjectAndScriptOnSwitch();
        DeactivateObjectScript();
        currentTroop = null; /// set current troop to null as there is no longer a troop to control
    }

    public void Possessing(GameObject troop)
    {
        /// call required scripts
        ObjectAndScriptOffSwitch();
        ActivateObjectScript(troop); 
    }
}
