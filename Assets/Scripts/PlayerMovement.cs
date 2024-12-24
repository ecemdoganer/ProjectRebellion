using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Flip player when moving left and right
        if (horizontalInput > 0.01f) {
            transform.localScale = new Vector3((float)-4.0961, (float)4.007428, (float)0.5); 
        } else if (horizontalInput < -0.01f) {
            transform.localScale = new Vector3((float)4.0961, (float)4.007428, (float)0.5);
        }
        
        if (Input.GetKey(KeyCode.Space)) {
            body.velocity = new Vector2(body.velocity.x, speed);
        }
        
    }
}