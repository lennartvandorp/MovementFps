using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputSetup
{
    public KeyCode forward = KeyCode.W;
    public KeyCode back = KeyCode.S;
    public KeyCode right = KeyCode.D;
    public KeyCode left = KeyCode.A;
    public KeyCode jump = KeyCode.Space;
    public KeyCode useHandHeld = KeyCode.Mouse0;
    public KeyCode grab = KeyCode.Mouse1;
}
