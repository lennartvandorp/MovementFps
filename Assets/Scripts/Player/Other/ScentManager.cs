using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScentManager : MonoBehaviour
{
    [SerializeField] float scentSurvivalTime;
    [SerializeField] float scentPlacementDistance;
    [SerializeField] GameObject scent;

    Rigidbody rb;

    Vector3 lastPlacedPos;
    // Start is called before the first frame update
    void Start()
    {
        lastPlacedPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((lastPlacedPos - transform.position).magnitude > scentPlacementDistance)
        {
            placeNew();
        }
    }

    void placeNew()
    {
        GameObject newScent = Instantiate(scent);

        newScent.transform.position = transform.position;
        newScent.transform.LookAt(newScent.transform.position + rb.velocity);
        newScent.tag = "Scent";
        StartCoroutine(DestroyInTime(newScent));
        lastPlacedPos = transform.position;

    }

    IEnumerator DestroyInTime(GameObject toDestroy)
    {
        yield return new WaitForSeconds(scentSurvivalTime);
        Destroy(toDestroy);
    }
}
