using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 respawnPos;
    // Start is called before the first frame update
    void Start()
    {
        respawnPos = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y < 0f) { player.position = respawnPos; }
    }
}
