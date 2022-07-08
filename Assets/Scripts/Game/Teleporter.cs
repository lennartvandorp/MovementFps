using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    Vector3 respawnPos;
    // Start is called before the first frame update
    void Start()
    {
        respawnPos = Movement.Instance.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Movement.Instance.transform.position.y < 0f) { Movement.Instance.transform.position = respawnPos; }
    }
}
