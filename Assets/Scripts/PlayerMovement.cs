using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float runSpeed; 
    public float wallrunSpeed; 
    public float groundDrag;
    public Animator playerAnimator;
    public GameObject playerObject; 

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;


    [Header("Sliding")]
    public float slideYScale;
    private bool isSliding;
    private float slideTimer;
    public float maxSlideTime;
    public float slideForce;

    [Header("Wallrunning")]
    public float wallRunForce;
    public float maxWallRunTime;
    private float wallRunTimer;
    private bool isWallrunning;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;
    public ParticleSystem speedParticles;
    private Vector3 delayedForceToApply; 

    [Header("Shielding")]
    public GameObject shield;
    public bool isShielding;
    public float shieldDuration; 
    

    [Header("Wall detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit, rightWallHit;
    private bool wallLeft, wallRight;

    [Header("Affected by Explosion")]
    public float affectedTime;
    private bool justExploded;


    [Header("Camera")]
    public PlayerCamera cam;


    [Header("Spawn position")]
    public Vector3 spawnPosition;

    [Header("Item detection")]
    private ItemCollectible itemInRange;
    private bool isInRange;
    public bool hasShield = false;
    public bool hasWallRun = false;
    public bool hasFrog = false; 


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space; 
    public KeyCode slideKey = KeyCode.LeftShift; 
    public KeyCode interactionKey = KeyCode.E; 
    public KeyCode shieldKey = KeyCode.Mouse0; 


    [Header("Ground check")]
    public float playerHeight;
    public LayerMask groundLayerMask;
    public LayerMask wallLayerMask;
    private bool isGrounded; 

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDir;
    private Rigidbody rigibody;

    public MovementState currentState; 
    public AnimationState currentAnimationState; 
    public enum MovementState
    {
        WALKING,
        CROUCHING,
        SLIDING,
        WALLRUNNING, 
        AIR
    }
    
    public enum AnimationState
    {
        IDLE,
        RUNNING,
        SLIDING,
        WALLRUNNING_LEFT,
        WALLRUNNING_RIGHT, 
        JUMPING,
        SHIELDING,
        FALLING
    }

    private void Start()
    {
        rigibody = GetComponent<Rigidbody>();
        rigibody.freezeRotation = true;

        spawnPosition = transform.position; 

        startYScale = transform.localScale.y;
        
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayerMask);
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.green);

        GetInput();
        StateHandler(); 
        LimitSpeed();
        CheckForWall(); 

        //Drag on player
        if (isGrounded)
        {
            rigibody.drag = groundDrag;
        }
        else
        {
            rigibody.drag = 0; 
        }


    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (isSliding)
        {
            SlideMovement(); 
        }

        if (isWallrunning)
        {
            WallrunMovement(); 
        }
    }

    private void GetInput()
    {
        //Movement inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if((horizontalInput != 0 || verticalInput != 0) && isGrounded && !isSliding && !isShielding)
        {
            ChangeAnimationState(AnimationState.RUNNING); 
        }else if(horizontalInput == 0 && verticalInput == 0 && isGrounded && !isShielding)
        {
            ChangeAnimationState(AnimationState.IDLE);
        }

        //Jump Input
        if(Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            if(!isShielding)
            {
                ChangeAnimationState(AnimationState.JUMPING);
            }
            
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown); 
        }

        if(rigibody.velocity.y < 0 && !isGrounded && !isShielding)
        {
            ChangeAnimationState(AnimationState.FALLING);
        }
        

        //Crouch Input
        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
        {
            if (!isShielding)
            {
                ChangeAnimationState(AnimationState.SLIDING);
            }
            
            StartSlide(); 
        }

        if (Input.GetKeyUp(slideKey) && isSliding)
        {
            StopSlide(); 
        }


        //Wallrun Input
        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall && hasWallRun)
        {
            if (!isWallrunning)
            {
                StartWallrun();
                if (wallLeft && !isShielding)
                {
                    ChangeAnimationState(AnimationState.WALLRUNNING_LEFT);
                }else if (wallRight && !isShielding)
                {
                    ChangeAnimationState(AnimationState.WALLRUNNING_RIGHT);
                }
            }

            if(wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime; 
            }

            if(wallRunTimer <=0 && isWallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime; 
            }

            if (Input.GetKeyDown(jumpKey))
            {
                if(!isShielding)
                {
                    ChangeAnimationState(AnimationState.JUMPING);
                }
                
                WallJump(); 
            }
            
        }else if(exitingWall)
        {
            if (isWallrunning)
            {
                StopWallrun();
            }

            if(exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime; 
            }
            if(exitWallTimer <= 0)
            {
                exitingWall = false; 
            }
        }
        else
        {
            if (isWallrunning)
            {
                StopWallrun(); 
            }
        }

        //Interaction Input 
        if (Input.GetKey(interactionKey) && isInRange)
        {
            ActivePower(itemInRange.powerUnlocked);
        }

        //Shield Input
        if (Input.GetKey(shieldKey) && hasShield)
        {
            ChangeAnimationState(AnimationState.SHIELDING);
            PutShieldOn(); 
        }
    }

    private void StateHandler()
    {

        if (isWallrunning)
        {
            currentState = MovementState.WALLRUNNING;
            moveSpeed = wallrunSpeed; 
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
        else if (!isGrounded && !exitingWall && !wallRight && !wallLeft && !justExploded)
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

        playerObject.transform.localScale = new Vector3(playerObject.transform.localScale.x, slideYScale, playerObject.transform.localScale.z);
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




    private void StopSlide()
    {
        isSliding = false;
        playerObject.transform.localScale = new Vector3(playerObject.transform.localScale.x, startYScale, playerObject.transform.localScale.z);
    }


    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wallLayerMask); 
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, wallLayerMask); 
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, groundLayerMask); 
    }

    private void StartWallrun()
    {
        speedParticles.Play(); // PLAY PARTICLES

        isWallrunning = true;
        wallRunTimer = maxWallRunTime;

        cam.DoFov(75f);
        if (wallLeft)
        {
            cam.DoTilt(-5f);
        }
        else if (wallRight)
        {
            cam.DoTilt(5f);
        }
    }

    private void WallrunMovement()
    {
        rigibody.useGravity = false;
        rigibody.velocity = new Vector3(rigibody.velocity.x, 0f, rigibody.velocity.z); 

        Vector3 wallNormal = Vector3.zero;

        if (wallRight)
        {
            wallNormal = rightWallHit.normal;
        }
        else if(wallLeft)
        {
            wallNormal = leftWallHit.normal;
        }

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward; 
        }

        rigibody.AddForce(wallForward * wallRunForce, ForceMode.Force); 
    }

    private void StopWallrun()
    {
        speedParticles.Stop();

        rigibody.useGravity = true;
        isWallrunning = false;

        //WallJump(); 

        //Reset camera effects
        cam.DoFov(60f);
        cam.DoTilt(0f); 
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime; 


        Vector3 wallNormal = Vector3.zero;

        if (wallRight)
        {
            wallNormal = rightWallHit.normal;
        }
        else if (wallLeft)
        {
            wallNormal = leftWallHit.normal;
        }

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
        delayedForceToApply = forceToApply;

        rigibody.velocity = new Vector3(rigibody.velocity.x, 0f, rigibody.velocity.z);

        Invoke(nameof(DelayedWallJump), 0.025f);

    }

    private void DelayedWallJump()
    {
        rigibody.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    public void GetAffectedByExplosion(Vector3 direction, float force)
    {
        justExploded = true; 
        rigibody.AddForceAtPosition(direction * force + Vector3.up * force, transform.position, ForceMode.Impulse);

        Invoke(nameof(CancelAffectedByExplosion), affectedTime);
    }

    private void CancelAffectedByExplosion()
    {
        justExploded = false; 
    }

    private void ChangeAnimationState(AnimationState newState)
    {
        if(currentAnimationState == newState)
        {
            return; 
        }

        playerAnimator.Play(newState.ToString());

        currentAnimationState = newState; 
    }

    public void SavePosition(Vector3 savePoint)
    {
        spawnPosition = savePoint;
        Debug.Log(spawnPosition); 
    }

    public void Respawn()
    {
        rigibody.velocity = Vector3.zero; 
        transform.position = spawnPosition; 
    }

    public void SetItemInRange(ItemCollectible item)
    {
        isInRange = true; 
        itemInRange = item;
        Debug.Log(itemInRange); 
    }

    public void ClearItemInRange()
    {
        isInRange = false; 
        itemInRange = null;
        Debug.Log(itemInRange);
    }

    public void ActivePower(string power)
    {

        switch (power)
        {
            case "Frog":
                hasFrog = true;
                break;
            case "Shield":
                hasShield = true;
                break;
            case "Wallrun":
                hasWallRun = true;
                break;

        }
        itemInRange.SetInactive();
    }

    public void PutShieldOn()
    {
        shield.SetActive(true);
        isShielding = true;

        Invoke(nameof(RemoveShield), shieldDuration); 
    }

    public void RemoveShield()
    {
        shield.SetActive(false);
        isShielding = false;
    }

}
