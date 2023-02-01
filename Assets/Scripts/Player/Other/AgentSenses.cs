using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AgentSenses : MonoBehaviour
{

    [SerializeField] Transform feet;
    Transform[] allFeet;

    private void Start()
    {
        if (feet == null)
        {
            Debug.LogError("Feet needs to be declared for the jumping to work. It's children will be used");
        }
        else
        {
            allFeet = feet.GetComponentsInChildren<Transform>();
        }

    }

    /// <summary>
    /// Returns true if the current character's feet are touching the ground. 
    /// </summary>
    /// <returns></returns>
    public bool HitGround()
    {
        float rayLength = .1f;//Only modify this or the feet

        bool hitGround = false;
        RaycastHit hit;
        for (int i = 0; i < allFeet.Length; i++)
        {
            Ray ray = new Ray(allFeet[i].position, new Vector3(0f, -rayLength, 0f));

            Physics.Raycast(ray, out hit, rayLength);
            Debug.DrawRay(ray.origin, ray.direction, Color.green);

            float dot = Vector3.Dot(transform.up, hit.normal);
            if (hit.collider != null && dot > .7f)
            {
                groundCollider = hit.collider;
                hitGround = true;
                return hitGround;
            }
        }
        return hitGround;
    }

    /// <summary>
    /// Hitground is run to update this
    /// </summary>
    /// <returns></returns>
    Collider groundCollider;
    public Collider GroundCollider { get { return groundCollider; } }
}
