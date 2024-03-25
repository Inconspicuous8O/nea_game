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
        BoxCollider b = gameObject.GetComponent<BoxCollider>(); /// Gets the box collider component
        Vertices = new Vector3[4]; /// Creates a new array to store the values   
        Vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        Vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length]; /// Creates a new array to store the values 

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(vertices[i]);
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }

        /// Calculation that takes adjacent vertices to find the size of the object
        Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x), 
                            Math.Abs((vertices[0] - vertices[3]).y), 
                            1);
    }

    public Vector3 GetStartPosition()
    {
        /// returns the first vertex
        return transform.TransformPoint(Vertices[0]); /// error here when spamming enter
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0,90,0)); /// rotate the coordinate by 90 degrees by y axis
        Size = new Vector3Int(Size.y, Size.x,1); /// alter the size as it has now changed

        Vector3[] vertices = new Vector3[Vertices.Length]; /// create a new array and alter the vertices
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Vertices[(i+1) % Vertices.Length];
        }

        Vertices = vertices; /// the new vertices replace the old ones
    }

    public void Start()
    {
        /// call required functions
        GetColliderVertexPositionLocal();
        CalculateSizeInCells();
    }

    public virtual void Place()
    {
        /// Gets ObjectDrag component and assign it to a local variable
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        /// Add a component that will later be used to identify if the object has been placed or not
        gameObject.AddComponent<PlacedObject>();
        Destroy(drag); /// Destroys drag component
    }
}
