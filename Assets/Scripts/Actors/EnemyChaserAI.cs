using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyChaserAI : MonoBehaviour
{
    [Header("Actor Script")]
    [SerializeField] public Actor actorScript;
    
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    private bool isFacingRight = true;
    private Invoker invoker;
    [SerializeField] private Seeker seeker;

    private Vector2 currentVelocity;
    private Vector2 direction; 
    private int health;
    [HideInInspector] public bool intangible = false;
    public void Start(){
        GameManager.Instance.EnemyBirth();
        invoker = new Invoker();
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
        health = actorScript.maxHealth;
        actorScript.animator.SetTrigger("Walk");
    }

    private void FixedUpdate(){
        if(TargetInDistance() && followEnabled){
            PathFollow();
        }
    }

    private void UpdatePath(){
        if(followEnabled && TargetInDistance() && seeker.IsDone()){
            seeker.StartPath(actorScript.rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow(){
        if(intangible){
            return;
        }

        if(path == null){
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count){
            return;
        }

        direction = ((Vector2)path.vectorPath[currentWaypoint] - actorScript.rb.position).normalized;

        if(jumpEnabled && IsGrounded()){
            if(direction.y > jumpNodeHeightRequirement){
                ICommand jump = new CommandJump(actorScript.rb, actorScript.jumpForce);
                invoker.Execute(jump);
            }
        }

        if(Mathf.Abs(direction.x) > 0.5f){
            if(direction.x < 0f){
                ICommand move = new CommandMove(actorScript.rb, -1, actorScript.speed);
                invoker.Execute(move);
            } else {
                ICommand move = new CommandMove(actorScript.rb, 1, actorScript.speed);
                invoker.Execute(move);
            }
        }
        
        float distance = Vector2.Distance(actorScript.rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance){
            currentWaypoint++;
        }

        if(directionLookEnabled){
            Flip(direction.x);
        }
    }
    private void Flip(float direction){
        if(isFacingRight && direction < 0f || !isFacingRight && direction > 0f){
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private bool TargetInDistance(){
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    private bool IsGrounded(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(actorScript.boxCollider.bounds.center, actorScript.boxCollider.bounds.size, 0f, Vector2.down, 0.1f, actorScript.worldLayer);
        return raycastHit.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player) && !Player.invincible){
            Player.TakeDamage(actorScript.attack);
            Player.isDamaged = true;
            Player.invincible = true;
            intangible = true;
            Vector2 damageDirection = new Vector2(direction.x * 10, 12);
            actorScript.rb.velocity = new Vector2(-direction.x * 10, 12);
            player.rb.velocity = damageDirection;
            StartCoroutine(PlayerDamagedFlag(collision));
        }
    }

    private IEnumerator PlayerDamagedFlag(Collider2D collision){
        yield return new WaitForSeconds(0.5f);
        Player.isDamaged = false;
        intangible = false;
        yield return new WaitForSeconds(1.5f);
        Player.invincible = false;
    }

    public void TakeDamage(int damage){
        int real_damage;
        if(damage >= actorScript.defense){
            real_damage = damage * 2 - actorScript.defense;
        } else {
            real_damage = damage * damage / actorScript.defense;
        }
        health -= real_damage;
        if(health <= 0){
            GameManager.Instance.EnemyDeath();
            Destroy(this.gameObject);
        }
    }
}
