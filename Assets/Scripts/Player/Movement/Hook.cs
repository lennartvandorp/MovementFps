using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] GameObject hitParticles;
    Rigidbody rb;

    public event Action onTimeOut;

    Grapple originScript;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer != 3 )
        {
            Instantiate(hitParticles, collision.contacts[0].point, transform.rotation);
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            originScript.GrappleHit();
        }
    }
    public IEnumerator DestroyInTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (rb.velocity != Vector3.zero)
        {
            onTimeOut();
            Destroy(gameObject);

        }
    }
    public void Setup(Grapple origin, float destroyTime)
    {
        originScript = origin;
        onTimeOut-= onTimeOut;
        StartCoroutine(DestroyInTime(destroyTime));

    }
}
