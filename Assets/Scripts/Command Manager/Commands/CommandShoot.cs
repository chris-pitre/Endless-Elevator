using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandShoot : ICommand
{
    GameObject bullet;
    Transform bulletSpawn;
    bool isFacingRight;
    private Bullet bulletScript;
    private int damage;

    public CommandShoot(GameObject bullet, Transform bulletSpawn, bool isFacingRight, int damage){
        this.bullet = bullet;
        this.bulletSpawn = bulletSpawn;
        this.isFacingRight = isFacingRight;
        bulletScript = bullet.GetComponent<Bullet>();
        this.damage = damage;
    }
    public void Execute(){
       GameObject bulletInstance = Object.Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
       int direction = isFacingRight ? 1 : -1;
       bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * bulletScript.speed, 0);
       bulletInstance.GetComponent<Bullet>().damage = damage;
    }
}
