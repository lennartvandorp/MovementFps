using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : Weapon
{
    [SerializeField] float shootForce;
    [SerializeField] GameObject impactParticles;
    private Animator animator;

    public override void Setup()
    {
        animator = GetComponent<Animator>();
        base.Setup();
    }

    public override void Use()
    {
        DoShootAnimation();

        RaycastHit hit;
        Rigidbody otherRb = null;
        Physics.Raycast(new Ray(cameraTrans.position, cameraTrans.forward), out hit);
        if (hit.collider)
        {
            otherRb = hit.collider.GetComponent<Rigidbody>();
            GameObject newParticles = Instantiate(impactParticles);
            newParticles.transform.position = hit.point;
        }
        if (otherRb)
        {
            otherRb.AddForceAtPosition(cameraTrans.forward * shootForce, hit.point);
        }

        base.Use();
    }

    void DoShootAnimation()
    {
        if (animator)
        {
            animator.SetTrigger("RevolverShootTrigger");
        }
        else { Debug.LogError("The Revolver should have an animator"); }
    }

}
