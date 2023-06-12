using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandShoot : ICommand
{
    GameObject bullet;
    Transform bulletSpawn;
    bool isFacingRight;
    private Bullet bulletScript;

    public CommandShoot(GameObject bullet, Transform bulletSpawn, bool isFacingRight){
        this.bullet = bullet;
        this.bulletSpawn = bulletSpawn;
        this.isFacingRight = isFacingRight;
        bulletScript = bullet.GetComponent<Bullet>();
    }
    public void Execute(){
       GameObject bulletInstance = Object.Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
       int direction = isFacingRight ? 1 : -1;
       bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * bulletScript.speed, 0); 
    }
}
