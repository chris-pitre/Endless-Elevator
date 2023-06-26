using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numberOfEnemies;
    public int currentFloor = 0;
    public bool completedLevel = false;
    private static GameManager _instance;
    public static GameManager Instance{ get { return _instance;} }

    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        HUDManager.Instance.UpdateLevel(currentFloor);
    }

    private void Start(){
        GetEnemies();
    }

    public void GetEnemies(){
        numberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void EnemyDeath(){
        numberOfEnemies--;
        Debug.Log(numberOfEnemies);
        if(numberOfEnemies <= 0){
            if(GameObject.FindGameObjectWithTag("Elevator").TryGetComponent<Animator>(out Animator anim)){
                anim.SetTrigger("Open");
            }
            completedLevel = true;
        }
    }
}
