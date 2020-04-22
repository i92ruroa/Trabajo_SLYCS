using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    // Movement Speed
    public float speed = 0.05f;
    public float jumpForce = 1200;


    // Current movement Direction
    Vector2 dir = Vector2.right;

    // Upwards push force
    public float upForce = 800;


    bool IsGrounded()
    {
        // Get Bounds and Cast Range (10% of height)
        Bounds bounds = GetComponent<Collider2D>().bounds;
        float range = bounds.size.y * 0.1f;

        // Calculate a position slightly below the collider
        Vector2 v = new Vector2(bounds.center.x,
            bounds.min.y - range);

        // Linecast upwards
        RaycastHit2D hit = Physics2D.Linecast(v, bounds.center);

        // Was there something in-between, or did we hit ourself?
        return (hit.collider.gameObject != gameObject);
    }

    void FixedUpdate() {
        // Set the Velocity
        GetComponent<Rigidbody2D>().velocity = dir * speed;

        // Vertical Movement (Jumping)
        bool grounded = IsGrounded();
        if (grounded)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce * 2);
            GetComponent<Animator>().SetBool("Jumping", !grounded);
        }

    }

    void OnTriggerEnter2D(Collider2D coll) {
        // Hit a destination? Then move into other direction
        transform.localScale = new Vector2(-1 * transform.localScale.x,
                                                transform.localScale.y);


        // And mirror it
        dir = new Vector2(-1 * dir.x, dir.y);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        // Collided with BabyMario?
        if (coll.gameObject.name == "BabyMario") {
            // Is the collision above?
            if (coll.contacts[0].point.y > transform.position.y) {
                // Play Animation
                GetComponent<Animator>().SetTrigger("Died");

                // Disable collider so it falls downwards
                GetComponent<Collider2D>().enabled = false;
                
                // Push BabyMario upwards
                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * upForce);
                
                // Die in a few seconds
                Invoke("Die", 5);
            } else {
                // Kill BabyMario
                Destroy(coll.gameObject);
            }
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}