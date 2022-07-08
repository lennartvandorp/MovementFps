using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Jumping : MonoBehaviour
{
    [Header("Jump Values")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float extendedJumpForce;
    [SerializeField] private float extendedJumpDuration;
    [SerializeField] private float fallExtraGravity;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void OnJump(KeyCode jumpKey)
    {
        StartCoroutine(Jump(jumpKey));
    }

    private void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.down * Time.deltaTime * fallExtraGravity;
        }
    }


    public IEnumerator Jump(KeyCode jumpKey)
    {
        float timeEllapsed = 0f;
        bool hasLetGo = false;
        //UpdateAirMoveSpeed();
        rb.velocity = rb.velocity + (new Vector3(0f, jumpSpeed, 0f));
        while (timeEllapsed < extendedJumpDuration && !hasLetGo)
        {
            if (Input.GetKey(jumpKey))
            {
                rb.AddForce(new Vector3(0f, extendedJumpForce * Time.deltaTime, 0f));
                timeEllapsed += Time.deltaTime;
                yield return null;

            }
            else { hasLetGo = true; }
        }

    }
}
