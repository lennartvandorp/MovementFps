using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Jumping))]
[RequireComponent(typeof(AgentSenses))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] bool holdJumpOnLand;

    AgentGroundMovement movement;
    AgentSenses senses;
    Jumping jumping;
    HoldItem holdItem;
    

    private void Start()
    {
        movement = GetComponent<AgentGroundMovement>();
        senses = GetComponent<AgentSenses>();
        jumping = GetComponent<Jumping>();
        holdItem = GetComponentInChildren<HoldItem>();
    }

    bool wasJumping;
    bool wasUsing;
    bool wasGrabbing;
    public void FixedUpdate()
    {

        Vector3 direction = Vector3.zero;
        //movement.OnStartOfFrame();
        if (Input.GetKey(GameManager.Instance.setup.forward))
        {
            //movement.ForwardInput();
            direction += transform.forward;
        }
        if (Input.GetKey(GameManager.Instance.setup.left))
        {
            //movement.LeftInput();
            direction -= transform.right;
        }
        if (Input.GetKey(GameManager.Instance.setup.right))
        {
            //movement.RightInput();
            direction += transform.right;
        }
        if (Input.GetKey(GameManager.Instance.setup.back))
        {
            //movement.BackInput();
            direction -= transform.forward;
        }
        if (Input.GetKey(GameManager.Instance.setup.jump))
        {
            //movement.Jump();
        }
        if((!wasJumping || holdJumpOnLand) && senses.HitGround() && Input.GetKey(GameManager.Instance.setup.jump))
        {
            wasJumping = true;
            jumping.OnJump(GameManager.Instance.setup.jump);
        }
        if (!Input.GetKey(GameManager.Instance.setup.jump))
        {
            wasJumping = false;
        }

        if (!wasUsing && Input.GetKey(GameManager.Instance.setup.useHandHeld))
        {
            wasUsing = true;
            holdItem.UseItem();
        }
        if (!Input.GetKey(GameManager.Instance.setup.useHandHeld))
        {
            wasUsing = false;
        }

        if (!wasGrabbing && Input.GetKey(GameManager.Instance.setup.grab))
        {
            wasGrabbing = true;
            holdItem.Grab();
        }
        if (!Input.GetKey(GameManager.Instance.setup.grab))
        {
            wasGrabbing = false;
        }

        movement.SetDirection(direction.normalized);


        //movement.OnEndOfFrame();
    }
}
