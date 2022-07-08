using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicFOV : MonoBehaviour
{
    //References to objects
    [SerializeField]private Rigidbody subject;
    private Camera cam;

    [Header("Variables")]

    [SerializeField] private float maxFOVIncrease;
    [SerializeField] private float FOVMult;

    //Needed variables
    private float stationaryFOV;
    private float currentFOV;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        stationaryFOV = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        currentFOV = stationaryFOV + subject.velocity.magnitude * FOVMult;
        if (currentFOV > maxFOVIncrease + stationaryFOV) currentFOV = maxFOVIncrease + stationaryFOV;
        cam.fieldOfView = currentFOV;
    }
}
