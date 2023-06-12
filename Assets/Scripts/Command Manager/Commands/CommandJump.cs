using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandJump : ICommand
{
    Rigidbody2D rb;
    float jumpForce;
    public CommandJump(Rigidbody2D rb, float jumpForce){
        this.rb = rb;
        this.jumpForce = jumpForce;
    }

    public void Execute(){
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
