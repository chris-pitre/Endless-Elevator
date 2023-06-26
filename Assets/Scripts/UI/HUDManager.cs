using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] GameObject ones;
    [SerializeField] GameObject tens;
    [SerializeField] GameObject hundreds;
    [SerializeField] Slider healthSlider;
    private static HUDManager _instance;

    public static HUDManager Instance{ get { return _instance;} }

    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        SetMaxHealth();
        SetHealth(Player.max_health);
    }

    public void UpdateLevel(int level){
        int ones_place = level % 10;
        int tens_place = (level % 100 - ones_place) / 10;
        int hundreds_place = (level - tens_place - ones_place) / 100;
        ones.GetComponent<ChangeImage>().ChangeNumber(ones_place);
        tens.GetComponent<ChangeImage>().ChangeNumber(tens_place);
        hundreds.GetComponent<ChangeImage>().ChangeNumber(hundreds_place);
    }

    private void SetMaxHealth(){
        healthSlider.maxValue = Player.max_health;
    }
    public void SetHealth(int health){
        healthSlider.value = health;
    }
}
