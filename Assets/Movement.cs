using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    [SerializeField] private LayerMask platformslayermask;
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private int Jumps = 1;
    [SerializeField] private float firstJumpVelocity = 15f;
    [SerializeField] private float secondJumpVelocity = 10f;
    [SerializeField] private bool defaults = false;
    private float jumpVelocity;
    private Rigidbody2D rigidbody2d;
    private CircleCollider2D circlecollider2d;
    

    void Awake() {
        if (defaults)
        {
            platformslayermask = LayerMask.GetMask("Platform");
            moveSpeed = 5f;
            Jumps = 1;
            firstJumpVelocity = 15f;
            secondJumpVelocity = 10f;
        }
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        circlecollider2d = transform.GetComponent<CircleCollider2D>();
    
    }
    void FixedUpdate() {
       
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded() || canJump()))
        {
            
            if (isGrounded()) { jumpVelocity = firstJumpVelocity; }
            else { jumpVelocity = secondJumpVelocity; }
            rigidbody2d.velocity = Vector2.up * jumpVelocity;
            Jumps--;
        }
        if (isGrounded()) { Jumps = 1; }

        //Debug.DrawRay(circlecollider2d.bounds.center, Vector2.down, Color.red);
    }
    private bool isGrounded()
    {
        //RaycastHit2D sray = Physics2D.BoxCast(circlecollider2d.bounds.center, circlecollider2d.bounds.size,0f, Vector2.down, .1f, platformslayermask);
        RaycastHit2D sray = Physics2D.CircleCast(circlecollider2d.bounds.center, .5f, Vector2.down, .1f, platformslayermask);
        Debug.Log(sray.collider);
        return sray.collider != null;
    }
    private bool canJump()
    {
        if (Jumps == 0) { return false; }
        else { return true; }
    }

}
