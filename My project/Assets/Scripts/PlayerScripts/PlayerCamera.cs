using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera_info")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public Transform combat;

    private void Update()
    {
    Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
    orientation.forward = viewDir.normalized;

    Vector3 combatDir = combat.position - new Vector3(transform.position.x, combat.position.y, transform.position.z);
    orientation.forward = combatDir.normalized;

    playerObj.forward = combatDir.normalized;
    }
}
