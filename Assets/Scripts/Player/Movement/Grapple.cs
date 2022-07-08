using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] private float grappleStrength;
    [SerializeField] private float lurchStrength;
    [SerializeField] private float upLurchStrength;
    [SerializeField] private float hookSpeed;
    [SerializeField] private float hookDistance;
    [SerializeField] private float disconnectNormal;

    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject grappleLine;
    private LineRenderer line;
    private GameObject currentHookObject;
    private Hook currentHook;
    private Rigidbody rb;
    bool grappleAttached = false;

    private void Start()
    {
        line = grappleLine.GetComponent<LineRenderer>();
        grappleLine.active = false;
        rb = GetComponentInParent<Rigidbody>();
        if (hookSpeed == 0f)
        {
            hookSpeed = 1f;
        }
    }

    bool hasLurched;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!grappleAttached)
            {
                ShootHook();
            }
            else { RemoveHook(); }
        }
        if (currentHookObject)
        {
            if (Vector3.Dot(transform.forward, (currentHookObject.transform.position - transform.position).normalized) < disconnectNormal)
            {
                RemoveHook();
            }
        }

        if (currentHookObject)
        {
            grappleLine.active = true;
            line.SetPosition(1, currentHookObject.transform.position);


            if (grappleAttached)
            {
                if (!hasLurched)
                {
                    hasLurched = true;
                    rb.AddForce((currentHookObject.transform.position - body.transform.position).normalized * lurchStrength * 10f);
                    rb.AddForce(rb.transform.up * upLurchStrength * 10);
                }
                rb.AddForce((currentHookObject.transform.position - body.transform.position).normalized * 100f * Time.deltaTime * grappleStrength);
                //PlayerStrafeMovement.main.UpdateAirMoveSpeed();
            }

        }
        line.SetPosition(0, body.transform.position);

    }

    /// <summary>
    /// Shoots a new hook gameobject 
    /// </summary>
    void ShootHook()
    {
        grappleAttached = false;
        Destroy(currentHookObject);
        GameObject newHook = Instantiate(hookPrefab, transform.position, transform.rotation);
        currentHookObject = newHook;

        currentHook = currentHookObject.GetComponent<Hook>();
        currentHook.Setup(this, hookDistance / hookSpeed);
        currentHook.onTimeOut += OnHookDelete;

        Rigidbody hookRb = currentHook.GetComponent<Rigidbody>();
        hookRb.velocity = transform.forward * hookSpeed;
    }

    /// <summary>
    /// resets the hook to normal
    /// </summary>
    void RemoveHook()
    {
        grappleAttached = false;
        //PlayerStrafeMovement.main.ChangePlayerInput(true);
        Destroy(currentHookObject);
        currentHookObject = null;
        grappleLine.active = false;
        hasLurched = false;
    }

    void OnHookDelete()
    {
        grappleLine.active = false;
    }

    public void GrappleHit()
    {
        grappleAttached = true;
    }
}
