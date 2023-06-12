using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   [SerializeField] private Player actorScript; 
   private Invoker invoker;
   private float x_input;
   private bool isFacingRight = true;

   private bool isDashing = false;

   private float coyoteTimeCounter;
   private float jumpBufferCounter;

    void Start(){
        invoker = new Invoker();   
    }

    void Update() {

        if(Input.GetKeyDown(KeyCode.Z)){
            ICommand shoot = new CommandShoot(actorScript.bullet, actorScript.bulletSpawn, isFacingRight);
            invoker.Execute(shoot);
        }

        if(isDashing){
            return;
        }

        x_input = Input.GetAxisRaw("Horizontal");

        if(IsGrounded()){
            coyoteTimeCounter = actorScript.coyoteTime;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            jumpBufferCounter = actorScript.jumpBufferTime;
        } else {
            jumpBufferCounter -= Time.deltaTime;
        }

        if(coyoteTimeCounter > 0f && jumpBufferCounter > 0f){
            ICommand jump = new CommandJump(actorScript.rb, actorScript.jumpForce);
            invoker.Execute(jump);
            jumpBufferCounter = 0f;
        }

        if(Input.GetKeyUp(KeyCode.Space) && actorScript.rb.velocity.y > 0f){
            ICommand jump = new CommandJump(actorScript.rb, actorScript.rb.velocity.y * 0.5f);
            invoker.Execute(jump);
            coyoteTimeCounter = 0f;
        }

        if(Input.GetKeyDown(KeyCode.C)){
            StartCoroutine(Dash());
        }

        Flip();
    }

    void FixedUpdate(){
        if(isDashing){
            return;
        }

        if(x_input != 0){
            ICommand move = new CommandMove(actorScript.rb, x_input, actorScript.speed);
            invoker.Execute(move);
            actorScript.animator.SetTrigger("Walk");
        } else if (IsGrounded()) {
            ICommand move = new CommandMove(actorScript.rb, 0, actorScript.speed);
            invoker.Execute(move);
            actorScript.animator.SetTrigger("Idle");
        }
    }
    private void Flip(){
        if(isFacingRight && x_input < 0f || !isFacingRight && x_input > 0f){
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(actorScript.boxCollider.bounds.center, actorScript.boxCollider.bounds.size, 0f, Vector2.down, 0.1f, actorScript.worldLayer);
        return raycastHit.collider != null;
    }

    private IEnumerator Dash(){
        isDashing = true;
        float originalGravity = actorScript.rb.gravityScale;
        actorScript.rb.gravityScale = 1f;
        actorScript.rb.velocity = new Vector2(transform.localScale.x * actorScript.dashingPower, 0f);
        yield return new WaitForSeconds(actorScript.dashingTime);
        actorScript.rb.gravityScale = originalGravity;
        yield return new WaitUntil(IsGrounded);
        isDashing = false;
    }
}
