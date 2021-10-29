using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public bool canFollow;

    void Update()
    {
        if (canFollow && gameObject.tag == "MainCamera")
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z); // Camera follows the player with specified offset position
        }
    }

    public void lockFollow() { canFollow = false; }
    public void unlockFollow() { canFollow = true; }
}
