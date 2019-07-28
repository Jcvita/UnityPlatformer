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
        //if (wasOnWall()) { jumps = 0; }
        else { Physics2D.gravity = new Vector3(0f, -9.81f, 0f); }
        if ((Input.GetKeyDown(KeyCode.Space) && (isGrounded() || canJump())) && rigidbody2d.velocity.y <= secondJumpVelocity) {
                   
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
        //Debug.DrawRay(circlecollider2d.bounds.center, Vector2.down, Color.red);
    }

    private bool isGrounded() {
        //RaycastHit2D sray = Physics2D.BoxCast(circlecollider2d.bounds.center, circlecollider2d.bounds.size,0f, Vector2.down, .1f, platformslayermask);
        RaycastHit2D dray = Physics2D.CircleCast(circlecollider2d.bounds.center, .5f, Vector2.down, .1f, platformsLayerMask);
        Debug.Log(dray.collider);
        return dray.collider != null;
    }
    private bool canJump() {
        if (jumps == 0) { return false; }
        else { return true; }
    }
    private bool onWall() {
        RaycastHit2D lray = Physics2D.CircleCast(circlecollider2d.bounds.center, .5f, Vector2.left, .1f, wallsLayerMask);
        RaycastHit2D rray = Physics2D.CircleCast(circlecollider2d.bounds.center, .5f, Vector2.right, .1f, wallsLayerMask);
        Debug.Log(lray.collider);
        Debug.Log(rray.collider);
        if (lray.collider != null) { side = 1f; }
        if (rray.collider != null) { side = -1f; }
        if (lray.collider == null && rray.collider == null) { side = 0f; }
        return (lray.collider != null || rray.collider != null);
    }
    //private bool wasOnWall()
    //{
    //    bool touched = false;
    //    bool x = false;
    //    if (onWall()) { touched = true; }
    //    if (isGrounded() && !onWall()) { touched = false; }
    //    x = touched;
    //    if (onWall()) { x = false; }
    //    return x;
    //}
}
