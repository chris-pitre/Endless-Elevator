using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMove : ICommand
{
    Rigidbody2D rb;
    float direction;
    float speed;
    public CommandMove(Rigidbody2D rb, float direction, float speed){
        this.rb = rb;
        this.direction = direction;
        this.speed = speed;
    }

    public void Execute(){
        Vector2 destinationVector = new Vector2(direction * speed, rb.velocity.y);
        rb.velocity = Vector2.MoveTowards(rb.velocity, destinationVector, speed);
    }
}  