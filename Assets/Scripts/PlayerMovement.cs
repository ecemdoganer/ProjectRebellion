using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private Rigidbody2D body;
    private Animator animator;
    private Vector2 moveInput;
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isRunning = false;

    public float CurrentMoveSpeed {
        get {
            if (IsMoving) {
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
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
}