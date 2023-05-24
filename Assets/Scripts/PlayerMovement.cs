using UnityEngine;

// Edited from:
// https://github.com/Dawnosaur/platformer-movement
// https://www.youtube.com/watch?v=KbtcEVCM7bw

public class PlayerMovement : MonoBehaviour
{
    // Speed and other values are saved in a Player Movement Data object
    public PlayerMovementData Data;
    private Rigidbody2D rb;
    private Transform t;
    private bool isFacingRight;
    private bool isJumping;
    private bool isJumpCut; // Longer jump while holding down the jump button
    private bool isJumpFalling;
    private float lastOnGroundTime;
    private float lastPressedJumpTime;
    private Vector2 moveInput;

    [Header("Ground check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D lastGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        t = GetComponent<Transform>();
    }

    private void Start()
    {
        SetGravityScale(Data.gravityScale);
        isFacingRight = true;
    }

    private void Update()
    {
        lastOnGroundTime -= Time.deltaTime;
        lastPressedJumpTime -= Time.deltaTime;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        CheckFlip();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J))
        {
            OnJumpInput();
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J))
        {
            OnJumpUpInput();
        }

        if (!isJumping)
        {
            // Ground Check
            Collider2D result = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            if (result)
            {
                lastOnGroundTime = Data.coyoteTime;
                lastGround = result.GetComponent<Rigidbody2D>();
            }
            else
            {
                lastGround = null;
            }
        }
        else
        {
            lastGround = null;
        }

        if (isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
        }

        if (lastOnGroundTime > 0 && !isJumping)
        {
            isJumpCut = false;
            if (!isJumping) isJumpFalling = false;
        }

        // Jump
        if (CanJump() && lastPressedJumpTime > 0)
        {
            isJumping = true;
            isJumpCut = true;
            isJumpFalling = false;
            Jump();
        }

        if (rb.velocity.y < 0 && moveInput.y < 0)
        {
            // Much higher gravity if holding down
            SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
            // Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -Data.maxFastFallSpeed));
        }
        else if (isJumpCut)
        {
            // Higher gravity if jump button released
            SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -Data.maxFallSpeed));
        }
        else if ((isJumping || isJumpFalling) && Mathf.Abs(rb.velocity.y) < Data.jumpHangTimeThreshold)
        {
            SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
        }
        else if (rb.velocity.y < 0)
        {
            // Higher gravity if falling
            SetGravityScale(Data.gravityScale * Data.fallGravityMult);
            // Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -Data.maxFallSpeed));
        }
        else
        {
            // Default gravity if standing on a platform or moving upwards
            SetGravityScale(Data.gravityScale);
        }
    }

    private void FixedUpdate()
    {
        // Calculate the direction we want to move in and our desired velocity
        float targetSpeed = moveInput.x * Data.runMaxSpeed;
        // We can reduce are control using Lerp() this smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, 1);

        float accelRate;
        
        
        // Gets an acceleration value based on if we are accelerating (includes turning) 
        // or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        if (lastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;

        // Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((isJumping || isJumpFalling) && Mathf.Abs(rb.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }

        // We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (Data.doConserveMomentum && Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && lastOnGroundTime < 0)
        {
            // Prevent any deceleration from happening, or in other words conserve are current momentum
            // You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }

        // Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - rb.velocity.x;
        float movement = speedDif * accelRate;

        // In case we are on a moving platform, add it's velocity to the player to mvoe with the platform
        if (lastGround != null)
        {
            rb.velocity = lastGround.velocity;
            if (targetSpeed != 0)
            {
                rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
            }
        }
        else
        {
            // Convert this to a vector and apply to rigidbody
            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }
    }


    // Methods which whandle input detected in Update()
    public void OnJumpInput()
    {
        lastPressedJumpTime = Data.jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut()) 
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - 0.5f),ForceMode2D.Impulse);
            isJumpCut = true;
        }
    }

    public void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }

    private void Jump()
    {
        // Ensures we can't call Jump multiple times from one press
        lastPressedJumpTime = 0;
        lastOnGroundTime = 0;

        // We increase the force applied if we are falling
        // This means we'll always feel like we jump the same amount 
        // (setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = Data.jumpForce;
        if (rb.velocity.y < 0) force -= rb.velocity.y;

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    public void CheckFlip()
    {
        if ((moveInput.x > 0 && !isFacingRight) || (moveInput.x < 0 && isFacingRight))
        {
            Vector3 scale = t.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            isFacingRight = !isFacingRight;
        }
    }

    private bool CanJump()
    {
        return lastOnGroundTime > 0 && !isJumping;
    }

    private bool CanJumpCut()
    {
        return isJumping && rb.velocity.y > 0;
    }
}
