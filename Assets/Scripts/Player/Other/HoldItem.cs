using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldItem : MonoBehaviour
{
    [SerializeField] float grabDist;
    [SerializeField] float throwForce;

    Rigidbody playerRb;
    HoldableItem heldItem;

    private void Start()
    {
        playerRb = GetComponentInParent<Rigidbody>();
    }

    public void Grab()
    {
        if (!heldItem)
        {
            TryToGrab();
        }
        else
        {
            ThrowItem();
        }
    }

    /// <summary>
    /// Grab an item from using a raycast
    /// </summary>
    void TryToGrab()
    {
        RaycastHit hit;
        Physics.Raycast(new Ray(transform.position, transform.forward), out hit, grabDist);
        if (hit.collider && hit.collider.gameObject.tag == "Holdable")
        {
            hit.collider.transform.parent = transform;
            heldItem = hit.collider.GetComponent<HoldableItem>();
            heldItem.Setup();
            heldItem.transform.localPosition = heldItem.cameraOffset;
            heldItem.transform.rotation = transform.rotation;
        }
    }

    void ThrowItem()
    {
        heldItem.transform.parent = null;
        heldItem.OnDrop();
        heldItem.GetThrown(playerRb.velocity, throwForce * transform.forward);
        heldItem = null;
    }

    public void UseItem()
    {
        if (heldItem)
        {
            heldItem.Use();
        }
    }
}
