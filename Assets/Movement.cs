using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    [SerializeField] private LayerMask platformsLayerMask;
    [SerializeField] private LayerMask wallsLayerMask;
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private int jumps = 1;
    [SerializeField] private float firstJumpVelocity = 15f;
    [SerializeField] private float secondJumpVelocity = 10f;
    [SerializeField] private bool defaults;
    private float jumpVelocity;
    private Rigidbody2D rigidbody2d;
    private CircleCollider2D circlecollider2d;
    private float side;
    void Awake() {
        if (defaults) {
            platformsLayerMask = LayerMask.GetMask("Platform");
            wallsLayerMask = LayerMask.GetMask("Wall");
            moveSpeed = 5f;
            jumps = 1;
            firstJumpVelocity = 15f;
            secondJumpVelocity = 10f;
        }
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        circlecollider2d = transform.GetComponent<CircleCollider2D>();
    
    }
    void FixedUpdate() {
       
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
                
        if (onWall() && rigidbody2d.velocity.y < 0) { Physics2D.gravity = new Vector3(0f, -1.962f, 0f); jumps = 1; }
        else { Physics2D.gravity = new Vector3(0f, -9.81f, 0f); }
        if ((Input.GetKeyDown(KeyCode.Space) && (isGrounded() || canJump())) /*&& rigidbody2d.velocity.y <= secondJumpVelocity*/) {
                   
            if (isGrounded()) { jumpVelocity = firstJumpVelocity; }
            else { jumpVelocity = secondJumpVelocity; }
            if (onWall()) {
                jumpVelocity = secondJumpVelocity;
            }
            rigidbody2d.velocity = new Vector2(side * (secondJumpVelocity/2f),jumpVelocity);
            
            jumps--;
        }
        if (isGrounded()) { jumps = 1; }
        
    transform.position += movement * Time.deltaTime * moveSpeed;
    }
    private bool isGrounded() {
        return Physics2D.IsTouchingLayers(circlecollider2d, LayerMask.GetMask("Platform"));
    }
    private bool canJump() {
        if (jumps == 0) { return false; }
        else { return true; }
    }
    private bool onWall() {
        if (Physics2D.IsTouchingLayers(circlecollider2d, LayerMask.GetMask("Left Wall"))) { side = 1f; }
        else if (Physics2D.IsTouchingLayers(circlecollider2d, LayerMask.GetMask("Right Wall"))) { side = -1f; }
        else { side = 0f; }
        return Physics2D.IsTouchingLayers(circlecollider2d, LayerMask.GetMask("Left Wall")) || Physics2D.IsTouchingLayers(circlecollider2d, LayerMask.GetMask("Right Wall"));
    }

}
