using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float dashSpeed;
    public float runSpeed; 
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;
    public float dashCooldown;
    private float dashCdTimer;
    private Vector3 delayedForceToApply; //to apply dash with a delay
    private bool isDashing;

    [Header("Sliding")]
    public float slideYScale;
    private bool isSliding;
    private float slideTimer;
    public float maxSlideTime;
    public float slideForce;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space; 
    public KeyCode slideKey = KeyCode.LeftShift; 
    public KeyCode dashKey = KeyCode.F; 


    [Header("Ground check")]
    public float playerHeight;
    public LayerMask groundLayerMask;
    private bool isGrounded; 

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDir;
    private Rigidbody rigibody;

    public MovementState currentState; 
    public enum MovementState
    {
        WALKING,
        CROUCHING,
        SLIDING,
        DASHING, 
        AIR
    }

    private void Start()
    {
        rigibody = GetComponent<Rigidbody>();
        rigibody.freezeRotation = true;

        startYScale = transform.localScale.y; 
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayerMask);
        Debug.Log(isGrounded); 

        GetInput();
        StateHandler(); 
        LimitSpeed(); 

        //Drag on player
        if (isGrounded && currentState != MovementState.DASHING)
        {
            rigibody.drag = groundDrag;
        }
        else
        {
            rigibody.drag = 0; 
        }

        //Dash Cooldown
        if(dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime; 
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (isSliding)
        {
            SlideMovement(); 
        }
    }

    private void GetInput()
    {
        //Movement inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Jump Input
        if(Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
          
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown); 
        }

        //Crouch Input
        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide(); 
        }

        if (Input.GetKeyUp(slideKey) && isSliding)
        {
            StopSlide(); 
        }

        //Dash Input 
        if (Input.GetKey(dashKey))
        {
            Dash(); 
        }

    }

    private void StateHandler()
    {
        if (isDashing)
        {
            currentState = MovementState.DASHING;
            moveSpeed = dashSpeed; 
        }
        else if (isSliding)
        {
            currentState = MovementState.SLIDING;
            moveSpeed = runSpeed;
        }
        //WALKING
        else if (isGrounded)
        {
            currentState = MovementState.WALKING;
            moveSpeed = runSpeed;

        }
        //AIR
        else
        {
            currentState = MovementState.AIR;
            moveSpeed = runSpeed;
        }

    }

    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded)
        {
            rigibody.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rigibody.AddForce(moveDir * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
       
    }

    private void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(rigibody.velocity.x, 0f, rigibody.velocity.z);

     
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rigibody.velocity = new Vector3(limitedVel.x, rigibody.velocity.y, limitedVel.z); 
        }
    }


    private void Jump()
    {
        rigibody.velocity = new Vector3(rigibody.velocity.x, 0f, rigibody.velocity.z);

        rigibody.AddForce(transform.up * jumpForce, ForceMode.Impulse); 
    }

    private void ResetJump()
    {
        readyToJump = true; 
    }

    private void StartSlide()
    {
        isSliding = true; 

        transform.localScale = new Vector3(transform.localScale.x, slideYScale, transform.localScale.z);
        rigibody.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime; 
    }

    private void SlideMovement()
    {
        Vector3 slideDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rigibody.AddForce(slideDir.normalized * slideForce, ForceMode.Force);

        slideTimer -= Time.deltaTime;

        if(slideTimer <= 0)
        {
            StopSlide(); 
        }
    }

    private void Dash()
    {
        if (dashCdTimer > 0)
        {
            return;
        }
        else
        {
            dashCdTimer = dashCooldown; 
        }



        isDashing = true; 
        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDash), 0.025f); 

        Invoke(nameof(ResetDash), dashDuration); 
    }

    private void DelayedDash()
    {
        rigibody.AddForce(delayedForceToApply, ForceMode.Impulse); 
    }

    private void ResetDash()
    {
        isDashing = false; 
    }

    private void StopSlide()
    {
        isSliding = false;
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
    }
}
