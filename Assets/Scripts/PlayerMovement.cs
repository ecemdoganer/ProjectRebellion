using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private Rigidbody2D body;
    private Animator animator;
    private Vector2 moveInput;
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isRunning = false;
    public float jumpImpulse = 10f;
    private TouchingDirections touchingDirections;
    Damageable damageable;
    
    [SerializeField] private float coyoteTime = 0.2f; // Duration of coyote time
    [SerializeField] private float jumpBufferTime = 0.2f; // Duration of jump buffering

    private float coyoteTimeCounter; // Tracks remaining coyote time
    private float jumpBufferCounter; // Tracks remaining jump buffer time


    
    public bool CanMove
    {
        get
        {
            return  animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    public float CurrentMoveSpeed {
        get {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall) {
                    if (IsRunning) {
                        return runSpeed;
                    } else {
                        return walkSpeed;
                    }
                } else {
                    // Idle speed is zero
                    return 0;
                }
            }
            else
            {
                // Movement locked
                return 0;
            }
        }
    }

    public bool IsMoving {
        get {
            return _isMoving;
        }
        set {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isWalking, value);
        }
    }
    
    public bool IsRunning {
        get {
            return _isRunning;
        }
        set {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveInput.x > 0.01f) {
            transform.localScale = new Vector2((float)5.5961, (float)5.507428); 
        } else if (moveInput.x < -0.01f) {
            transform.localScale = new Vector2((float)-5.5961, (float)5.507428);
        }
    }

    private void FixedUpdate()
    {

        if (touchingDirections.IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }

        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.fixedDeltaTime;

            AttemptJump();
        }

        if (!damageable.IsHit)
        {
            body.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, body.velocity.y);
        }
        animator.SetFloat(AnimationStrings.yVelocity, body.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
    }

    
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        } else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attack);
        }
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        // Check if it's alive?
        if (context.started )
        {
            jumpBufferCounter = jumpBufferTime;

            if (touchingDirections.IsGrounded && CanMove)
            {
                animator.SetTrigger(AnimationStrings.jump);
                body.velocity = new Vector2(body.velocity.x, jumpImpulse);

                jumpBufferCounter = 0;
                coyoteTimeCounter = 0;
            }

            
        }
    }

    private void AttemptJump()
    {
        if (coyoteTimeCounter > 0 || touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jump);
            body.velocity = new Vector2(body.velocity.x, jumpImpulse);
            
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        body.velocity = new Vector2(knockback.x, body.velocity.y + knockback.y);
    }
}