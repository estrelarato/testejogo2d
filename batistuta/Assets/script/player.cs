using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player-related variables
    private Rigidbody2D rb;
    private float moveX;
    private Animator anim; 

    public float speed;
    public float jumpForce;
    public bool noChao;
    public int addJumps;

    // Initialize components
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            // Check jump conditions
            if (noChao || addJumps == 0)
            {
                Jump();
                if (!noChao) addJumps--;
            }
        }
    }

    // FixedUpdate is used for physics-based movements
    void FixedUpdate()
    {
        Move();

        if (noChao)
        {
            addJumps = 1;
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && addJumps > 0)
            {
                addJumps--;
                Jump();
            }
        }
    }

    // Move the player character
    void Move()
    {
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

        if (moveX > 0)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f); // Face right
            anim.SetBool("isRun", true);
        }
        else if (moveX < 0)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f); // Face left
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false); // Stop running animation
        }
    }

    // Handle jumping behavior
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetBool("isJump", true); // Trigger jump animation
    }

    // Detect collision with the ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            noChao = true;
            anim.SetBool("isJump", false); // Stop jump animation when grounded
        }
    }

    // Detect when player leaves the ground
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            noChao = false;
        }
    }
}
