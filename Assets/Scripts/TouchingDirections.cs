using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;

    private CapsuleCollider2D touchingCollider;
    Animator animator;
    [SerializeField] private bool _isGrounded;

    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] wallHits = new RaycastHit2D[5];
    private RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    

    public bool IsGrounded { 
        get {
            return _isGrounded;
        } private set {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }
    
    [SerializeField] 
    private bool _isTouchingWall;

    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    public bool _isOnWall { 
        get {
            return _isOnWall;
        } private set {
            _isOnWall = value;
            animator.SetBool(AnimationStrings._isOnWall, value);
        }
    }
    
    public bool _isOnCeiling { 
        get {
            return _isOnCeiling;
        } private set {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings._isOnCeiling, value);
        }
    }

    private void Awake()
    {
        touchingCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        IsGrounded = touchingCollider.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        _isOnWall = touchingCollider.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        _isOnCeiling = touchingCollider.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;

    }
}
