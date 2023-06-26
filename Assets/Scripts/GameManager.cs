using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int numberOfEnemies = 0;
    public int currentFloor = 0;
    public bool completedLevel = false;
    public int currentLevel;
    public int maxLevels = 4;
    private static GameManager _instance;
    public static GameManager Instance{ get { return _instance;} }

    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }        
    }

    private void Start(){
        currentLevel = Random.Range(2, maxLevels);
        SceneManager.LoadSceneAsync(currentLevel, LoadSceneMode.Additive);
    }

    public void EnemyBirth(){
        numberOfEnemies++;
        CheckCompletionFlag();
    }

    public void EnemyDeath(){
        numberOfEnemies--;
        CheckCompletionFlag();
    }

    private void CheckCompletionFlag(){
        if(numberOfEnemies <= 0){
            if(GameObject.FindGameObjectWithTag("Elevator").TryGetComponent<Animator>(out Animator anim)){
                anim.SetTrigger("Open");
            }
            completedLevel = true;
        }
    }

    public void ChangeLevel(){
        if(GameObject.FindGameObjectWithTag("Elevator").TryGetComponent<Animator>(out Animator anim)){
            anim.SetTrigger("Close");
        }
        currentFloor++;
        HUDManager.Instance.UpdateLevel(currentFloor);
        completedLevel = false;
        int randomLevel = Random.Range(1, maxLevels);
        SceneManager.UnloadSceneAsync(currentLevel);
        SceneManager.LoadSceneAsync(randomLevel, LoadSceneMode.Additive);
        currentLevel = randomLevel;
    }
}
