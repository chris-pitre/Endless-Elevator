using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] public Bullet bulletScript;
    void Awake(){
        GameObject ignore = GameObject.FindGameObjectWithTag(bulletScript.ignoredTag);
        Physics2D.IgnoreCollision(ignore.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.0f);
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.TryGetComponent<EnemyChaserAI>(out EnemyChaserAI enemy)){
            enemy.TakeDamage(bulletScript.damage);
        }
        Destroy(this.gameObject);
    }
}

