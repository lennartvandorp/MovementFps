using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AgentSenses))]
public class MovementRework : Movement
{
    Vector3 velocity;
    Vector3 direction;
    AgentSenses senses;

    [SerializeField] float acceleration;
    [SerializeField] float airAcc;
    [SerializeField] float movingFriction;
    [SerializeField] float stoppingFriction;


    private void Start()
    {
        senses = GetComponent<AgentSenses>();
    }

    public override void OnStartOfFrame()
    {
        direction = Vector3.zero;
    }

    public override void ForwardInput()
    {
        direction += Vector3.forward;
        base.ForwardInput();
    }
    public override void LeftInput()
    {
        direction -= Vector3.right;
        base.LeftInput();
    }
    public override void RightInput()
    {
        direction += Vector3.right;
        base.RightInput();
    }
    public override void BackInput()
    {
        direction -= Vector3.forward;
        base.BackInput();
    }


    float friction;

    public override void OnEndOfFrame()
    {
        direction = direction.normalized;
        velocity = velocity.normalized * new Vector3(Rb.velocity.x, 0f, Rb.velocity.z).magnitude;//So the velocity is influenced by hitting objects. 

        if (senses.HitGround())
        {
            if (direction.magnitude == 0f)
            {
                friction = stoppingFriction;
            }
            else
            {
                friction = movingFriction;
            }

            Rb.velocity *= friction;
            Rb.velocity += ((transform.forward * direction.z) + (transform.right * direction.x)) * acceleration;
        }
        else
        {
            Vector3 horVel = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);
            Vector3 worldDir = (transform.forward * direction.z) + (transform.right * direction.x);
            if (Vector3.Dot(horVel.normalized, worldDir) > 0)
            {
                worldDir = worldDir - Vector3.Dot(horVel.normalized, worldDir) * horVel.normalized;
            }
            Rb.velocity += worldDir * airAcc;

        }

        base.OnEndOfFrame();
    }

}
