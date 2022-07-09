using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableItem : MonoBehaviour
{
    [SerializeField] public bool equipOnGrab;
    [SerializeField] public Vector3 cameraOffset;
    Rigidbody rb;
    Collider collider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    /// <summary>
    /// This is run when the object is picked up
    /// </summary>
    public virtual void Setup()
    {
        rb.isKinematic = true;
        collider.enabled = false;
    }

    public virtual void OnDrop()
    {
        rb.isKinematic = false;
        collider.enabled = true;
    }

    public virtual void GetThrown(Vector3 inheritedVel ,Vector3 force)
    {
        rb.velocity = inheritedVel;
        rb.AddForce(force);
    }

    public virtual void Use()
    {

    }
}
