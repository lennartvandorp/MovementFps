using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : Weapon
{
    [SerializeField] float shootForce;

    public override void Use()
    {
        RaycastHit hit;
        Rigidbody otherRb = null;
        Physics.Raycast(new Ray(cameraTrans.position, cameraTrans.forward), out hit);
        if (hit.collider) { otherRb = hit.collider.GetComponent<Rigidbody>(); }
        if (otherRb)
        {
            otherRb.AddForceAtPosition(cameraTrans.forward * shootForce, hit.point);
        }

        base.Use();
    }

}
