using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : HoldableItem
{
    [HideInInspector] public Transform cameraTrans;

    public override void Setup()
    {
        cameraTrans = transform.parent.transform;
        base.Setup();
    }

}
