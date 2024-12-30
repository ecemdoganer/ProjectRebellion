using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]

public class DeathBringer : MonoBehaviour
{
    public DetectionZone attackZone;
    public float walkSpeed;
    public float walkStopRate = 0.6f;
    private Rigidbody2D body;
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    TouchingDirections touchingDirections;
    Animator animator;

    public WalkableDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        } set {
            if (_walkDirection != value)
            {
                // Direction flipped
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                
                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                } else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            
            _walkDirection = value; 
        }
    }
    
    public enum WalkableDirection 
    {
        Left, Right
    };

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }

    public bool _hasTarget = false;

    public bool HasTarget {
        get {
            return _hasTarget;
        } private set {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        // Duvar veya tavan algılaması kontrolü
        if (touchingDirections.IsGrounded && (touchingDirections.IsOnWall || touchingDirections.IsOnCeiling))
        {
            FlipDirection();
        }

        // Hareket kontrolü
        if (CanMove)
        {
            body.velocity = new Vector2(walkSpeed * walkDirectionVector.x, body.velocity.y);
        }
        else
        {
            body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, 0, walkStopRate), body.velocity.y);
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        } else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        } else {
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
        }
        Vector2 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (WalkDirection == WalkableDirection.Right ? 1 : -1);
        transform.localScale = scale;
        
        walkDirectionVector = WalkDirection == WalkableDirection.Right ? Vector2.right : Vector2.left;
        
    }
}
