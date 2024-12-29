using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
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

    
    public bool CanMove
    {
        get
        {
            return  animator.GetBool(AnimationStrings.canMove);
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
        body.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, body.velocity.y);
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
        //chechk if its alive?
        if (context.started && touchingDirections.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.jump);
            body.velocity = new Vector2(body.velocity.x, jumpImpulse);
        }
    }
}