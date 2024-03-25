using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        /// calculation for distance between the centre of an object and the position of the mouse
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();

    }

    private void OnMouseDrag()
    {
        /// gets world position of mouse and add offset
        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;
        /// snaps object to position
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
    }
}
