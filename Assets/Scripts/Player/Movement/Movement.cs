using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;

    static Movement instance;

    static public Movement Instance
    {
        get
        {
            if (!instance)
            {
                Debug.LogError("There is no Movement instance, but it's still being called");
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public Rigidbody Rb
    {
        get
        {
            if (!rb)
            {
                rb = GetComponent<Rigidbody>();
            }
            return rb;
        }
    }

    public virtual void OnStartOfFrame() { }

    public virtual void ForwardInput()
    {
    }
    public virtual void LeftInput()
    {
    }

    public virtual void RightInput()
    {
    }

    public virtual void BackInput()
    {
    }

    public virtual void OnEndOfFrame()
    {

    }
    public virtual void Jump()
    {
    }
}

