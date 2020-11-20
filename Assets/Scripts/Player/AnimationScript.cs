using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{

    private Animator anim;
    private Movement move;
    [HideInInspector]
    public SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        move = GetComponentInParent<Movement>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        

    }
    public void SetHorizontalMovement(float x,float y, float yVel)
    {
        anim.SetFloat("HorizontalAxis", x);
        anim.SetFloat("VerticalAxis", y);
        anim.SetFloat("VerticalVelocity", yVel);
    }


    public void Flip(int side)
    {
        bool state = (side == 1) ? false : true;
        sr.flipX = state;
    }
}
