using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]

public class DeathBringer : MonoBehaviour
{
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    private Rigidbody2D body;
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector;
    TouchingDirections touchingDirections;

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
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
        
        body.velocity = new Vector2(walkSpeed * walkDirectionVector.x, body.velocity.y);
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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
