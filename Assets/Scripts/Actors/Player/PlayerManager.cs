using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Actor References")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public GameObject bullet;
    [SerializeField] public Transform bulletSpawn;
    [SerializeField] public BoxCollider2D boxCollider;
    [SerializeField] public LayerMask worldLayer;
    [SerializeField] public Animator animator;
    [SerializeField] public SpriteRenderer sprite;
   private Invoker invoker;
   private float x_input;
   private bool isFacingRight = true;

   private bool isDashing = false;
   private bool canExit = false;
   private float coyoteTimeCounter;
   private float jumpBufferCounter;

    void Start(){
        invoker = new Invoker();
        Player.SetAttack();
        Player.SetDefense();
        Player.SetHealth();
    }

    void Update() {
        DamageColor();
        Debug.Log("Can Exit: " + canExit+", Level Complete: "+GameManager.Instance.completedLevel);

        if(Input.GetKeyDown(KeyCode.Z)){
            ICommand shoot = new CommandShoot(bullet, bulletSpawn, isFacingRight, Player.attack);
            invoker.Execute(shoot);
        }
        
        if(Input.GetKeyDown(KeyCode.UpArrow) && canExit){
            canExit = false;
            Debug.Log("leaving level");
        }

        if(isDashing){
            return;
        }

        x_input = Input.GetAxisRaw("Horizontal");

        if(IsGrounded()){
            coyoteTimeCounter = Player.coyoteTime;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            jumpBufferCounter = Player.jumpBufferTime;
        } else {
            jumpBufferCounter -= Time.deltaTime;
        }

        if(coyoteTimeCounter > 0f && jumpBufferCounter > 0f){
            ICommand jump = new CommandJump(rb, Player.jumpForce);
            invoker.Execute(jump);
            jumpBufferCounter = 0f;
        }

        if(Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f){
            ICommand jump = new CommandJump(rb, rb.velocity.y * 0.5f);
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

        if(x_input != 0 && !Player.isDamaged){
            ICommand move = new CommandMove(rb, x_input, Player.speed);
            invoker.Execute(move);
            animator.SetTrigger("Walk");
        } else if (IsGrounded() && !Player.isDamaged) {
            ICommand move = new CommandMove(rb, 0, Player.speed);
            invoker.Execute(move);
            animator.SetTrigger("Idle");
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, worldLayer);
        return raycastHit.collider != null;
    }

    private IEnumerator Dash(){
        isDashing = true;
        Player.invincible = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 1f;
        rb.velocity = new Vector2(transform.localScale.x * Player.dashingPower, 0f);
        yield return new WaitForSeconds(Player.dashingTime);
        rb.gravityScale = originalGravity;
        yield return new WaitUntil(IsGrounded);
        isDashing = false;
        Player.invincible = false;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(isDashing && collision.gameObject.TryGetComponent<EnemyChaserAI>(out EnemyChaserAI enemy)){
            enemy.TakeDamage(Player.attack / 8);
            enemy.intangible = true;
            enemy.actorScript.animator.SetTrigger("Idle");
            Physics2D.IgnoreCollision(boxCollider, enemy.GetComponent<Collider2D>(), true);
            int direction = isFacingRight ? 1 : -1;
            Vector2 damageDirection = new Vector2(direction * 20, 12);
            collision.rigidbody.velocity = damageDirection;
            StartCoroutine(EnemyDashDamagedFlag(enemy));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(GameManager.Instance.completedLevel && collision.gameObject.tag == "Elevator"){
            canExit = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if(GameManager.Instance.completedLevel && collision.gameObject.tag == "Elevator"){
            canExit = false;
        }
    }

    private IEnumerator EnemyDashDamagedFlag(EnemyChaserAI enemy){
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(boxCollider, enemy.GetComponent<Collider2D>(), false);
        enemy.intangible = false;
        enemy.actorScript.animator.SetTrigger("Walk");
    }

    public void DamageColor(){
        if(Player.invincible){
            sprite.color = Color.red;
        } else {
            sprite.color = Color.white;
        }
    }
}
