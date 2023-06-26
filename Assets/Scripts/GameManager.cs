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
    private int maxLevels;
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
        currentLevel = GetRandomLevelIndex(1);
        maxLevels = SceneManager.sceneCountInBuildSettings;
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
            if(GameObject.FindGameObjectWithTag("Arrow").TryGetComponent<SpriteRenderer>(out SpriteRenderer arrow)){
                arrow.enabled = true;
            }
            completedLevel = true;
        }
    }

    public void ChangeLevel(){
        if(GameObject.FindGameObjectWithTag("Elevator").TryGetComponent<Animator>(out Animator anim)){
            anim.SetTrigger("Close");
        }
        if(GameObject.FindGameObjectWithTag("Arrow").TryGetComponent<SpriteRenderer>(out SpriteRenderer arrow)){
                arrow.enabled = false;
            }
        currentFloor++;
        HUDManager.Instance.UpdateLevel(currentFloor);
        completedLevel = false;
        int randomLevel = GetRandomLevelIndex(currentLevel);
        SceneManager.UnloadSceneAsync(currentLevel);
        SceneManager.LoadSceneAsync(randomLevel, LoadSceneMode.Additive);
        currentLevel = randomLevel;
    }

    private int GetRandomLevelIndex(int currentLevel){
        int randomLevel;
        do{
            randomLevel = Random.Range(2, maxLevels);
        } while(randomLevel == currentLevel);
        return randomLevel;
    }

    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
}
