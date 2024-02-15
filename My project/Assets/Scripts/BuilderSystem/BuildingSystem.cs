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
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();

    }

    public void GoldHutFunction(){
        if(Upgrade.goldHutRequirement == Upgrade.goldHutCurrent)
        {
            Debug.Log("Upgrade Town Hall first");
            return;
        }
        allPlacedCheck();
        if (allPlaced == true){
            InitializeWithObject(gold); 
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

    public void allPlacedCheck(){
        allPlaced = true;

        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allGameObjects){
        
        ObjectDrag drag = obj.GetComponent<ObjectDrag>();

        if (drag != null){
            allPlaced = false;
        }
        }

        if (allPlaced != true){
            Debug.Log("Previous object not placed");
        }

        if(inBattle == true)
        {
            Debug.Log("Cant build when in battle");
            allPlaced = false;
        }

    }

    private void Update()
    {

        /*if (!objectToPlace)
        {
            Debug.Log("IDK why this is triggering");
            return;
        }*/

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(objectToPlace == null)
            {
                return;
            }

            if (CanBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                TakeArea(start, objectToPlace.Size);
                objectToPlace = null;
            }
            else
            {
                Destroy(objectToPlace.gameObject);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (objectToPlace != null)
            {
                Destroy(objectToPlace.gameObject);
            }
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            if (objectToPlace != null)
            {
                PlacedObject placed = objectToPlace.GetComponent<PlacedObject>();
                if (placed == false)
                {
                    objectToPlace.Rotate();
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Selected();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            DeletableObject[] deletableObjects = FindObjectsOfType<DeletableObject>();

            foreach (DeletableObject obj in deletableObjects)
            {
                RemoveObject(obj.gameObject);
            }
        }
        
    }

    #endregion

    #region Utils

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;
        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
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
        MainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x+ size.x, start.y+size.y);
    }

    

    #endregion


    #region Demolishing

    public void RemoveObject(GameObject obj)
    {
        Upgrade UpgradeInstance = FindObjectOfType<Upgrade>();
        UpgradeInstance.RequirmentChange(obj);///.gameObject

        ResourcesScript resourcesScriptInstance = FindObjectOfType<ResourcesScript>();
        resourcesScriptInstance.ResourceChange(obj);


        Vector3 objectPosition = obj.transform.position; // Get the position of your game object
        Vector3 objectSize = new Vector3(1.0f, 1.0f, 0.0f);
        RemoveArea(objectPosition, objectSize);
        Destroy(obj); // Destroy the game object, not the component
    }

    public void RemoveArea(Vector3 position, Vector3 size)
    {
        // Calculate the cell positions covered by the game object
        Vector3Int cellPosition1 = MainTilemap.WorldToCell(position);
        MainTilemap.SetTile(cellPosition1, null);
    }

    public void Selected()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            PlaceableObject selectable = hit.collider.GetComponent<PlaceableObject>();
            
            if (hit.transform.gameObject.name.Contains("Town Hall"))
                {
                    THCanvasChange town_hall_script= hit.transform.gameObject.GetComponent<THCanvasChange>();
                    town_hall_script.TH_Change_canvas();

                }
            
            if (hit.transform.gameObject.name.Contains("Barbarian") || hit.transform.gameObject.name.Contains("Archer") )
            {
                TroopsPossessing troopsPossessingInstance = FindObjectOfType<TroopsPossessing>();
                troopsPossessingInstance.Possessing(hit.transform.gameObject);
            }
            
            if (selectable != null)
            {
                GameObject hitObject = hit.transform.gameObject;

                DeletableObject deletable = hitObject.GetComponent<DeletableObject>();
                PlacedObject placed = hitObject.GetComponent<PlacedObject>();

                if (deletable != null && placed != null)
                {
                    Destroy(hitObject.GetComponent<DeletableObject>());

                    Renderer[] childRenderers = hitObject.GetComponentsInChildren<Renderer>();
                
                    foreach (Renderer childRenderer in childRenderers)
                    {    
                    Material material = childRenderer.material;
                    material.color = Color.white;
                    }
                }
                else if (deletable == null && placed != null)
                {
                    hitObject.AddComponent<DeletableObject>();
                
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
