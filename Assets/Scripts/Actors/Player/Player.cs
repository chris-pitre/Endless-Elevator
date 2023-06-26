public static class Player
{
    //Default Universal Actor Settings
    public static int max_health = 100;
    public static int d_attack = 5;
    public static int d_defense = 5;
    public static int speed = 12; //does not change through gameplay
    public static int jumpForce = 20; //does not change through gameplay
    public static int health;
    public static int attack;
    public static int defense;
    //Player Specific Settings
    public static float dashingPower = 4f;
    public static float dashingTime = 0.2f;
    public static float coyoteTime = 0.2f;
    public static float jumpBufferTime = 0.2f;
    public static bool isDamaged = false;
    public static bool invincible = false;

    public static void SetHealth(){
        health = max_health;
    }
    public static void SetAttack(){
        attack = d_attack;
    }
    public static void SetDefense(){
        defense = d_defense;
    }
    public static void TakeDamage(int damage){
        int real_damage;
        if(damage >= defense){
            real_damage = damage * 2 - defense;
        } else {
            real_damage = damage * damage / defense;
        }
        health -= real_damage;
        HUDManager.Instance.SetHealth(health);
    }
}
