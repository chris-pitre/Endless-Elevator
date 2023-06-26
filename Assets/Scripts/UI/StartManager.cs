using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public enum Options{
        START, LEAVE
    }
    public Options CurrentOption = Options.START;

    [SerializeField] RectTransform arrow;

    private void Update(){
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)){
            if(CurrentOption == Options.START){
                arrow.localPosition = new Vector2(-236f, -293f);
                CurrentOption = Options.LEAVE;
            } else {
                arrow.localPosition = new Vector2(-236f, -100f);
                CurrentOption = Options.START;
            }
        }

        if(Input.GetKeyDown(KeyCode.Z)){
            switch(CurrentOption){
                case Options.START:
                    SceneManager.LoadSceneAsync(1);
                    break;
                case Options.LEAVE:
                    Application.Quit();
                    break;
            }
        }
    }
}
