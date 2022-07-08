using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Jumping))]
[RequireComponent(typeof(PlayerSenses))]
[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] InputSetup setup;

    Movement movement;
    PlayerSenses senses;
    Jumping jumping;

    private void Start()
    {
        movement = GetComponent<Movement>();
        senses = GetComponent<PlayerSenses>();
        jumping = GetComponent<Jumping>();
    }

    bool wasJumping;
    public void FixedUpdate()
    {
        movement.OnStartOfFrame();
        if (Input.GetKey(setup.forward))
        {
            movement.ForwardInput();
        }
        if (Input.GetKey(setup.left))
        {
            movement.LeftInput();
        }
        if (Input.GetKey(setup.right))
        {
            movement.RightInput();
        }
        if (Input.GetKey(setup.back))
        {
            movement.BackInput();
        }
        if (Input.GetKey(setup.jump))
        {
            movement.Jump();
        }
        if(/*!wasJumping &&*/ senses.HitGround() && Input.GetKey(setup.jump))
        {
            wasJumping = true;
            jumping.OnJump(setup.jump);
        }
        if (!Input.GetKey(setup.jump))
        {
            wasJumping = false;
        }
        

        movement.OnEndOfFrame();
    }
}
