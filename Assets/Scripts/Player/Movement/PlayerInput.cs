using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Jumping))]
[RequireComponent(typeof(PlayerSenses))]
[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] bool holdJumpOnLand;

    Movement movement;
    PlayerSenses senses;
    Jumping jumping;
    HoldItem holdItem;
    

    private void Start()
    {
        movement = GetComponent<Movement>();
        senses = GetComponent<PlayerSenses>();
        jumping = GetComponent<Jumping>();
        holdItem = GetComponentInChildren<HoldItem>();
    }

    bool wasJumping;
    bool wasUsing;
    bool wasGrabbing;
    public void FixedUpdate()
    {
        movement.OnStartOfFrame();
        if (Input.GetKey(GameManager.Instance.setup.forward))
        {
            movement.ForwardInput();
        }
        if (Input.GetKey(GameManager.Instance.setup.left))
        {
            movement.LeftInput();
        }
        if (Input.GetKey(GameManager.Instance.setup.right))
        {
            movement.RightInput();
        }
        if (Input.GetKey(GameManager.Instance.setup.back))
        {
            movement.BackInput();
        }
        if (Input.GetKey(GameManager.Instance.setup.jump))
        {
            movement.Jump();
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
            //Todo add the logic for using a held item;
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


        movement.OnEndOfFrame();
    }
}
