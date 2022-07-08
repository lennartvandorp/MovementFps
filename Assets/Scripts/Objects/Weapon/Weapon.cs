using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : HoldableItem
{
    Transform cameraTrans;

    public override void Setup()
    {
        cameraTrans = transform.parent.transform;
        base.Setup();
    }



    public virtual void Attack()
    {

    }

}
