using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;

    public bool inBattle;

    [Header("Gold Hut")]
    public GameObject gold;

    [Header("Wizard Hut")]
    public GameObject elixir;

    [Header("Mage Tower")]
    public GameObject mage;

    [Header("Cannon")]
    public GameObject cannon;

    [Header("Walls")]
    public GameObject regularWall;
    public GameObject cornerWall;
    public GameObject tWall;
    public GameObject plusWall;

    private PlaceableObject objectToPlace;

    #region Unity methods

    public bool allPlaced = true;

    private void Awake()
    {
        current = this; /// Initialises the current field with this script
        grid = gridLayout.gameObject.GetComponent<Grid>(); /// Grabs grid component from gridLayout
    }

    public void GoldHutFunction(){
        /// test if building requirements has already been reached
        if(Upgrade.goldHutRequirement == Upgrade.goldHutCurrent)
        {
            Debug.Log("Upgrade Town Hall first");
            return;
        }
        allPlacedCheck(); /// call function to check if everything has been placed or not
        if (allPlaced == true){
            InitializeWithObject(gold); /// spawn building if all has been placed
        } 
    }

    public void WizardHutFunction(){
        if(Upgrade.wellRequirement == Upgrade.wellCurrent)
        {
            Debug.Log("Upgrade Town Hall first");
            return;
        }
        allPlacedCheck();
        if (allPlaced == true){
            InitializeWithObject(elixir); 
        }   
    }

    public void MageTowerFunction(){
        if(Upgrade.mageRequirement == Upgrade.mageCurrent)
        {
            Debug.Log("Upgrade Town Hall first");
            return;
        }
        allPlacedCheck();
        if (allPlaced == true){
            InitializeWithObject(mage); 
        }   
    }

    public void CannonFunction(){
        if(Upgrade.cannonRequirement == Upgrade.cannonCurrent)
        {
            Debug.Log("Upgrade Town Hall first");
            return;
        }
        allPlacedCheck();
        if (allPlaced == true){
            InitializeWithObject(cannon); 
        }
    }

    public void RegularWallFunc(){
        if(Upgrade.wallRequirement == Upgrade.wallCurrent)
        {
            Debug.Log("Upgrade Town Hall first");
            return;
        }
        allPlacedCheck();
        if (allPlaced == true){
            InitializeWithObject(regularWall); 
        }
    }
    public void CornerWallFunc(){
        if(Upgrade.wallRequirement == Upgrade.wallCurrent)
        {
            Debug.Log("Upgrade Town Hall first");
            return;
        }
        allPlacedCheck();
        if (allPlaced == true){
            InitializeWithObject(cornerWall); 
        }
    }
    public void TWallFunc(){
        if(Upgrade.wallRequirement == Upgrade.wallCurrent)
        {
            Debug.Log("Upgrade Town Hall first");
            return;
        }
        allPlacedCheck();
        if (allPlaced == true){
            InitializeWithObject(tWall); 
        }
    }
    public void PlusWallFunc(){
        if(Upgrade.wallRequirement == Upgrade.wallCurrent)
        {
            Debug.Log("Upgrade Town Hall first");
            return;
        }
        allPlacedCheck();
        if (allPlaced == true){
            InitializeWithObject(plusWall); 
        }
    }

    public void allPlacedCheck()
    {
        allPlaced = true;

        if(inBattle == true)
        {
            Debug.Log("Cant build when in battle");
            allPlaced = false;
            return;
        }

        GameObject[] allGameObjects = FindObjectsOfType<GameObject>(); /// gets all game objects
        foreach (GameObject obj in allGameObjects) /// Loops through all game objects
        {
            ObjectDrag drag = obj.GetComponent<ObjectDrag>(); /// Grabs the component if available
            if (drag != null)
            {
                allPlaced = false; /// Changes value if component does not exist
            }
        }

        if (allPlaced != true){
            Debug.Log("Previous object not placed"); /// Notifies user that object hasn't been placed
        }
    }

    private void Update()
    {
        if (inBattle == true && objectToPlace != null)
        {
            Destroy(objectToPlace.gameObject);
            objectToPlace = null;
        }
        if (Input.GetKeyDown(KeyCode.Return)) /// if enter is pressed
        {
            if(objectToPlace == null)
            {
                return;
            }

            if (CanBePlaced(objectToPlace)) /// Input validation
            {
                objectToPlace.Place(); /// place the object using the function
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition()); /// sets initial position
                TakeArea(start, objectToPlace.Size); /// creates white tiles
                objectToPlace = null;
            }
            else
            {
                Destroy(objectToPlace.gameObject); ///if cannot place, destroy object
            }
        }

        else if (Input.GetKeyDown(KeyCode.Escape)) /// If the user doesn't want to place the object
        {
            if (objectToPlace != null)
            {
                Destroy(objectToPlace.gameObject);
            }
        }
        else if(Input.GetKeyDown(KeyCode.R)) ///For rotation of 90 degrees if r is pressed
        {
            if (objectToPlace != null) ///Checks if there is an unplaced object
            {
                PlacedObject placed = objectToPlace.GetComponent<PlacedObject>(); /// Gets component
                if (placed == false)
                {
                    objectToPlace.Rotate(); /// Rotates the object by calling function 
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))/// Used to select objects
        {
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace)) /// Deletes the selected object
        {
            DeletableObject[] deletableObjects = FindObjectsOfType<DeletableObject>(); /// array of object with the deletable object script

            foreach (DeletableObject obj in deletableObjects) /// Loop used to delete each objects
            {
                RemoveObject(obj.gameObject); /// calls function dedicated to removing buildings
            }
        }
        
    }

    #endregion

    #region Utils

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); /// Raycast is shot out
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }


    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    #endregion

    #region Building Placement

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();

    }

    public bool CanBePlaced(PlaceableObject placeableObject)
    {
        /// Create a variable to represent the area where the object will be placed
        BoundsInt area = new BoundsInt();
        /// Set the position of the area to the grid cell
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        /// Set the size of the area to the size of the placeable object
        area.size = placeableObject.Size;
        /// Padding around the placeable object by increasing by 1 for each dimension 
        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);

        /// Get an array of tiles within the area
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        /// Loop through the array
        foreach (var b in baseArray)
        {
            /// Check if the tile is a white tile
            if (b == whiteTile)
            {
                /// If a white tile is found, the object cannot be placed in this location
                return false;
            }
        }

        if (!ResourcesScript.ResourceUsage(objectToPlace.gameObject)){
            return false;
        }

        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        /// create a white tile with the given inputs
        MainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x+ size.x, start.y+size.y);
    }

    

    #endregion


    #region Demolishing

    public void RemoveObject(GameObject obj)
    {
        Upgrade UpgradeInstance = FindObjectOfType<Upgrade>(); /// grabs instance of upgrade
        UpgradeInstance.RequirmentChange(obj); /// use current object as parameter in function to change values

        ResourcesScript resourcesScriptInstance = FindObjectOfType<ResourcesScript>(); /// grabs instance of resource
        resourcesScriptInstance.ResourceChange(obj); /// use current object as parameter in function to change values

        Vector3 objectPosition = obj.transform.position; // Get the position of your game object
        Vector3 objectSize = new Vector3(1.0f, 1.0f, 0.0f);
        RemoveArea(objectPosition, objectSize); /// remove white tile under object
        Destroy(obj); // Destroy the game object, not the component
    }

    public void RemoveArea(Vector3 position, Vector3 size)
    {
        // Calculate the cell positions covered by the game object
        Vector3Int cellPosition1 = MainTilemap.WorldToCell(position);
        MainTilemap.SetTile(cellPosition1, null); /// set space to null tiles
    }

    public void Selected()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); /// Shoots raycast
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            PlaceableObject selectable = hit.collider.GetComponent<PlaceableObject>();
            /// Grabs necessary component

            if (hit.transform.gameObject.name.Contains("Town Hall")) /// if object is the townhall
                {
                    THCanvasChange town_hall_script= hit.transform.gameObject.GetComponent<THCanvasChange>();
                    town_hall_script.TH_Change_canvas(); /// activate town hall change canvas

                }
            
            if (hit.transform.gameObject.name.Contains("Barbarian") || hit.transform.gameObject.name.Contains("Archer") )
            {
                TroopsPossessing troopsPossessingInstance = FindObjectOfType<TroopsPossessing>();
                troopsPossessingInstance.Possessing(hit.transform.gameObject);
            }
            
            if (selectable != null) /// Runs only if it has the selectable script
            {
                GameObject hitObject = hit.transform.gameObject;

                DeletableObject deletable = hitObject.GetComponent<DeletableObject>();
                PlacedObject placed = hitObject.GetComponent<PlacedObject>();
                /// Grabs components that will be used for selection statements

                if (deletable != null && placed != null) /// Effects when object has been selected before
                {
                    Destroy(hitObject.GetComponent<DeletableObject>()); /// removes deletable object component

                    /// Turns the prefab green by:
                    Renderer[] childRenderers = hitObject.GetComponentsInChildren<Renderer>(); /// gets all child objects and store in array
                
                    foreach (Renderer childRenderer in childRenderers) ///loops through each object
                    {    
                    Material material = childRenderer.material; /// grabs the material of the object
                    material.color = Color.white; /// turns material colour to white
                    }
                }
                else if (deletable == null && placed != null)
                {
                    hitObject.AddComponent<DeletableObject>(); /// add deletable object component
                
                    /// turn object to black
                    Renderer[] childRenderers = hitObject.GetComponentsInChildren<Renderer>();
                
                    foreach (Renderer childRenderer in childRenderers)
                    {
                    Material material = childRenderer.material;
                    material.color = Color.black;
                    }
                }
            }
        } 
    }

    #endregion





}
