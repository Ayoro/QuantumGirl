using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    bool onGround = true;
    RaycastHit2D hit;
    public Animator animator;
    [HideInInspector]
    public Rigidbody2D rb;
    private AnimationScript anim;

    [Space]
    [Header("Stats")]
    public float speed = 3;
    public float jumpForce = 1; 

    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallJumped;

    [Space]
    public int side = 1;
    
    
    private void Walk(Vector2 dir)
    {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }
    private void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce / 5;
    }
    private void anim_change()
    {
        if(onGround)
        {
            if (rb.velocity.x == 0)
                animator.SetInteger("mode", 0);//站立
            else
                animator.SetInteger("mode", 1);//步行
        }
        else
        {
            if (rb.velocity.y >= 0)
                animator.SetInteger("mode", 2);//向上跳
            else if (rb.velocity.y < 0)
                animator.SetInteger("mode", 3);//向下坠落
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(transform.position + new Vector3(0, -0.85f), Vector2.down, 0.5f);
        if (!hit)
        {
            onGround = false;
        }
        else if (hit.collider.tag == "energy_block" || hit.collider.tag == "ground"|| hit.collider.tag == "grass"||hit.collider.tag=="water")
        {
            onGround = true;
        }
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        if (onGround)
        {
            GetComponent<BetterJumping>().enabled = true;
        }
        
        rb.gravityScale = 3;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
                Jump(Vector2.up);
        }

        if(x > 0)
        {
            side = 1;
            anim.Flip(side);
        }
        if (x < 0)
        {
            side = -1;
            anim.Flip(side);
        }
        anim_change();
    }
}
