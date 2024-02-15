using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed { get; private set;}
    public Vector3Int Size { get; private set;}
    private Vector3[] Vertices;

    private void GetColliderVertexPositionLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        Vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        Vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(vertices[i]);
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }

        Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x), 
                            Math.Abs((vertices[0] - vertices[3]).y), 
                            1);
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0,90,0));
        Size = new Vector3Int(Size.y, Size.x,1);

        Vector3[] vertices = new Vector3[Vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Vertices[(i+1) % Vertices.Length];
        }

        Vertices = vertices;
    }

    public void Start()
    {
        GetColliderVertexPositionLocal();
        CalculateSizeInCells();
    }

    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        gameObject.AddComponent<PlacedObject>();
        Destroy(drag);
        
        //call any functions that are to do with when the building is being placed

    }

    public static bool ResourceUsage(GameObject objectToPlace){
        int goldUsage = 0;
        int elixirUsage = 0;

        if (objectToPlace.name.Contains("Gold Hut")){
            goldUsage = 50;
            elixirUsage = 100;
            ResourcesScript.goldPerSec += 5;
            Upgrade.Built("gold");
        }
        else if (objectToPlace.name.Contains("Wizard Hut")){
            goldUsage = 100;
            elixirUsage = 50;
            ResourcesScript.elixirPerSec += 5;
            Upgrade.Built("well");
        }
        else if (objectToPlace.name.Contains("Mage Tower")){
            goldUsage = 100;
            elixirUsage = 200;
            Upgrade.Built("mage");
        }
        else if (objectToPlace.name.Contains("Cannon")){
            goldUsage = 200;
            elixirUsage = 100;
            Upgrade.Built("cannon");
        }
        else if (objectToPlace.name.Contains("Wall")){
            goldUsage = 50;
            elixirUsage = 50;
            Upgrade.Built("wall");
        }



        if (((ResourcesScript.currentGold - goldUsage) < 0)||((ResourcesScript.currentElixir - elixirUsage) < 0)){
            Debug.Log("Not enough resources");
            return false;
        }
        else{
            ResourcesScript.currentGold -= goldUsage;
            ResourcesScript.currentElixir -= elixirUsage;
            return true;
        }
            
    }


    
}
