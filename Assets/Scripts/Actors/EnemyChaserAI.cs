using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyChaserAI : MonoBehaviour
{
    [Header("Actor Script")]
    [SerializeField] Actor actorScript;
    
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpForce = 9f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    [SerializeField] private Seeker seeker;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 currentVelocity;

    public void Start(){
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate(){
        if(TargetInDistance() && followEnabled){
            PathFollow();
        }
    }

    private void UpdatePath(){
        if(followEnabled && TargetInDistance() && seeker.IsDone()){
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow(){
        if(path == null){
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count){
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if(jumpEnabled && IsGrounded()){
            if(direction.y > jumpNodeHeightRequirement){
                rb.AddForce(Vector2.up * speed * jumpForce);
            }
        }

        if(!IsGrounded()) force.y = 0;

        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref currentVelocity, 0.5f);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance){
            currentWaypoint++;
        }

        if(directionLookEnabled){
            if(rb.velocity.x > 0.05f){
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            } else if (rb.velocity.x < -0.05f){
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
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
}
