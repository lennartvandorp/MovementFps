using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerStrafeMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 wishDir;
    InputSetup inputSetup;

    [Header("Movement Values")]
    [SerializeField] float acceleration;
    [SerializeField] float groundFrictionmMult;
    [SerializeField] float stoppingFrictionMult;


    [Header("Jump Values")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float extendedJumpForce;
    [SerializeField] private float extendedJumpDuration;
    [SerializeField] private float airMovementModifier;


    [SerializeField] Transform feet;
    Transform[] allFeet;

    [Header("Walljump Values")]
    [SerializeField] private float wallJumpRayLength;
    [SerializeField] private Transform leftWallrun;
    [SerializeField] private Transform rightWallrun;
    [SerializeField] private float walljumpSpeed;
    [SerializeField] private float wallJumpTime;

    public static PlayerStrafeMovement main;

    private void Awake()
    {
        if (!main)
        {
            main = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputSetup = new InputSetup();
        if (feet == null)
        {
            Debug.LogError("Feet needs to be declared for the jumping to work. It's children will be used");
        }
        else
        {
            allFeet = feet.GetComponentsInChildren<Transform>();
        }

    }


    private float currentMaxAirVelocity;
    private bool playerCanInput = true;
    // Update is called once per frame
    void Update()
    {
        wishDir = Vector3.zero;//Resets the wished direction
        bool hitGround = HitGround();
        if (hitGround)
        {
            UpdateAirMoveSpeed();//Makes sure the max air move speed is always correct
        }

        bool moveInput = false;
        if (playerCanInput)
        {
            //handles the inputs
            if (Input.GetKey(inputSetup.forward))
            {
                wishDir += transform.forward;
                moveInput = true;
            }
            if (Input.GetKey(inputSetup.back))
            {
                wishDir += -transform.forward;
                moveInput = true;
            }
            if (Input.GetKey(inputSetup.right))
            {
                wishDir += transform.right;
                moveInput = true;
            }
            if (Input.GetKey(inputSetup.left))
            {
                wishDir -= transform.right;
                moveInput = true;
            }
            if (hitGround && Input.GetKeyDown(inputSetup.jump))
            {
                StartCoroutine(Jump());
            }
        }
        if (hitGround)
        {
            rb.AddForce(wishDir.normalized * acceleration * 1000f * Time.deltaTime);
            if (moveInput)
            {
                //handles the ground friction
                rb.AddForce(-new Vector3(rb.velocity.x, 0f, rb.velocity.z) * groundFrictionmMult * Time.deltaTime * 200);
            }
            else { rb.AddForce(-new Vector3(rb.velocity.x, 0f, rb.velocity.z) * stoppingFrictionMult * Time.deltaTime * 200); }
        }
        else
        {
            rb.AddForce(wishDir.normalized * acceleration * 1000f * airMovementModifier * Time.deltaTime);
            if (HorizontalVelocity() > currentMaxAirVelocity && playerCanInput)
            {
                SetHorizontalVelocity(currentMaxAirVelocity);//makes sure the player can't move too much in the air
            }
            //Handles the walljumping inputs
            if (TouchingWallLeft() && Input.GetKeyDown(inputSetup.jump))
            {
                StartCoroutine(WallJump(true));
            }
            if (TouchingWallRight() && Input.GetKeyDown(inputSetup.jump))
            {
                StartCoroutine(WallJump(false));
            }
        }

    }

    /// <summary>
    /// Uses a RayCast to check if the ground is currently hit
    /// </summary>
    /// <returns>Wether the ground is currently hit</returns>
    bool HitGround()
    {
        float rayLength = .1f;//Only modify this or the feet

        bool hitGround = false;
        RaycastHit hit;
        for (int i = 0; i < allFeet.Length; i++)
        {
            Ray ray = new Ray(allFeet[i].position, new Vector3(0f, -rayLength, 0f));

            Physics.Raycast(ray, out hit, rayLength);
            Debug.DrawRay(ray.origin, ray.direction, Color.green);

            if (hit.collider != null)
            {
                hitGround = true;
            }

        }


        return hitGround;
    }

    public IEnumerator Jump()
    {
        float timeEllapsed = 0f;
        bool hasLetGo = false;
        UpdateAirMoveSpeed();
        rb.velocity = rb.velocity + (new Vector3(0f, jumpSpeed, 0f));
        while (timeEllapsed < extendedJumpDuration && !hasLetGo)
        {
            if (Input.GetKey(inputSetup.jump))
            {
                rb.AddForce(new Vector3(0f, extendedJumpForce * Time.deltaTime, 0f));
                timeEllapsed += Time.deltaTime;
                yield return null;

            }
            else { hasLetGo = true; }
        }

    }

    /// <summary>
    /// Jumps to the side
    /// </summary>
    /// <param name="left">Wether you're touching the left wall or the right</param>
    /// <returns></returns>
    public IEnumerator WallJump(bool left)
    {
        StartCoroutine(WaitEnableInput(wallJumpTime));
        float timeEllapsed = 0f;
        bool hasLetGo = false;
        rb.velocity = (new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z));
        if (left)
        {
            rb.velocity += (transform.right * walljumpSpeed);// if going right
        }
        else { rb.velocity += (-transform.right * walljumpSpeed); }//if going left

        while (timeEllapsed < extendedJumpDuration && !hasLetGo)
        {
            if (Input.GetKey(inputSetup.jump))
            {
                rb.AddForce(new Vector3(0f, extendedJumpForce * Time.deltaTime, 0f));
                timeEllapsed += Time.deltaTime;
                yield return null;

            }
            else { hasLetGo = true; }
        }

    }
    public IEnumerator WaitEnableInput(float time)
    {
        playerCanInput = false;
        yield return new WaitForSeconds(time);
        playerCanInput = true;
    }

    /// <summary>
    /// Sets the horizontal velocity
    /// </summary>
    /// <param name="amount">The amount of horizontal value the player will have</param>
    void SetHorizontalVelocity(float amount)
    {
        Vector2 currentHorizontalVector = new Vector2(rb.velocity.x, rb.velocity.z);
        Vector2 newHorizontalVector = currentHorizontalVector.normalized * amount;
        rb.velocity = new Vector3(newHorizontalVector.x, rb.velocity.y, newHorizontalVector.y);
    }

    float HorizontalVelocity()
    {
        return (new Vector2(rb.velocity.x + wishDir.x, rb.velocity.z + wishDir.z).magnitude);
    }

    public void UpdateAirMoveSpeed()
    {
        currentMaxAirVelocity = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
    }

    /// <summary>
    /// Checks if the player is touching a wall on the left
    /// </summary>
    /// <returns></returns>
    bool TouchingWallLeft()
    {
        Ray ray = new Ray(leftWallrun.position, -transform.right);

        RaycastHit hit;
        Physics.Raycast(ray, out hit, wallJumpRayLength);
        Debug.DrawRay(ray.origin, ray.direction);
        if (hit.collider)
        {
            return true;
        }
        else return false;
    }
    /// <summary>
    /// Checks if the player is touching a wall on the right
    /// </summary>
    /// <returns></returns>
    bool TouchingWallRight()
    {
        Ray ray = new Ray(rightWallrun.position, transform.right);

        RaycastHit hit;
        Physics.Raycast(ray, out hit, wallJumpRayLength);
        Debug.DrawRay(ray.origin, ray.direction);
        if (hit.collider)
        {
            return true;
        }
        else return false;
    }

    public void ChangePlayerInput(bool value)
    {
        playerCanInput = value;
    }
}
